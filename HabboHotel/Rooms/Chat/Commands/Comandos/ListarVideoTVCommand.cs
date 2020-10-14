using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using System.Data;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ListarVideoTVCommand : IChatCommand
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
            get { return "Ver os vídeos de sua playlist."; }
        }

        public void Execute(GameClients.GameClient session, Rooms.Room Room, string[] Params)
        {

            IQueryAdapter adapter;
            DataTable table = null;
            StringBuilder builder = new StringBuilder();

            using (adapter = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                adapter.SetQuery("SELECT id,video,canal FROM tv_videos WHERE user = " + session.GetHabbo().Id+" ORDER BY id DESC");
                table = adapter.getTable();

                builder.AppendLine(string.Concat(new object[] { "Listagem de vídeos em sua playlist:" }));
                foreach (DataRow row in table.Rows)
                {
                        builder.AppendLine(string.Concat(new object[] { "ID: ", row["id"], " - Plataforma: ", row["canal"], " - Vídeo ID: ", row["video"].ToString().Replace("pornhub:", "").ToString().Replace("twitch:", "").ToString().Replace("youtube:", "").ToString().Replace("xvideos:", "") }));
                }

                adapter.SetQuery("SELECT COUNT(id) as total FROM tv_videos WHERE user = " + session.GetHabbo().Id + ";");
                builder.AppendLine(string.Concat(new object[] { "Total de vídeos: " + adapter.getString() + "\n\n" }));
                builder.AppendLine(string.Concat(new object[] { "Para remover use :removervideo [id]" }));
            }
            session.SendMessage(new MOTDNotificationComposer(builder.ToString()));

        }
    }
}