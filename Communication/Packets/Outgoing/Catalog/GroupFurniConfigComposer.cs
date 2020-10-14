using System.Collections.Generic;
using Galaxy.HabboHotel.Groups;

namespace Galaxy.Communication.Packets.Outgoing.Catalog
{
    class GroupFurniConfigComposer : ServerPacket
    {
        public GroupFurniConfigComposer(ICollection<Group> Groups)
            : base(ServerPacketHeader.GroupFurniConfigMessageComposer)
        {
			WriteInteger(Groups.Count);
            foreach (Group Group in Groups)
            {
				WriteInteger(Group.Id);
				WriteString(Group.Name);
				WriteString(Group.Badge);
				WriteString(GalaxyServer.GetGame().GetGroupManager().GetColourCode(Group.Colour1, true));
				WriteString(GalaxyServer.GetGame().GetGroupManager().GetColourCode(Group.Colour2, false));
				WriteBoolean(false);
				WriteInteger(Group.CreatorId);
				WriteBoolean(Group.ForumEnabled);
            }
        }
    }
}
