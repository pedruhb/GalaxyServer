using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms.Chat.Commands;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class AbrirQuartoCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_openroom"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Abre o quarto."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            var room = Session.GetHabbo().CurrentRoom;
            room.Access = RoomAccess.OPEN;
            room.RoomData.Access = RoomAccess.OPEN;
            using (var queryReactor = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                queryReactor.runFastQuery(string.Format("UPDATE rooms SET state = 'open' WHERE id = {0}",
                    room.RoomId));

            Session.SendMessage(new RoomAlertComposer("O quarto foi aberto!"));
        }
    }
}
