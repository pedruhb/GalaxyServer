using Galaxy.Core;
using System;
using System.Collections.Generic;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RoomMuteCommand : IChatCommand
    {
        public string PermissionRequired => "command_room_mute";
        public string Parameters => "[MENSAGEM]";
        public string Description => "Mutar a sala";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Forneça um motivo para silenciar a sala para mostrar os usuários.");
                return;
            }

            if (!Room.RoomMuted)
                Room.RoomMuted = true;

            string Msg = CommandManager.MergeParams(Params, 1);

            List<RoomUser> RoomUsers = Room.GetRoomUserManager().GetRoomUsers();
            if (RoomUsers.Count > 0)
            {
                foreach (RoomUser User in RoomUsers)
                {
                    if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null || User.GetClient().GetHabbo().Username == Session.GetHabbo().Username)
                        continue;

                    User.GetClient().SendWhisper("Este quarto foi silenciado porque: " + Msg);
                }
            }
        }
    }
}
