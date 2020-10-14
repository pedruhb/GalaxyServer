using System;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Moderation;

namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class HotelAlertCommand : IRCONCommand
    {
        public string Description => "Mandar alerta pro hotel.";
        public string Parameters => "[MENSAGEM]";

        public bool TryExecute(string[] parameters)
        {
            string message = Convert.ToString(parameters[0]);

			GalaxyServer.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(message + "\r\n" + "Equipe " + GalaxyServer.HotelName));
            return true;
        }
    }
}