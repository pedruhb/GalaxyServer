using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Rooms.Avatar;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class SetDanceUserBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectDanceUserBox; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public SetDanceUserBox(Room Instance, Item Item)
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
            if (Player == null || Player.GetClient() == null)
                return false;

            RoomUser ThisUser = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Id);
            if (ThisUser == null)
                return false;

            string Dance = StringData;

            if (String.IsNullOrEmpty(StringData))
                return false;

            Player.GetClient().SendMessage(new DanceComposer(ThisUser, Convert.ToInt32(Dance)));

            if (Convert.ToInt32(Dance) == 1)
            {
                Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("advice", "" + Player.GetClient().GetHabbo().Username + ", agora você está dançando Hap-Hop.", ""));
            }
            if (Convert.ToInt32(Dance) == 2)
            {
                Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("advice", "" + Player.GetClient().GetHabbo().Username + ", agora você está dançando Pogo Mogo.", ""));
            }
            if (Convert.ToInt32(Dance) == 3)
            {
                Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("advice", "" + Player.GetClient().GetHabbo().Username + ", agora você está dançando Duck Funk.", ""));
            }
            if (Convert.ToInt32(Dance) == 4)
            {
                Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("advice", "" + Player.GetClient().GetHabbo().Username + ", agora você está dançando Rollie.", ""));
            }


            return true;
        }
    }
}