using System.Collections.Generic;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Quests;
using Galaxy.Communication.Packets.Incoming;

namespace Galaxy.Communication.Packets.Incoming.Quests
{
    public class GetQuestListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            GalaxyServer.GetGame().GetQuestManager().GetList(Session, null);
        }
    }
}