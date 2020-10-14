using System.Collections.Generic;
using Galaxy.HabboHotel.Users.Messenger;
using Galaxy.HabboHotel.Cache.Type;

namespace Galaxy.Communication.Packets.Outgoing.Messenger
{
	class BuddyRequestsComposer : ServerPacket
    {
        public BuddyRequestsComposer(ICollection<MessengerRequest> Requests)
            : base(ServerPacketHeader.BuddyRequestsMessageComposer)
        {
			WriteInteger(Requests.Count);
			WriteInteger(Requests.Count);

            foreach (MessengerRequest Request in Requests)
            {
				WriteInteger(Request.From);
				WriteString(Request.Username);

                UserCache User = GalaxyServer.GetGame().GetCacheManager().GenerateUser(Request.From);
				WriteString(User != null ? User.Look : "");
            }
        }
    }
}
