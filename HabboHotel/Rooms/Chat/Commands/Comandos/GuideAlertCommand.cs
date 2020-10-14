using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class GuideAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_guide_alert";
        public string Parameters => "[MENSAGEM]";
        public string Description => "Envie uma mensagem de alerta para todos os funcionários on-line.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Session.GetHabbo()._guidelevel < 1)
            {
                Session.SendWhisper("Você não pode enviar alertas para guias, se não estiver rank.");
                return;
              
            }
            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite a mensagem que deseja enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            GalaxyServer.GetGame().GetClientManager().GuideAlert(new MOTDNotificationComposer("[GUÍAS][" + Session.GetHabbo().Username + "]\r\r" + Message));
            return;
        }
    }
}