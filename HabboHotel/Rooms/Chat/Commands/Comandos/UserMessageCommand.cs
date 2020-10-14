using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class UserMessageCommand : IChatCommand
    {
        public string PermissionRequired => "command_alert_user";
        public string Parameters => "[USUÁRIO] [MENSAGEM]";
        public string Description => "Enviar mensagem para um usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome de usuário do usuário.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez eles não estejam online.");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez eles não estejam online.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pdoe manda uma mensagem para você mesmo.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 2);

            TargetClient.SendMessage(new RoomNotificationComposer("command_gmessage", "message", "" + Message + "!"));
            Session.SendMessage(new RoomNotificationComposer("command_gmessage", "message", "Mensagem enviada com sucesso para " + TargetClient.GetHabbo().Username));
        }
    }
}
