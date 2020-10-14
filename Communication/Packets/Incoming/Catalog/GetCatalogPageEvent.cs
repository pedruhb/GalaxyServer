using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.HabboHotel.Catalog;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogPageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int PageId = Packet.PopInt();
            int Something = Packet.PopInt();
            string CataMode = Packet.PopString();

            CatalogPage Page = null;

            if (CataMode == "NORMAL")
            {
                if (!GalaxyServer.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                    return;

                if (!Page.Enabled || !Page.Visible || Page.MinimumRank > Session.GetHabbo().Rank)
                    return;

                if (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1)
                    return;

                    Session.SendMessage(new CatalogPageComposer(Page, CataMode, Session));
            }

            Session.GetHabbo().lastLayout = Page.Template;

        }
    }
}