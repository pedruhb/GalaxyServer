using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System.Collections.Concurrent;
using System.Data;
using System;
namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class GiveFurniUmaVezBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.EffectGiveFurniUmaVezBox; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public GiveFurniUmaVezBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Message = Packet.PopString();

            this.StringData = Message;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null || string.IsNullOrWhiteSpace(StringData))
                return false;

            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;
            ItemData ItemData = null;
            string Message = StringData;

            DataRow Mobis = null;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                 dbClient.SetQuery("SELECT COUNT(ID) as total FROM `givefurniumavezbox` WHERE user_id ='"+Player.Id+"' and room_Id ='"+ Player.GetClient().GetHabbo().CurrentRoomId + "'");
                 Mobis = dbClient.getRow();
            }
            if (Convert.ToInt32(Mobis["total"]) >= 1)
            {
                Player.GetClient().SendWhisper("Você já recebeu o prêmio!");
                return true;
            }
            else
            {
                if (!GalaxyServer.GetGame().GetItemManager().GetItem(int.Parse(Message), out ItemData))
                {
                    Player.GetClient().SendWhisper("O mobi id '" + Message + "' não existe!");
                    return false;
                }
                Item Item = ItemFactory.CreateSingleItemNullable(ItemData, Player.GetClient().GetHabbo(), "", "", 0, 0, 0);
                if (Item != null)
                {
                    Player.GetClient().GetHabbo().GetInventoryComponent().TryAddItem(Item);
                    Player.GetClient().SendMessage(new FurniListNotificationComposer(Item.Id, 1));
                    Player.GetClient().SendMessage(new PurchaseOKComposer());
                    Player.GetClient().SendMessage(new FurniListAddComposer(Item));
                    Player.GetClient().SendMessage(new FurniListUpdateComposer());
                    Player.GetClient().SendWhisper("Você acaba de receber um mobi!");
                    Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("furni/" + Item.Data.ItemName, "Você acaba de receber um mobi!"));
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("INSERT INTO `givefurniumavezbox` (`user_id`, `room_Id`) VALUES ('"+ Player.GetClient().GetHabbo().Id + "', '" + Player.GetClient().GetHabbo().CurrentRoomId + "');");
                    }

                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("INSERT INTO `log_wiredstaff` (`timestamp`, `quantidade`, `user`, `tipo`) VALUES ('" + GalaxyServer.GetUnixTimestamp() + "', @messages, '" + Player.GetClient().GetHabbo().Username + "', 'Mobi (Uma Vez)')");
                        dbClient.AddParameter("messages", Message);
                        dbClient.RunQuery();
                    }

                }
                return true;
            }
        }
    }
}