using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DisableFriendsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_disable_friends"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Desativar solicitações de amizade."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            Session.GetHabbo().AllowFriendRequests = !Session.GetHabbo().AllowFriendRequests;
            Session.SendWhisper("Agora você " + (Session.GetHabbo().AllowFriendRequests == true ? "pode" : "não pode") + " receber solicitações de amizade.");

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `block_newfriends` = '1' WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.RunQuery();
            }
        }
    }
}