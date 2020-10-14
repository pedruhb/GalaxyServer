using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using System.Data;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class ComandosBloqueados : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_blockcmd"; }
		}

		public string Parameters
		{
			get { return ""; }
		}

		public string Description
		{
			get { return "Ver os comandos bloqueados do quarto."; }
		}

		public void Execute(GameClients.GameClient session, Rooms.Room Room, string[] Params)
		{

			IQueryAdapter adapter;
			DataTable table = null;
			StringBuilder builder = new StringBuilder();

			using (adapter = GalaxyServer.GetDatabaseManager().GetQueryReactor())
			{
				adapter.SetQuery("SELECT * FROM room_blockcmd WHERE room = '" + session.GetHabbo().CurrentRoomId + "' ORDER BY id DESC");
				table = adapter.getTable();

				builder.AppendLine(string.Concat(new object[] { "Listagem de comandos bloqueados:" }));
				foreach (DataRow row in table.Rows)
				{
					builder.AppendLine(string.Concat(new object[] { "ID: ", row["id"], " - Comando: :", row["command"] }));
				}

				adapter.SetQuery("SELECT COUNT(id) as total FROM room_blockcmd WHERE room = " + session.GetHabbo().CurrentRoomId + ";");
				builder.AppendLine(string.Concat(new object[] { "Total de comandos bloqueados: " + adapter.getString() + "\n\n" }));
				builder.AppendLine(string.Concat(new object[] { "Para remover use :unblockcmd [comando]" }));
			}
			session.SendMessage(new MOTDNotificationComposer(builder.ToString()));

		}
	}
}