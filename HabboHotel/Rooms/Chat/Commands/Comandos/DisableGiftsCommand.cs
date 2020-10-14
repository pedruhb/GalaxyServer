using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DisableGiftsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_disable_gifts"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Permite que você desabilite a capacidade de receber presentes."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            Session.GetHabbo().AllowGifts = !Session.GetHabbo().AllowGifts;
            Session.SendWhisper("Agora você " + (Session.GetHabbo().AllowGifts == true ? "não aceita" : "aceita") + " presentes.");

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_gifts` = @AllowGifts WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("AllowGifts", GalaxyServer.BoolToEnum(Session.GetHabbo().AllowGifts));
                dbClient.RunQuery();
            }
        }
    }
}