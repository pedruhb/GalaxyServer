using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class LimparVideoTVCommand : IChatCommand
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
            get { return "Remova todos os vídeos da sua playlist."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("DELETE FROM `tv_videos` WHERE user = @iduser");
                    dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                    dbClient.RunQuery();
                }
                Session.SendWhisper("Sua playlist foi limpa.");
        }
    }
}