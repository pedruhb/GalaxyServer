using System.Drawing;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class WarpRoomCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_warproom"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Traz todos os usuários do quarto para você."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
            {
                if (RoomUser == null || RoomUser.GetClient() == null || Session.GetHabbo().Id == RoomUser.UserId)
                    continue;

                RoomUser SessionTarget = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (SessionTarget == null)
                    return;

                RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (TargetUser == null)
                    return;

                RoomUser.Frozen = true;
                Room.SendMessage(Room.GetRoomItemHandler().UpdateUserOnRoller(RoomUser, new Point(SessionTarget.X, SessionTarget.Y), 0, SessionTarget.Z));

                if (RoomUser.Statusses.ContainsKey("sit"))
                    RoomUser.Z -= 0.35;

                RoomUser.UpdateNeeded = true;
                Room.GetGameMap().GenerateMaps();
                RoomUser.Frozen = false;
            }
        }
    }
}