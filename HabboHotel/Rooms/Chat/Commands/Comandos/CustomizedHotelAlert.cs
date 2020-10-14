using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class CustomizedHotelAlert : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_customalert"; }
        }

        public string Parameters
        {
            get { return "[MENSAGEM]"; }
        }

        public string Description
        {
            get { return "Enviar uma mensagem custom para todo o Hotel"; }
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
                    Session.SendWhisper("Por favor, indique a mensagem para enviar.");
                    return;
                }

                string Message = CommandManager.MergeParams(Params, 1);
                GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomCustomizedAlertComposer("\n" + Message + "\n\n - " + Session.GetHabbo().Username + ""));
                GalaxyServer.GetGame().GetClientManager().SendMessage(new MassEventComposer(Message));
                Session.SendWhisper("Custom Alerta enviado!");
                return;
        }
    }
}
