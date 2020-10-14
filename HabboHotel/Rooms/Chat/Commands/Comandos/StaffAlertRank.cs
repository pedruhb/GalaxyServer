using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class StaffAlertRankCommand : IChatCommand
	{
		public string PermissionRequired => "command_hal";
		public string Parameters => "[RANK] [MENSAGEM]";
		public string Description => "Enviar alerta com link";

		public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
		{
			if (Session.GetHabbo().isLoggedIn == false)
			{
				Session.SendWhisper("Você não logou como staff!");
				return;
			}

			if (Params.Length == 2)
			{
				Session.SendWhisper("Por favor, escreva a mensagem.");
				return;
			}

			if (Params.Length == 1)
			{
				Session.SendWhisper("Por favor, escreva o rank.");
				return;
			}

			int Rank;
			if (int.TryParse(Params[1], out Rank))
			{
				string Message = CommandManager.MergeParams(Params, 2);
				GalaxyServer.GetGame().GetClientManager().StaffAlertRank(new MOTDNotificationComposer("Mensagem da Equipe Staff:\r\r" + Message + "\r\n" + "De " + Session.GetHabbo().Username + " ás " + DateTime.Now.ToString("HH:mm")), Rank);
			}
			else Session.SendWhisper("Rank inválido!");
			return;
		}
	}
}
