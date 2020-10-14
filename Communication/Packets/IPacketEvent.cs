using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets
{
    public interface IPacketEvent
    {
        void Parse(GameClient Session, ClientPacket Packet);
    }
}