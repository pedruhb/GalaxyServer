using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class BotCommunicateToUserBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectBotCommunicatesToUserBox; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public BotCommunicateToUserBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            int ChatMode = Packet.PopInt();
            string ChatConfig = Packet.PopString();

            this.StringData = ChatConfig;
            if (ChatMode == 1)
            {
                this.BoolData = true;
            }
            else
            {
                this.BoolData = false;
            }

        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            if (String.IsNullOrEmpty(this.StringData))
                return false;

            this.StringData.Split(' ');
            string BotName = this.StringData.Split('	')[0];
            string Chat = this.StringData.Split('	')[1];

            string Message = StringData.Split('	')[1];
            string MessageFiltered = Message;

			RoomUser User = this.Instance.GetRoomUserManager().GetBotByName(BotName);
            if (User == null)
                return false;

            Habbo Player = (Habbo)Params[0];

			if (StringData.Contains("%user%")) MessageFiltered = MessageFiltered.Replace("%user%", Player.Username);
			if (StringData.Contains("%username%")) MessageFiltered = MessageFiltered.Replace("%username%", Player.Username);
			if (StringData.Contains("%userid%")) MessageFiltered = MessageFiltered.Replace("%userid%", Convert.ToString(Player.Id));
			if (StringData.Contains("%gotw%")) MessageFiltered = MessageFiltered.Replace("%gotw%", Convert.ToString(Player.GOTWPoints));
			if (StringData.Contains("%duckets%")) MessageFiltered = MessageFiltered.Replace("%duckets%", Convert.ToString(Player.Duckets));
			if (StringData.Contains("%diamonds%")) MessageFiltered = MessageFiltered.Replace("%diamonds%", Convert.ToString(Player.Diamonds));
			if (StringData.Contains("%rank%")) MessageFiltered = MessageFiltered.Replace("%rank%", Convert.ToString(Player.Rank));
			if (StringData.Contains("%roomname%")) MessageFiltered = MessageFiltered.Replace("%roomname%", Player.CurrentRoom.Name);
			if (StringData.Contains("%roomusers%")) MessageFiltered = MessageFiltered.Replace("%roomusers%", Player.CurrentRoom.UserCount.ToString());
			if (StringData.Contains("%roomowner%")) MessageFiltered = MessageFiltered.Replace("%roomowner%", Player.CurrentRoom.OwnerName.ToString());
			if (StringData.Contains("%roomlikes%")) MessageFiltered = MessageFiltered.Replace("%roomlikes%", Player.CurrentRoom.Score.ToString());
			if (StringData.Contains("%hotelname%")) MessageFiltered = MessageFiltered.Replace("%hotelname%", GalaxyServer.HotelName);
			if (StringData.Contains("%versaoGalaxy%")) MessageFiltered = MessageFiltered.Replace("%versaoGalaxy%", GalaxyServer.VersionGalaxy);
			if (StringData.Contains("%userson%")) MessageFiltered = MessageFiltered.Replace("%userson%", GalaxyServer.GetGame().GetClientManager().Count.ToString());
			if (StringData.Contains("%dolar%")) MessageFiltered = MessageFiltered.Replace("%dolar%", GalaxyServer.CotacaoDolar);


			if (this.BoolData)
            {
                Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, MessageFiltered, 0, 31));
            }
            else
            {
                User.Chat(MessageFiltered, false, User.LastBubble);
            }

            return true;
        }
    }
}