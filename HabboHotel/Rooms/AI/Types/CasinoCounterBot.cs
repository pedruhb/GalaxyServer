using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.AI.Types
{
    class CasinoCounter : BotAI
    {
        private int VirtualId;

        public CasinoCounter(int VirtualId)
        {
            this.VirtualId = VirtualId;
        }

        public override void OnSelfEnterRoom()
        {
        }

        public override void OnSelfLeaveRoom(bool Kicked)
        {
        }

        public override void OnUserEnterRoom(RoomUser User)
        {         
        }


        public override void OnUserLeaveRoom(GameClient Client)
        {
        }

        public override void OnUserSay(RoomUser User, string Message)
        {
            if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                return;

            if (Gamemap.TileDistance(GetRoomUser().X, GetRoomUser().Y, User.X, User.Y) > 8)
                return;

            long nowTime = GalaxyServer.CurrentTimeMillis();
            long timeBetween = nowTime - User.GetClient().GetHabbo()._lastTimeUsedHelpCommand;
            if (timeBetween < 60000 && Message.Length == 5)
            {
                User.GetClient().SendMessage(RoomNotificationComposer.SendBubble("furni_placement_error", "Aguarde pelo menos 1 minuto para reutilizar o sistema de revisão de raros.", ""));
                return;
            }

            User.GetClient().GetHabbo()._lastTimeUsedHelpCommand = nowTime;

            string Rare = Message.Split(' ')[2];
            string Username = Message.Split(' ')[4];

            GameClient Target = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Message.Split(' ')[4]);
            if (Target == null)
            {
                GetRoomUser().Chat("Opa, esta pessoa não foi encontrada, não se esqueça de soletrar seu nome.", false, 34);
                return;
            }

            int itemstotal = 0;
            using (IQueryAdapter query = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                query.SetQuery("SELECT COUNT(*) FROM items i LEFT JOIN furniture f ON(i.base_item = f.id) WHERE f.public_name = @itemsito AND i.user_id = @id AND f.is_rare = '1'");
                query.AddParameter("id", Target.GetHabbo().Id);
                query.AddParameter("itemsito", Message.Split(' ')[2]);
                itemstotal = query.getInteger();
            }

            if (itemstotal == 0)
            {
                GetRoomUser().Chat("<font color=\"#DF3A01\"><b>" + Username + "</b> não possui nenhum " + Rare + ", então você não pode apostar com ninguem.</font>", false, 33);
                return;
            }

            GetRoomUser().Chat("<font color=\"#DF3A01\"><b>" + Username + "</b> tem um total de <b>" + itemstotal + "</b> " + Rare + "s.</font>", false, 33);        
        }

        public override void OnUserShout(RoomUser User, string Message)
        {
        }

        public override void OnTimerTick()
        {
        }
    }
}