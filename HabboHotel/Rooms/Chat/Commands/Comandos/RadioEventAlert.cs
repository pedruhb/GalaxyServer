using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.GameClients;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    internal class RadioEventAlert : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_radio_event_alert";
            }
        }
        public string Parameters
        {
            get
            {
                return "[MENSAGEM]";
            }
        }
        public string Description
        {
            get
            {
                return "Enviar um rádio event alert.";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Session != null)
            {
                if (Room != null)
                {
                    if (Params.Length == 1)
                    {
                        Session.SendWhisper("Por favor, digite uma mensagem para enviar.");
                        return;
                    }
                    else
                    {
                        string Message = CommandManager.MergeParams(Params, 1);

                        GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Novo evento da Rádio " + GalaxyServer.HotelName,
                                 "Está acontecendo um novo evento realizado pela Rádio " + GalaxyServer.HotelName + "! <br><br>Este evento, tem o intuito de proporcionar entretenimento aos usuários!<br><br>Evento:<b>  " + Message +
                                 "</b> <br><br>Caso deseje participar, clique no botão abaixo! <br>",
                                 "eventoradio", "Participar do Evento", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
                    }
                    Room.GetGameMap().GenerateMaps();
                }
            }
        }
    }
}