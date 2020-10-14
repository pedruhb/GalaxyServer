
using Galaxy.Communication.Packets.Outgoing.BuildersClub;
using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogIndexEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            Session.SendMessage(new CatalogIndexComposer(Session, GalaxyServer.GetGame().GetCatalog().GetPages(), "NORMAL"));
            Session.SendMessage(new CatalogItemDiscountComposer());
        }
    }
}