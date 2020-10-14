
using System.Collections.Generic;

using Galaxy.HabboHotel.Games;
using Galaxy.Communication.Packets.Outgoing.GameCenter;

namespace Galaxy.Communication.Packets.Incoming.GameCenter
{
    class GetGameListingEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<GameData> Games = GalaxyServer.GetGame().GetGameDataManager().GameData;

            Session.SendMessage(new GameListComposer(Games));
        }
    }
}
