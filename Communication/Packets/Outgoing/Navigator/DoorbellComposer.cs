namespace Galaxy.Communication.Packets.Outgoing.Navigator
{
	class DoorbellComposer : ServerPacket
    {
        public DoorbellComposer(string Username)
            : base(ServerPacketHeader.DoorbellMessageComposer)
        {
			WriteString(Username);
        }
    }
}
