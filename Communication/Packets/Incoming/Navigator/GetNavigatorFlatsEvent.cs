using System.Collections.Generic;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Navigator;
using Galaxy.HabboHotel.Navigator;

namespace Galaxy.Communication.Packets.Incoming.Navigator
{
    class GetNavigatorFlatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            ICollection<SearchResultList> Categories = GalaxyServer.GetGame().GetNavigator().GetEventCategories();

            Session.SendMessage(new NavigatorFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}