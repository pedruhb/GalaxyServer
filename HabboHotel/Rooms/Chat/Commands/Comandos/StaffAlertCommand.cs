using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class StaffAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_staff_alert";
        public string Parameters => "[MENSAGE]";
        public string Description => "Enviar mensage a todos os staff.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite uma mensagem para enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            GalaxyServer.GetGame().GetClientManager().StaffAlert(new MOTDNotificationComposer("Mensagem da Equipe Staff:\r\r" + Message + "\r\n" + "De " + Session.GetHabbo().Username + " ás " + DateTime.Now.ToString("HH:mm")));
            return;
        }
    }
}