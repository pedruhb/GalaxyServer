using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.Users.Relationships;
using Galaxy.HabboHotel.Cache.Type;

namespace Galaxy.Communication.Packets.Outgoing.Users
{
	class GetRelationshipsComposer : ServerPacket
    {
        public GetRelationshipsComposer(Habbo Habbo, int Loves, int Likes, int Hates)
            : base(ServerPacketHeader.GetRelationshipsMessageComposer)
        {
			WriteInteger(Habbo.Id);
			WriteInteger(Habbo.Relationships.Count); // Count
            foreach (Relationship Rel in Habbo.Relationships.Values)
            {
                UserCache HHab = GalaxyServer.GetGame().GetCacheManager().GenerateUser(Rel.UserId);
                if (HHab == null)
                {
					WriteInteger(0);
					WriteInteger(0);
					WriteInteger(0); // Their ID
					WriteString("Placeholder");
					WriteString("hr-115-42.hd-190-1.ch-215-62.lg-285-91.sh-290-62");
                }
                else
                {
					WriteInteger(Rel.Type);
					WriteInteger(Rel.Type == 1 ? Loves : Rel.Type == 2 ? Likes : Hates);
					WriteInteger(Rel.UserId); // Their ID
					WriteString(HHab.Username);
					WriteString(HHab.Look);
                }
            }
        }
    }
}