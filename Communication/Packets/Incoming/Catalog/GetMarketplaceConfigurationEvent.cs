﻿using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Catalog
{
    public class GetMarketplaceConfigurationEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new MarketplaceConfigurationComposer());
        }
    }
}