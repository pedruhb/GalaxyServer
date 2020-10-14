using Galaxy.HabboHotel.Users.Messenger;

namespace Galaxy.Communication.Packets.Outgoing.Messenger
{
	class FriendNotificationComposer : ServerPacket
    {
        public FriendNotificationComposer(int UserId, MessengerEventTypes type, string data)
            : base(ServerPacketHeader.FriendNotificationMessageComposer)
        {
			WriteString(UserId.ToString());
			WriteInteger(MessengerEventTypesUtility.GetEventTypePacketNum(type));
			WriteString(data);
        }
    }
}
