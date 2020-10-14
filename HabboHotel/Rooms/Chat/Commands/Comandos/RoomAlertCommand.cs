using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RoomAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_room_alert";
        public string Parameters => "[MENSAGEM]";
        public string Description => "Envie uma mensagem a todos na sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite uma mensagem que você gostaria de enviar para a sala.");
                return;
            }

            if(!Session.GetHabbo().GetPermissions().HasRight("mod_alert") && Room.OwnerId != Session.GetHabbo().Id)
            {
                Session.SendWhisper("Você só pode marcar o alerta em seu próprio quarto!");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
            {
                if (RoomUser == null || RoomUser.GetClient() == null || Session.GetHabbo().Id == RoomUser.UserId)
                    continue;

                RoomUser.GetClient().SendNotification(Session.GetHabbo().Username + " mandou um alerta a sala com a seguinte mensagem:\n\n" + Message);
            }
            Session.SendWhisper("Mensagem enviada com sucesso para a sala.");
        }
    }
}
