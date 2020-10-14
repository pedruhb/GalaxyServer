namespace Galaxy.Communication.Packets.Outgoing.Rooms.Nux
{
    class NuxAlertComposer : ServerPacket
    {
        public NuxAlertComposer(string Message)
            : base(ServerPacketHeader.NuxAlertMessageComposer)
        {
            base.WriteString(Message);
        }
    }
}
