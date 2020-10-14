using Galaxy.Communication.Packets.Outgoing.Catalog;

namespace Galaxy.Communication.Packets.Incoming.Catalog
{
	class GetCatalogModeEvent : IPacketEvent
    {
       public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string PageMode = Packet.PopString();

            if (PageMode == "NORMAL")
                Session.SendMessage(new CatalogIndexComposer(Session, GalaxyServer.GetGame().GetCatalog().GetPages(), PageMode));//, Sub));

        }
    }
}
