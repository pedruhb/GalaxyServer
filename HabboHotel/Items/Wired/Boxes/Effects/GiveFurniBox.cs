using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System.Collections.Concurrent;
namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class GiveFurniBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.EffectGiveFurni; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public GiveFurniBox(Room Instance, Item Item)
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
     
            if (!GalaxyServer.GetGame().GetItemManager().GetItem(int.Parse(Message), out ItemData))
            {
                Player.GetClient().SendWhisper("O mobi id '"+ Message+"' não existe!");
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
                Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("furni/" + Item.Data.ItemName,"Você acaba de receber um mobi!"));
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `log_wiredstaff` (`timestamp`, `quantidade`, `user`, `tipo`) VALUES ('" + GalaxyServer.GetUnixTimestamp() + "', @messagen, '" + Player.GetClient().GetHabbo().Username + "', 'Mobi')");
                    dbClient.AddParameter("messagen", Message);
                    dbClient.RunQuery();
                }
            }
            return true;
        }
    }
}