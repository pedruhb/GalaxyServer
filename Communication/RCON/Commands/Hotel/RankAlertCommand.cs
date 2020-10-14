using System;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Communication.Packets.Outgoing.Notifications;

namespace Galaxy.Communication.RCON.Commands.Hotel
{
	class RankAlertCommand : IRCONCommand
	{
		public string Description => "Mandar alerta pro hotel.";
		public string Parameters => "[MENSAGEM]";

		public bool TryExecute(string[] parameters)
		{
			string message = Convert.ToString(parameters[0]);

			int Rank;
			if (int.TryParse(parameters[2], out Rank))
			{
				GalaxyServer.GetGame().GetClientManager().StaffAlertRank(new MOTDNotificationComposer("Mensagem da Equipe Staff:\r\r" + message + "\r\n" + "De " + parameters[1] + " ás " + DateTime.Now.ToString("HH:mm")), Rank);
			}

			return true;
		}
	}
}
