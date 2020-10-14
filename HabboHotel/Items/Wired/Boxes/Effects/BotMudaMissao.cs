using System;
using System.Collections.Concurrent;

using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Database.Interfaces;
using System.Data;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class BotMudaMissao : IWiredItem
    {
        public Room Instance { get; set; }

        public Item Item { get; set; }

        public WiredBoxType Type { get { return WiredBoxType.EffectBotMudaMissao; } }

        public ConcurrentDictionary<int, Item> SetItems { get; set; }

        public string StringData { get; set; }

        public string Message2 { get; set; }

        public bool BoolData { get; set; }

        public string ItemsData { get; set; }

        public BotMudaMissao(Room Instance, Item Item)
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

            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null || string.IsNullOrWhiteSpace(StringData))
                return false;

            this.StringData.Split(' ');
            string BotName = this.StringData.Split('	')[0];
            string Chat = this.StringData.Split('	')[1];

            string Message = StringData.Split('	')[1];


            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;

            RoomUser Bot = this.Instance.GetRoomUserManager().GetBotByName(BotName);
            if (Bot == null)
                return false;

            string MissaoBot = Message;
            if (StringData.Contains("%user%")) MissaoBot = MissaoBot.Replace("%user%", Player.Username);
            if (StringData.Contains("%username%")) MissaoBot = MissaoBot.Replace("%username%", Player.Username);
            if (StringData.Contains("%userid%")) MissaoBot = MissaoBot.Replace("%userid%", Convert.ToString(Player.Id));
            if (StringData.Contains("%gotw%")) MissaoBot = MissaoBot.Replace("%gotw%", Convert.ToString(Player.GOTWPoints));
            if (StringData.Contains("%duckets%")) MissaoBot = MissaoBot.Replace("%duckets%", Convert.ToString(Player.Duckets));
            if (StringData.Contains("%diamonds%")) MissaoBot = MissaoBot.Replace("%diamonds%", Convert.ToString(Player.Diamonds));
            if (StringData.Contains("%rank%")) MissaoBot = MissaoBot.Replace("%rank%", Convert.ToString(Player.Rank));
            if (StringData.Contains("%roomname%")) MissaoBot = MissaoBot.Replace("%roomname%", Player.CurrentRoom.Name);
            if (StringData.Contains("%roomusers%")) MissaoBot = MissaoBot.Replace("%roomusers%", Player.CurrentRoom.UserCount.ToString());
            if (StringData.Contains("%roomowner%")) MissaoBot = MissaoBot.Replace("%roomowner%", Player.CurrentRoom.OwnerName.ToString());
            if (StringData.Contains("%roomlikes%")) MissaoBot = MissaoBot.Replace("%roomlikes%", Player.CurrentRoom.Score.ToString());
            if (StringData.Contains("%hotelname%")) MissaoBot = MissaoBot.Replace("%hotelname%", GalaxyServer.HotelName);
            if (StringData.Contains("%versaoGalaxy%")) MissaoBot = MissaoBot.Replace("%versaoGalaxy%", GalaxyServer.VersionGalaxy);
            if (StringData.Contains("%userson%")) MissaoBot = MissaoBot.Replace("%userson%", GalaxyServer.GetGame().GetClientManager().Count.ToString());
            if (StringData.Contains("%dolar%")) MissaoBot = MissaoBot.Replace("%dolar%", GalaxyServer.CotacaoDolar);

            if (MissaoBot == null)
                return false;

            DataRow UserData = null;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `look`,`gender` FROM bots WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("id", Bot.BotData.Id);
                UserData = dbClient.getRow();
            }

            ServerPacket UserChangeComposer = new ServerPacket(ServerPacketHeader.UserChangeMessageComposer);
            UserChangeComposer.WriteInteger(Bot.VirtualId);
            UserChangeComposer.WriteString(UserData["look"].ToString());
            UserChangeComposer.WriteString(UserData["gender"].ToString());
            UserChangeComposer.WriteString(MissaoBot);
            UserChangeComposer.WriteInteger(0);
            this.Instance.SendMessage(UserChangeComposer);

            Bot.BotData.Motto = MissaoBot;

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `bots` SET `motto` = @motto WHERE `id` = '" + Bot.BotData.Id + "' LIMIT 1");
                dbClient.AddParameter("motto", MissaoBot);
                dbClient.RunQuery();
            }

            return true;

        }
    }
}
