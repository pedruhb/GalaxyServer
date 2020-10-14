using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Catalog;

namespace Galaxy.Communication.Packets.Incoming.Catalog
{
    class GetClubGiftsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            Session.SendMessage(new ClubGiftsComposer());
        }
    }
}
