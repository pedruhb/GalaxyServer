using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class GlobalMessageCommand : IChatCommand
    {
        public string PermissionRequired => "command_global_bubble";
        public string Parameters => "[MENSAGEM]";
        public string Description => "Enviar alerta 'BUBBLE' global";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite a mensagem.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            foreach (GameClient client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
            {
                client.SendMessage(new RoomNotificationComposer("command_gmessage", "message", "" + Message + "!"));
            }
        }
    }
}
