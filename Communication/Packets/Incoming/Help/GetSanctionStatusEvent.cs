using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Help;

namespace Galaxy.Communication.Packets.Incoming.Help
{
    class GetSanctionStatusEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new SanctionStatusComposer());
        }
    }
}
