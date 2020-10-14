using System;

namespace Galaxy.HabboHotel.Rooms
{
    public delegate void RoomEventDelegate(object sender, EventArgs e);
    public delegate void UserWalksFurniDelegate(object sender, UserWalksOnArgs e);
}