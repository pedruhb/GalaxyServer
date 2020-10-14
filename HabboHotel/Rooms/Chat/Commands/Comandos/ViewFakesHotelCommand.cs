using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using System.Data;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class ViewFakesHotelCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_view_fakes"; }
		}

		public string Parameters
		{
			get { return ""; }
		}

		public string Description
		{
			get { return "Ver usuários com o mesmo ip onlines no hotel."; }
		}

		public void Execute(GameClients.GameClient session, Rooms.Room Room, string[] Params)
		{

			if (session.GetHabbo().isLoggedIn == false)
			{
				session.SendWhisper("Você não logou como staff!");
				return;
			}

			IQueryAdapter adapter;
			DataTable table = null;
			StringBuilder builder = new StringBuilder();
		
				using (adapter = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					adapter.SetQuery("SELECT DISTINCT ip_last AS ip, (SELECT COUNT(id) FROM users WHERE ip_last = ip AND online = '1') AS contas_fakes, (SELECT GROUP_concat(username) FROM users WHERE ip_last = ip AND online = '1') AS contas FROM users WHERE online = '1';");
					table = adapter.getTable();
					foreach (DataRow row in table.Rows)
					{
					if(System.Convert.ToInt32(row["contas_fakes"]) > 1)
						builder.AppendLine(string.Concat(new object[] { "IP: ", row["ip"], " - Fakes: ", row["contas_fakes"], " - Contas: ", row["contas"] }));
					}
				}
				session.SendMessage(new MOTDNotificationComposer(builder.ToString()));
	
		}
	}
}