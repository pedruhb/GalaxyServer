using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System.Collections.Concurrent;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class EnableTeleportUserBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectEnableTeleportUserBox; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public EnableTeleportUserBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;

            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
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

            User.TeleportEnabled = !User.TeleportEnabled;
            if (!User.TeleportEnabled)
            {
                Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("furni_placement_error", "Seu teletransporte foi desativado.", ""));
            }
            else
            {
                Player.GetClient().SendMessage(RoomNotificationComposer.SendBubble("furni_placement_error", "Seu teletransporte foi ativado!", ""));
            }
            Player.CurrentRoom.GetGameMap().GenerateMaps();
            return true;
        }
    }
}