using System.Linq;

using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.Users.Badges;

namespace Galaxy.Communication.Packets.Outgoing.Users
{
	class HabboUserBadgesComposer : ServerPacket
    {
        public HabboUserBadgesComposer(Habbo Habbo)
            : base(ServerPacketHeader.HabboUserBadgesMessageComposer)
        {
			WriteInteger(Habbo.Id);
            WriteInteger(Habbo.GetBadgeComponent().EquippedCount);

            foreach (Badge Badge in Habbo.GetBadgeComponent().GetBadges().ToList())
            {
                if (Badge.Slot <= 0)
                    continue;

				WriteInteger(Badge.Slot);
				WriteString(Badge.Code);
            }
        }
    }
}
