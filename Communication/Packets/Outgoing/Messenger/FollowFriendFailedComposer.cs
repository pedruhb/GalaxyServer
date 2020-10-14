namespace Galaxy.Communication.Packets.Outgoing.Messenger
{
	class FollowFriendFailedComposer : ServerPacket
    {
        public FollowFriendFailedComposer(int ErrorCode)
            : base(ServerPacketHeader.FollowFriendFailedMessageComposer)
        {
			WriteInteger(ErrorCode);
        }
    }
}
