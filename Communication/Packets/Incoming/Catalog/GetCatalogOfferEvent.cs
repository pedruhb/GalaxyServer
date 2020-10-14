using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.HabboHotel.Catalog;

namespace Galaxy.Communication.Packets.Incoming.Catalog
{
    class GetCatalogOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int OfferId = Packet.PopInt();
            if (!GalaxyServer.GetGame().GetCatalog().ItemOffers.ContainsKey(OfferId))
                return;

            int PageId = GalaxyServer.GetGame().GetCatalog().ItemOffers[OfferId];

            CatalogPage Page;
            if (!GalaxyServer.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                return;

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank  >  Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;

            CatalogItem Item = null;
            if (!Page.ItemOffers.ContainsKey(OfferId))
                return;

            Item = (CatalogItem)Page.ItemOffers[OfferId];
            if (Item != null)
                Session.SendMessage(new CatalogOfferComposer(Item));
        }
    }
}
