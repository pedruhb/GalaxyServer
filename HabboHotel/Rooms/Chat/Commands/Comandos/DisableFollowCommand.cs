using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DisableFollowCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ativar ou desativar a capacidade de ser seguido."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().FollowStatus == true)
            {
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `users` SET `allow_follow` = 'false' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                    dbClient.RunQuery();
                }
                Session.GetHabbo().FollowStatus = false;
                Session.SendWhisper("Você agora não permite ser seguido.");
            }
            else

            if (Session.GetHabbo().FollowStatus == false)
            {
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `users` SET `allow_follow` = 'true' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                    dbClient.RunQuery();
                }
                Session.GetHabbo().FollowStatus = true;
                Session.SendWhisper("Você agora permite ser seguido.");
            }

        }
    }
}