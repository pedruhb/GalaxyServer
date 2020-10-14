using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class ShowAlertPHBox : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.ShowAlertPHBox; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public ShowAlertPHBox(Room Instance, Item Item)
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

            if (StringData.Contains("%USERNAME%"))
                Message = Message.Replace("%USERNAME%", Player.Username);
            if (StringData.Contains("%username%"))
                Message = Message.Replace("%username%", Player.Username);

            if (StringData.Contains("%ROOMNAME%"))
                Message = Message.Replace("%ROOMNAME%", Player.CurrentRoom.Name);
            if (StringData.Contains("%roomname%"))
                Message = Message.Replace("%roomname%", Player.CurrentRoom.Name);

            if (StringData.Contains("%USERCOUNT%"))
                Message = Message.Replace("%USERCOUNT%", Player.CurrentRoom.UserCount.ToString());
            if (StringData.Contains("%usercount%"))
                Message = Message.Replace("%usercount%", Player.CurrentRoom.UserCount.ToString());

            if (StringData.Contains("%USERSONLINE%"))
                Message = Message.Replace("%USERSONLINE%", GalaxyServer.GetGame().GetClientManager().Count.ToString());
            if (StringData.Contains("%usersonline%"))
                Message = Message.Replace("%usersonline%", GalaxyServer.GetGame().GetClientManager().Count.ToString());

            if (StringData.Contains("%USERID%"))
                Message = Message.Replace("%USERID%", Convert.ToString(Player.Id));
            if (StringData.Contains("%userid%"))
                Message = Message.Replace("%userid%", Convert.ToString(Player.Id));

            if (StringData.Contains("%GOTW%"))
                Message = Message.Replace("%GOTW%", Convert.ToString(Player.GOTWPoints));
            if (StringData.Contains("%gotw%"))
                Message = Message.Replace("%gotw%", Convert.ToString(Player.GOTWPoints));

            if (StringData.Contains("%DUCKETS%"))
                Message = Message.Replace("%DUCKETS%", Convert.ToString(Player.Duckets));
            if (StringData.Contains("%duckets%"))
                Message = Message.Replace("%duckets%", Convert.ToString(Player.Duckets));
            if (StringData.Contains("%DIAMONDS%"))
                Message = Message.Replace("%DIAMONDS%", Convert.ToString(Player.Diamonds));
            if (StringData.Contains("%diamonds%"))
                Message = Message.Replace("%diamonds%", Convert.ToString(Player.Diamonds));

            if (StringData.Contains("%RANK%")) // Put names not number
                Message = Message.Replace("%RANK%", Convert.ToString(Player.Rank));
            if (StringData.Contains("%rank%")) // Put names not number
                Message = Message.Replace("%rank%", Convert.ToString(Player.Rank));

            if (StringData.Contains("%LIKESROOM%"))
                Message = Message.Replace("%LIKESROOM%", Player.CurrentRoom.Score.ToString());

            if (StringData.Contains("%likes%"))
                Message = Message.Replace("%likes%", Player.CurrentRoom.Score.ToString());

            Player.GetClient().SendNotification(Message);
            return true;
        }
    }
}