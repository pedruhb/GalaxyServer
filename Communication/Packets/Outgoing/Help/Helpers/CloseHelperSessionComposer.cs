namespace Galaxy.Communication.Packets.Outgoing.Help.Helpers
{
	class CloseHelperSessionComposer : ServerPacket
    {
        public CloseHelperSessionComposer()
            : base(ServerPacketHeader.CloseHelperSessionMessageComposer)
        { }
    }
}
