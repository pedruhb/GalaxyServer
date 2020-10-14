using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class SendToRoomUserBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.EffectSendToRoomUserBox; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public SendToRoomUserBox(Room Instance, Item Item)
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

            string Message = StringData;
            Room room = null;

            if (Convert.ToInt32(Message) == null)
            {
                Player.GetClient().SendWhisper("Você deve inserir um id de quarto válido!");
                return false;
            }
            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(Convert.ToInt32(Message), out room))
            {
                Player.GetClient().SendWhisper("Oops, o quarto não existe ou está trancado!");
                   return false;
            } else
            {
                if(Convert.ToInt32(Message) == Player.GetClient().GetHabbo().CurrentRoom.RoomId)
                {
                    Player.GetClient().SendWhisper("Você já está neste quarto!");
                    return false;
                }
                Player.GetClient().GetHabbo().PrepareRoom(Convert.ToInt32(Message), "");
                   return true;
            }  
        }
    }
}