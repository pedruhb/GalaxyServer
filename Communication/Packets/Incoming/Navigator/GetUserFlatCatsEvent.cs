using System.Collections.Generic;
using Galaxy.HabboHotel.Navigator;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Navigator;

namespace Galaxy.Communication.Packets.Incoming.Navigator
{
    public class GetUserFlatCatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            ICollection<SearchResultList> Categories = GalaxyServer.GetGame().GetNavigator().GetFlatCategories();

            Session.SendMessage(new UserFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}