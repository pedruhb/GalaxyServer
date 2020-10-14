using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DisableEventsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_eventosoff"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ativar ou desativar mensagens de eventos."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().AllowEvents == true)
            {
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `users` SET `allow_events` = 'false' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                    dbClient.RunQuery();
                }
                Session.GetHabbo().AllowEvents = false;
                Session.SendWhisper("Você agora não permite receber alertas de eventos.");
            } else
             
            if (Session.GetHabbo().AllowEvents == false)
            {
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `users` SET `allow_events` = 'true' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                    dbClient.RunQuery();
                }
                Session.GetHabbo().AllowEvents = true;
                Session.SendWhisper("Você agora permite receber alertas de eventos.");
            }

        }
    }
}