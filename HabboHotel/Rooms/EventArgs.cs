using Galaxy.HabboHotel.Rooms.Games.Teams;
using System;

namespace Galaxy.HabboHotel.Rooms
{
    public class UserSaysArgs : EventArgs
    {
        internal readonly RoomUser user;
        internal readonly string message;

        public UserSaysArgs(RoomUser user, string message)
        {
            this.user = user;
            this.message = message;
        }
    }

    public class ItemTriggeredArgs : EventArgs
    {
        internal readonly RoomUser TriggeringUser;
        internal readonly Room TriggeringItem;

        public ItemTriggeredArgs(RoomUser user, Room item)
        {
            TriggeringUser = user;
            TriggeringItem = item;
        }
    }

    public class TeamScoreChangedArgs : EventArgs
    {
        internal readonly int Points;
        internal readonly TEAM Team;
        internal readonly RoomUser user;

        public TeamScoreChangedArgs(int points, TEAM team, RoomUser user)
        {
            Points = points;
            Team = team;
            this.user = user;
        }
    }

    public class UserWalksOnArgs : EventArgs
    {
        internal readonly RoomUser user;

        public UserWalksOnArgs(RoomUser user)
        {
            this.user = user;
        }
    }
}
