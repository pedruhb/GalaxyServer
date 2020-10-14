using Galaxy.HabboHotel.Pathfinding;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class AllEyesOnMeCommand : IChatCommand
    {
        public string PermissionRequired => "command_alleyesonme";
        public string Parameters => ""; 
        public string Description => "Todos olhando para você";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
            foreach (RoomUser U in Users.ToList())
            {
                if (U == null || Session.GetHabbo().Id == U.UserId)
                    continue;

                U.SetRot(Rotation.Calculate(U.X, U.Y, ThisUser.X, ThisUser.Y), false);
            }
        }
    }
}
