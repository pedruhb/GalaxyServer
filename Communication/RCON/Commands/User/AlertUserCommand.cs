using System;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Moderation;

namespace Galaxy.Communication.RCON.Commands.User
{
    class AlertUserCommand : IRCONCommand
    {
        public string Description
        {
            get { return "Alertar um usuário"; }
        }

        public string Parameters
        {
            get { return "[USERID] [MENSAGEM]"; }
        }

        public bool TryExecute(string[] parameters)
        {
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            // Validate the message
            if (string.IsNullOrEmpty(Convert.ToString(parameters[1])))
                return false;

            string message = Convert.ToString(parameters[1]);

            client.SendMessage(new BroadcastMessageAlertComposer(message));
            return true;
        }
    }
}