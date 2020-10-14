using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Galaxy.Communication.Packets.Outgoing.Handshake;

using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class GiveRewardBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectGiveReward; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public GiveRewardBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            int Often = Packet.PopInt();
            bool Unique = (Packet.PopInt() == 1);
            int Limit = Packet.PopInt();
            int Often_No = Packet.PopInt();
            string Reward = Packet.PopString();

            this.BoolData = Unique;
            this.StringData = Reward + "-" + Often + "-" + Limit;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            Habbo Owner = GalaxyServer.GetHabboById(Item.UserID);
            if (Owner == null || !Owner.GetPermissions().HasRight("room_item_wired_rewards"))
                return false;

            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null)
                return false;

            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;

            if (String.IsNullOrEmpty(StringData))
                return false;

            int amountLeft = int.Parse(this.StringData.Split('-')[2]);
            int often = int.Parse(this.StringData.Split('-')[1]);
            bool unique = this.BoolData;

            bool premied = false;

            if (amountLeft == 1)
            {
                Player.GetClient().SendWhisper("Os prêmios acabaram, volte mais tarde.");
                return true;
            }

            foreach (var dataStr in (this.StringData.Split('-')[0]).Split(';'))
            {
                var dataArray = dataStr.Split(',');

                var isbadge = dataArray[0] == "0";
                var code = "";
				code = dataArray[1];
				var percentage = int.Parse(dataArray[2]);

                var random = GalaxyServer.GetRandomNumber(0, 100);

                if (!unique && percentage < random)

                    continue;

                premied = true;


                if (isbadge)
                {

                    if (Player.GetBadgeComponent().HasBadge(code))
                        Player.GetClient().SendWhisper("Oops, parece que você já tem este emblema.");

                    else
                    {
                        Player.GetBadgeComponent().GiveBadge(code, true, Player.GetClient());
                        Player.GetClient().SendWhisper("Você acaba de receber um emblema!");
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("INSERT INTO `log_wiredstaff` (`timestamp`, `quantidade`, `user`, `tipo`) VALUES ('" + GalaxyServer.GetUnixTimestamp() + "', @messaj, '" + Player.GetClient().GetHabbo().Username + "', 'Emblema')");
                            dbClient.AddParameter("messaj", " "+ code+" ");
                            dbClient.RunQuery();
                        }
                    }
                }
                else
                {
                    ItemData ItemData = null;

                    if (!GalaxyServer.GetGame().GetItemManager().GetItem(int.Parse(code), out ItemData))
                    {
                        Player.GetClient().SendWhisper("Item inexistente.");
                        return false;
                    }

                    Item Itemc = ItemFactory.CreateSingleItemNullable(ItemData, Player.GetClient().GetHabbo(), "", "", 0, 0, 0);


                    if (Itemc != null)
                    {
                        Player.GetClient().GetHabbo().GetInventoryComponent().TryAddItem(Itemc);
                        Player.GetClient().SendMessage(new FurniListNotificationComposer(Itemc.Id, 1));
                        Player.GetClient().SendMessage(new PurchaseOKComposer());
                        Player.GetClient().SendMessage(new FurniListAddComposer(Itemc));
                        Player.GetClient().SendMessage(new FurniListUpdateComposer());
                        Player.GetClient().SendWhisper("Você acaba de receber um mobi, confira o seu inventário.");
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("INSERT INTO `log_wiredstaff` (`timestamp`, `quantidade`, `user`, `tipo`) VALUES ('" + GalaxyServer.GetUnixTimestamp() + "', @messagei, '" + Player.GetClient().GetHabbo().Username + "', 'Mobi')");
                            dbClient.AddParameter("messagei", code);
                            dbClient.RunQuery();
                        }
                    }
                }
            }

            if (!premied)
            {
                Player.GetClient().SendWhisper("Mais sorte da próxima vez. :(");
            }
            else if (amountLeft > 1)
            {
                amountLeft--;
                this.StringData.Split('-')[2] = amountLeft.ToString();
            }

            return true;
        }
    }
}