using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class FecharQuartoCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_closeroom"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Fecha o quarto."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
                var room = Session.GetHabbo().CurrentRoom;
                room.Access = RoomAccess.DOORBELL;
                room.RoomData.Access = RoomAccess.DOORBELL;
                using (var queryReactor = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    queryReactor.runFastQuery(string.Format("UPDATE rooms SET state = 'locked' WHERE id = {0}",
                        room.RoomId));
                
                Session.SendMessage(new RoomAlertComposer("O quarto foi fechado!"));
                Session.SendWhisper("Comando executado com sucesso!");
            }
        }
    }
