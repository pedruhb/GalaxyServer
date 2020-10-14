using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.GameClients;
using System.Linq;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    internal class EventAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_event_alert";
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
                return "Enviar um alerta de evento";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session != null)
            {
                if (Room != null)
                {
                    foreach (GameClient client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
                    {
                        if (Session.GetHabbo().isLoggedIn == false)
                        {
                            Session.SendWhisper("Você não logou como staff!");
                            return;
                        }

                        string Message = CommandManager.MergeParams(Params, 1);

						if (Message == null || Message == "")
							Message = Room.Name;

						GalaxyServer.discordWH(":partying_face: " + Session.GetHabbo().Username + " acaba de iniciar um evento no hotel. (\"" + Message + "\")");

                        if (GalaxyServer.Tipo == 0)
                        {
                            dynamic product = new Newtonsoft.Json.Linq.JObject();
                            product.tipo = "eventalert";
                            product.titulo = "Um evento está acontecendo!";
                            product.subtitulo = "Para desligar digite :eventosoff";
                            product.evento = Message;
                            product.usuario = Session.GetHabbo().Username;
                            product.roomid = Session.GetHabbo().CurrentRoomId;

                            GalaxyServer.GetGame().GetClientManager().EventAlertJson(product);
                            return;
                        }
                        else
						{
                            GalaxyServer.GetGame().GetClientManager().EventAlertPHBLindo(Session.GetHabbo().Username, Session.GetHabbo().CurrentRoomId, Message);
                            return;
                        }
                    }                  
                }
            }
        }
    }
}