
using Galaxy.HabboHotel.Rooms;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Engine
{
    class MoveAvatarEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            if (!Session.GetHabbo().InRoom)
                return;
		
            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null || !User.CanWalk)
                return;

			if (User.kikando == true)
				User.kikando = false;

            int MoveX = Packet.PopInt();
            int MoveY = Packet.PopInt();

             if (MoveX == User.X && MoveY == User.Y)
                {
                    User.SeatCount++;

                    if (User.SeatCount == 4)
                    {
                        User.SeatCount = 0;
                        return;
                    }
                }

                else { User.SeatCount = 0; }
             

            if (User.RidingHorse)
            {
                RoomUser Horse = Room.GetRoomUserManager().GetRoomUserByVirtualId(User.HorseID);
                if (Horse != null)
                    Horse.MoveTo(MoveX, MoveY);
            }

            User.MoveTo(MoveX, MoveY);

                Session.GetHabbo().lastX = MoveX;
                Session.GetHabbo().lastY = MoveY;
            
        }
    }
}
