using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class EnableFriendsCommand : IChatCommand
    {
        public string PermissionRequired => "command_enable_friends";
        public string Parameters => ""; 
        public string Description => "Ativar solicitaçoes de Amizade.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            
            Session.GetHabbo().AllowFriendRequests = !Session.GetHabbo().AllowFriendRequests;
            Session.SendWhisper("Agora você " + (Session.GetHabbo().AllowFriendRequests == true ? "pode" : "não pode") + " receber solicitações de amizade.");

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `block_newfriends` = '0' WHERE `id` = '" + Session.GetHabbo().Id + "'");
               
                dbClient.RunQuery();
            }
        }
    }
}