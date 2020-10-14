using System;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Communication.Packets.Outgoing.Notifications;

namespace Galaxy.Communication.RCON.Commands.Hotel
{
	class StaffAlertCommand : IRCONCommand
	{
		public string Description => "Mandar alerta pro hotel.";
		public string Parameters => "[MENSAGEM]";

		public bool TryExecute(string[] parameters)
		{
			string message = Convert.ToString(parameters[0]);

			GalaxyServer.GetGame().GetClientManager().StaffAlert(new MOTDNotificationComposer("Mensagem da Equipe Staff:\r\r" + message + "\r\n" + "De " + parameters[1] + " ás " + DateTime.Now.ToString("HH:mm")));

			return true;
		}
	}
}