using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms.Chat.Commands;
using System;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class DesbloquearComando : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_blockcmd"; }
		}

		public string Parameters
		{
			get { return "[COMANDO]"; }
		}

		public string Description
		{
			get { return "Desbloqueia o comando no quarto."; }
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{
			if (Params.Length == 1)
			{
				Session.SendWhisper("Falta o comando que será bloqueado.");
				return;
			}

			string comando = Params[1].ToLower();

			DataRow BlockCMD = null;
			using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
			{

				dbClient.SetQuery("SELECT id FROM room_blockcmd WHERE room = @room AND command = @command LIMIT 1");
				dbClient.AddParameter("command", comando);
				dbClient.AddParameter("room", Session.GetHabbo().CurrentRoomId);
				BlockCMD = dbClient.getRow();
				if (BlockCMD == null)
				{
					Session.SendWhisper("Esse comando não está bloqueado.");
					return;
				}
				else
				{
					dbClient.SetQuery("DELETE FROM room_blockcmd WHERE `room` = @room AND `command` = @command LIMIT 1;");
					dbClient.AddParameter("command", comando);
					dbClient.AddParameter("room", Session.GetHabbo().CurrentRoomId);
					dbClient.RunQuery();

					Session.SendWhisper("O comando foi desbloqueado com sucesso.");
					return;
				}
			}

		}
	}
}