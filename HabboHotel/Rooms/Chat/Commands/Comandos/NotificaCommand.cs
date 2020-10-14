using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using System;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class NotificaCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_notifica"; }
        }

        public string Parameters
        {
            get { return "[NOTIFICAÇÃO]"; }
        }

        public string Description
        {
            get { return "Envia uma notificação a todos os usuários."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
                {
                    Session.SendWhisper("Opa, você deve escolher o modelo da notificação(;notifica lista) que você planeja usar!");
                    return;
                }
                string notificathiago = Params[1];
                string Colour = notificathiago.ToUpper();

                switch (notificathiago)
                {
                    case "lista":
                    case "modelos":
                    case "heapp":
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append("Lista de notificações \r");
                        stringBuilder.Append("------------------------------------------------------------------------------\r");
                        stringBuilder.Append(":notifica normal texto / notifica com a imagem do microfone                            ");
                        stringBuilder.Append(":notifica custom texto / notifica com seu boneco na imagem                             ");
                        stringBuilder.Append(":notifica emblema codigo texto   / notifica um site                                              ");
                        stringBuilder.Append(":notifica quarto texto / notifica usuário é levado ao quarto quando clica             ");
                        stringBuilder.Append("------------------------------------------------------------------------------\r");
                        Session.SendMessage(new MOTDNotificationComposer(stringBuilder.ToString()));
                        break;
                case "normal":
                    case "comum":
                    case "micro":
                        string Message = CommandManager.MergeParams(Params, 2);

                        GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("micro", "message", "" + Message + ""));
                        break;

                    case "custom":
                    case "novo":
                    case "cabeça":
                        string Messagecustom = CommandManager.MergeParams(Params, 2);

                        string figure = Session.GetHabbo().Look;
                        GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, "" + Messagecustom + "", ""));
                        break;

                    case "quarto":
                    case "seguir":
                    case "ir":
                        string Messageseguir = CommandManager.MergeParams(Params, 2);
                        string Messageseguirs = CommandManager.MergeParams(Params, 3);

                        string figureseguir = Session.GetHabbo().Look;
                        GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figureseguir, 3, Messageseguir + Environment.NewLine+ " Clique para ir!", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
                        break;


                    case "emblema":
                    case "git":
                    case "emb":
                        string Messageemblema = Params[2].ToUpper();
                    string Messageemblemas = CommandManager.MergeParams(Params, 3);

                    GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("emblema/" + Messageemblema, 3, "" + Messageemblemas + "", ""));
                        break;
                }
        }
    }
}
