using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class AlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_alert_user";
        public string Parameters => "[USUARIO] [MENSAGEM]";
        public string Description => "Enviar alerta a um usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome do usuário que deseja alertar.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez ele não esteja online.");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez ele não esteja online.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode manda alerta para você mesmo!");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 2);

            TargetClient.SendNotification(Session.GetHabbo().Username + " alertou você com a seguinte mensagem:\n\n" + Message);
            Session.SendWhisper("Alerta enviada com sucesso para " + TargetClient.GetHabbo().Username);

        }
    }
}
