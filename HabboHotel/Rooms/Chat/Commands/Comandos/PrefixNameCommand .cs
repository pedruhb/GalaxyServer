using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class PrefixNameCommand : IChatCommand
    {

        public string PermissionRequired => "command_event_alert"; 
        public string Parameters => "[PREFIXO]"; 
        public string Description => "Muda sua TAG"; 

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Introduza um prefixo, como por exemplo [ADM]");
                return;
            }

            if (Params[1].ToString().ToLower() == "off")
            {
                Session.GetHabbo()._NamePrefix = "";
                Session.SendWhisper("Você desativou com sucesso");
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `users` SET `prefix_name` = '' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                }
            }
            else
            {
                string PrefixName = CommandManager.MergeParams(Params, 1);
                Session.GetHabbo()._NamePrefix = PrefixName;
                Session.SendWhisper("Alterações salvas");
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `users` SET `prefix_name` = @prefix WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    dbClient.AddParameter("prefix", PrefixName);
                    dbClient.RunQuery();
                }
            }
            return;
        }
    }
}