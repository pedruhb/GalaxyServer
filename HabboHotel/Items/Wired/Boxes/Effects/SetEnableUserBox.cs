using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class SetEnableUserBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectEnableUserBox; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public SetEnableUserBox(Room Instance, Item Item)
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

            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;

            string Message = StringData;

            if (String.IsNullOrEmpty(StringData))
                return false;

            if (Convert.ToInt32(Message) == 592 || Convert.ToInt32(Message) == 595 || Convert.ToInt32(Message) == 597 ||
                Convert.ToInt32(Message) == 594 || Convert.ToInt32(Message) == 599 || Convert.ToInt32(Message) == 44 || 
                Convert.ToInt32(Message) == 178 || Convert.ToInt32(Message) == 23 || Convert.ToInt32(Message) == 24 || 
                Convert.ToInt32(Message) == 25 || Convert.ToInt32(Message) == 26 || Convert.ToInt32(Message) == 548 || 
                Convert.ToInt32(Message) == 531 || Convert.ToInt32(Message) == 102 || Convert.ToInt32(Message) == 187 ||
                Convert.ToInt32(Message) == 593 || Convert.ToInt32(Message) == 596 || Convert.ToInt32(Message) == 598 ||
                Convert.ToInt32(Message) == 39 || Convert.ToInt32(Message) == 38 || Convert.ToInt32(Message) == 55 ||
                Convert.ToInt32(Message) == 56 || Convert.ToInt32(Message) == 97 || Convert.ToInt32(Message) == 27)
            {
                Player.GetClient().SendWhisper("Você não pode usar esse efeito.");
                return true;
            }
               else
            {
            Player.GetClient().GetHabbo().Effects().ApplyEffect(Convert.ToInt32(Message));
            Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("wfeffect", "Você acaba de receber um efeito de um Wired", ""));
            return true;
            }
    
           
        }
    }
}