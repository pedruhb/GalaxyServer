using Galaxy.Communication.Packets.Outgoing.Misc;

namespace Galaxy.Communication.Packets.Incoming.Misc
{
    class LatencyTestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new LatencyTestComposer(Session, Packet.PopInt()));
        }
    }
}
