using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class VIPAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_staff_alert";
        public string Parameters => "[MENSAGE]";
        public string Description => "Enviar mensagem a todos os vip's.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite uma mensagem para enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            GalaxyServer.GetGame().GetClientManager().VipAlert(new MOTDNotificationComposer("Mensagem da equipe " + GalaxyServer.HotelName + " para usuários vips:\r\r" + Message + "\r\n" + "Equipe "+GalaxyServer.HotelName+" - " + Session.GetHabbo().Username));
            Session.SendMessage(new MOTDNotificationComposer("Mensagem da equipe " + GalaxyServer.HotelName + " para usuários vips:\r\r" + Message + "\r\n" + "Equipe " + GalaxyServer.HotelName + " - " + Session.GetHabbo().Username));
            return;
        }
    }
}