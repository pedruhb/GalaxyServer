
using Galaxy.HabboHotel.Users.Messenger;

namespace Galaxy.Communication.Packets.Outgoing.Messenger
{
	class InstantMessageErrorComposer : ServerPacket
    {
        public InstantMessageErrorComposer(MessengerMessageErrors Error, int Target)
            : base(ServerPacketHeader.InstantMessageErrorMessageComposer)
        {
			WriteInteger(MessengerMessageErrorsUtility.GetMessageErrorPacketNum(Error));
			WriteInteger(Target);
			WriteString("");
        }
    }
}
