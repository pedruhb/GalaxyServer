using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using System.Data;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ViewFakesCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_view_fakes"; }
        }

        public string Parameters
        {
            get { return "[USUARIO]"; }
        }

        public string Description
        {
            get { return "Ver contas fakes de um usuário específico."; }
        }

        public void Execute(GameClients.GameClient session, Rooms.Room Room, string[] Params)
        {

            if (session.GetHabbo().isLoggedIn == false)
            {
                session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                session.SendWhisper("Coloque o nome do usuario a ver as contas fakes.");
                return;
            }

            string str2;
            IQueryAdapter adapter;
            string username = Params[1];
            DataTable table = null;
            StringBuilder builder = new StringBuilder();
            if (GalaxyServer.GetGame().GetClientManager().GetClientByUsername(username) != null)
            {
                str2 = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(username).GetConnection().getIp();
                builder.AppendLine("Usuário:  " + username + " - IP: " + str2);
                using (adapter = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    adapter.SetQuery("SELECT id,username FROM users WHERE ip_last = @ip OR ip_reg = @ip");
                    adapter.AddParameter("ip", str2);
                    table = adapter.getTable();
                    builder.AppendLine("Contas no mesmo IP: " + table.Rows.Count);
                    foreach (DataRow row in table.Rows)
                    {
                        builder.AppendLine(string.Concat(new object[] { "ID : ", row["id"], " - Nick: ", row["username"] }));
                    }
                }
                session.SendMessage(new MOTDNotificationComposer(builder.ToString()));
            }
            else
            {
                using (adapter = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    adapter.SetQuery("SELECT ip_last FROM users WHERE username = @username");
                    adapter.AddParameter("username", username);
                    str2 = adapter.getString();
                    builder.AppendLine("Nick :  " + username + " - IP : " + str2);
                    adapter.SetQuery("SELECT id,username FROM users WHERE ip_last = @ip OR ip_reg = @ip");
                    adapter.AddParameter("ip", str2);
                    table = adapter.getTable();
                    builder.AppendLine("Contas no mesmo IP: " + table.Rows.Count);
                    foreach (DataRow row in table.Rows)
                    {
                        builder.AppendLine(string.Concat(new object[] { "ID : ", row["id"], " - Nick: ", row["username"] }));
                    }
                }

                session.SendMessage(new MOTDNotificationComposer(builder.ToString()));
            }
            return;
        }
    }
}