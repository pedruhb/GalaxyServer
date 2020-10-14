﻿using Galaxy.Communication.Packets.Outgoing.Marketplace;

namespace Galaxy.Communication.Packets.Incoming.Marketplace
{
    class GetMarketplaceCanMakeOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new MarketplaceCanMakeOfferResultComposer((Session.GetHabbo().TradingLockExpiry > 0 ? 6 : 1)));
        }
    }
}