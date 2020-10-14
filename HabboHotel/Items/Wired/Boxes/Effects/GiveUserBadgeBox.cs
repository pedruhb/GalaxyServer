using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class GiveUserBadgeBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.EffectGiveUserBadge; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public GiveUserBadgeBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Badge = Packet.PopString();

            this.StringData = Badge;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;
            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null)
                return false;
			RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;
			if (String.IsNullOrEmpty(StringData))
                return false;
			if (Player.GetBadgeComponent().HasBadge(Convert.ToString(StringData)))
                Player.GetClient().SendWhisper("Parece que você já tem esse emblema no inventário.");
            else
            {
				
                Player.GetBadgeComponent().GiveBadge(Convert.ToString(StringData), true, Player.GetClient());
                Player.GetClient().SendWhisper("Você recebeu um emblema.");

                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("INSERT INTO `log_wiredstaff` (`timestamp`, `quantidade`, `user`, `tipo`) VALUES ('" + GalaxyServer.GetUnixTimestamp() + "', @messagem, '" + Player.GetClient().GetHabbo().Username + "', 'Emblema')");
                    dbClient.AddParameter("messagem", " "+StringData+ " ");
                    dbClient.RunQuery();
                }

                Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("emblema/" + StringData, "Você acaba de receber um emblema!", "/inventory/open/badge"));
              
            }

            return true;
        }
    }
}