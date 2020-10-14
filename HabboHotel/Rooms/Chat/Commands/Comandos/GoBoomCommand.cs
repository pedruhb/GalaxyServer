using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class GoBoomCommand :IChatCommand
    {
        public string PermissionRequired => "command_goboom";
        public string Parameters => "";
        public string Description => "Explode todos (Efeito 108)";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
            if (Users.Count > 0)
            {
                foreach (RoomUser U in Users.ToList())
                {
                    if (U == null)
                        continue;

                    U.ApplyEffect(108);
                }
            }
        }
    }
}
