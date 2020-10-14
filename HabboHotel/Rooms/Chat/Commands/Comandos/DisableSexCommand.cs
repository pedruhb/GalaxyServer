using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DisableSexCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_disable_sex"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ativar ou desativar o comando sexo."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowSex = !Session.GetHabbo().AllowSex;
            Session.SendWhisper("Você " + (Session.GetHabbo().AllowSex == true ? "ativou" : "desativou") + " o comando :sexo.");
            Session.SendWhisper("Para " + (Session.GetHabbo().AllowSex == true ? "desativar" : "ativar") + " novamente utilize o mesmo comando.");
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_sex` = @allowsex WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("allowsex", GalaxyServer.BoolToEnum(Session.GetHabbo().AllowSex));
                dbClient.RunQuery();
            }
        }
    }
}