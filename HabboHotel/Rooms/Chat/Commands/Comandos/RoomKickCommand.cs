using Galaxy.Core;
using System;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RoomKickCommand : IChatCommand
    {
        public string PermissionRequired => "command_roomkick";
        public string Parameters => "[MENSAGEM]";
        public string Description => "Kicka todos os usuários nessa sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, forneça um motivo kick todos da sala.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (RoomUser == null || RoomUser.IsBot || RoomUser.GetClient() == null || RoomUser.GetClient().GetHabbo() == null || RoomUser.GetClient().GetHabbo().GetPermissions().HasRight("mod_tool") || RoomUser.GetClient().GetHabbo().Id == Session.GetHabbo().Id)
                    continue;

                RoomUser.GetClient().SendNotification("Você foi kickado por um moderador: " + Message);

                Room.GetRoomUserManager().RemoveUserFromRoom(RoomUser.GetClient(), true, false);
            }

            Session.SendWhisper("Você expulsou com sucesso todos os usuários da sala.");
        }
    }
}
