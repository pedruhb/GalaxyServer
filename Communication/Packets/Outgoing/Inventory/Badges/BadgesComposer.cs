﻿using System.Linq;
using System.Collections.Generic;

using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Users.Badges;

namespace Galaxy.Communication.Packets.Outgoing.Inventory.Badges
{
	class BadgesComposer : ServerPacket
    {
        public BadgesComposer(GameClient Session)
            : base(ServerPacketHeader.BadgesMessageComposer)
        {
            List<Badge> EquippedBadges = new List<Badge>();

			WriteInteger(Session.GetHabbo().GetBadgeComponent().Count);
            foreach (Badge Badge in Session.GetHabbo().GetBadgeComponent().GetBadges().ToList())
            {
				WriteInteger(1);
				WriteString(Badge.Code);

                if (Badge.Slot > 0)
                    EquippedBadges.Add(Badge);
            }

			WriteInteger(EquippedBadges.Count);
            foreach (Badge Badge in EquippedBadges)
            {
				WriteInteger(Badge.Slot);
				WriteString(Badge.Code);
            }
        }
    }
}
