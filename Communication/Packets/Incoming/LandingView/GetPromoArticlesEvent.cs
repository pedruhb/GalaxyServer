using Galaxy.Communication.Packets.Outgoing.LandingView;
using Galaxy.HabboHotel.LandingView.Promotions;
using System.Collections.Generic;

namespace Galaxy.Communication.Packets.Incoming.LandingView
{
    class GetPromoArticlesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<Promotion> LandingPromotions = GalaxyServer.GetGame().GetLandingManager().GetPromotionItems();

            Session.SendMessage(new PromoArticlesComposer(LandingPromotions));
        }
    }
}
