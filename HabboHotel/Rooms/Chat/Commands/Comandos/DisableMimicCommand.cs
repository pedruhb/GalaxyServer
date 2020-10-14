using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DisableMimicCommand : IChatCommand
    {
        public string PermissionRequired => "command_disable_mimic";
        public string Parameters => "";
        public string Description => "Desativa comando :copiar.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            
            Session.GetHabbo().AllowMimic = !Session.GetHabbo().AllowMimic;
            Session.SendWhisper("Agora você " + (Session.GetHabbo().AllowMimic == true ? "pode" : "não pode") + " ser copiado.");

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_mimic` = @AllowMimic WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("AllowMimic", GalaxyServer.BoolToEnum(Session.GetHabbo().AllowMimic));
                dbClient.RunQuery();
            }
        }
    }
}