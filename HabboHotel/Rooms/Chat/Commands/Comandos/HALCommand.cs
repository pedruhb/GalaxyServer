using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class HALCommand : IChatCommand
    {
        public string PermissionRequired => "command_hal";
        public string Parameters => "[LINK] [MENSAGEM]";
        public string Description => "Enviar alerta com link";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 2)
            {
                Session.SendWhisper("Por favor, escreva uma mensagem e uma URL para enviar.");
                return;
            }

            string URL = Params[1];
            string Message = CommandManager.MergeParams(Params, 2);
            GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Alerta com link - " + GalaxyServer.HotelName + " Hotel!", Message + "\r\n" + "Equipe "+GalaxyServer.HotelName+" - " + Session.GetHabbo().Username, "", "Clique aqui", URL));
            return;
        }
    }
}
