using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Groups.Forums;

namespace Galaxy.Communication.Packets.Outgoing.Groups
{
	class ThreadUpdatedComposer : ServerPacket
    {
        public ThreadUpdatedComposer(GameClient Session, GroupForumThread Thread)
            : base(ServerPacketHeader.ThreadUpdatedMessageComposer)
        {
			WriteInteger(Thread.ParentForum.Id);

            Thread.SerializeData(Session, this);
        }
    }
}
