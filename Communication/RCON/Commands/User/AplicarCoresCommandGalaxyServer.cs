
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms;

namespace Galaxy.Communication.RCON.Commands.User
{
    class AplicarTagCorGalaxyServer : IRCONCommand
    {
        public string Description
        {
            get { return "Comando para atualizar cores de nome e tag do usuário."; }
        }

        public string Parameters
        {
            get { return "%userId% %tagcor% %prefixcor%"; }
        }

        public bool TryExecute(string[] parameters)
        {

            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            client.SendMessage(RoomNotificationComposer.SendBubble("advice", "Você atualizou as cores da tag e nome!", ""));
            client.GetHabbo()._NamePrefixColor = parameters[2];
            client.GetHabbo().chatHTMLColour = parameters[1];
            return true;
        }
    }
}