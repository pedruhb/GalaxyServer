using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class AlertarQuartoCommand : IChatCommand
    {
        public string PermissionRequired => "command_alertarquarto";
        public string Parameters => "[MENSAGEM]";
        public string Description => "Envie uma mensagem a todos na sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Você só pode usar o comando em seu quarto.");
                return;

            }

          if (Params.Length == 1)
            {
                Session.SendWhisper("Digite uma mensagem que você gostaria de enviar para a sala.");
                return;
            }
            string Message = CommandManager.MergeParams(Params, 1);
            foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
            {
                if (RoomUser == null || RoomUser.GetClient() == null || Session.GetHabbo().Id == RoomUser.UserId)
                    continue;

                RoomUser.GetClient().SendNotification("O usuário " +Session.GetHabbo().Username + " alertou a sala:\n\n" + Message);
            }

            Session.SendWhisper("Mensagem enviada com sucesso para a sala.");
        }
    }
}
