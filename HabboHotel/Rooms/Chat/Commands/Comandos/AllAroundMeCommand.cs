using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class AllAroundMeCommand : IChatCommand
    {
        public string PermissionRequired => "command_allaroundme";
        public string Parameters => "";
        public string Description => "Vire em volta para você.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
            foreach (RoomUser U in Users.ToList())
            {
                if (U == null || Session.GetHabbo().Id == U.UserId)
                    continue;

                U.MoveTo(User.X, User.Y, true);
            }
        }
    }
}
