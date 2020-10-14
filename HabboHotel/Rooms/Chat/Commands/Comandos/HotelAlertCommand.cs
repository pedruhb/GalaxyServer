using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class HotelAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_hotel_alert";
        public string Parameters => "[MENSAGEM]";
        public string Description => "Enviar alerta para hotel.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, introduza un mensage para enviar.");
                return;
            }
            int OnlineUsers = GalaxyServer.GetGame().GetClientManager().Count;
            int RoomCount = GalaxyServer.GetGame().GetRoomManager().Count;
            string Message = CommandManager.MergeParams(Params, 1);
            GalaxyServer.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(Message +  "\r\n" + "Equipe "+GalaxyServer.HotelName+" - " + Session.GetHabbo().Username));
            return;
        }
    }
}
