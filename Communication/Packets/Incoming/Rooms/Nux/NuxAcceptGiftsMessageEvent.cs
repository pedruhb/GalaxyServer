using Galaxy.Communication.Packets.Outgoing.Rooms.Nux;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Nux
{
    class NuxAcceptGiftsMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new NuxItemListComposer());
        }
    }
}