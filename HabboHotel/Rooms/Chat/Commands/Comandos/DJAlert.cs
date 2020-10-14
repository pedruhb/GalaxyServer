using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DJAlert : IChatCommand
    {
        public string PermissionRequired => "command_djalert";
        public string Parameters => "[NICK]";
        public string Description => "Envie um alerta de DJ para todo o Hotel.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Escreva o nick do dj.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            GalaxyServer.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("DJAlertNEW", "O " + Message + " está ao vivo na rádio " + GalaxyServer.HotelName+ "!", ""));
            return;
        }
    }
}
