using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using System.Data;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class UsersOncommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_userson"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ver usuários online."; }
        }

        public void Execute(GameClients.GameClient session, Rooms.Room Room, string[] Params)
        {

            if (session.GetHabbo().isLoggedIn == false)
            {
                session.SendWhisper("Você não logou como staff!");
                return;
            }

            string str2;
            IQueryAdapter adapter;
            string username = "a";
            DataTable table = null;
            StringBuilder builder = new StringBuilder();
            if (GalaxyServer.GetGame().GetClientManager().GetClientByUsername(username) != null)
            {
                str2 = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(username).GetConnection().getIp();
                builder.AppendLine("Listagem de todos os usuários onlines no "+GalaxyServer.HotelName);
                using (adapter = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    adapter.SetQuery("Select * from users where online = '1'");
                    table = adapter.getTable();
                    foreach (DataRow row in table.Rows)
                    {
                        builder.AppendLine(string.Concat(new object[] { "ID : ", row["id"], " - Nick: ", row["username"], " - IP: ", row["ip_last"], }));
                    }
                }
                session.SendMessage(new MOTDNotificationComposer(builder.ToString()));
            }
            else
            {
                builder.AppendLine("Listagem de todos os usuários onlines no " + GalaxyServer.HotelName);
                using (adapter = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    adapter.SetQuery("Select * from users where online = '1'");
                    table = adapter.getTable();
                    foreach (DataRow row in table.Rows)
                    {
                        builder.AppendLine(string.Concat(new object[] { "ID : ", row["id"], " - Nick: ", row["username"], " - IP: ", row["ip_last"], }));
                    }
                }

                session.SendMessage(new MOTDNotificationComposer(builder.ToString()));
            }
            return;
        }
    }
}