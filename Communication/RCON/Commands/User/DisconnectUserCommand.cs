using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.RCON.Commands.User
{
    class DisconnectUserCommand : IRCONCommand
    {
        public string Description
        {
            get { return "Desconectar um usuário"; }
        }

        public string Parameters
        {
            get { return "[USERID]"; }
        }

        public bool TryExecute(string[] parameters)
        {
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            client.Disconnect();
            return true;
        }
    }
}
