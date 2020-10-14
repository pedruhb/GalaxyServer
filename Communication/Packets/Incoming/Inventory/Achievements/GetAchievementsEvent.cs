
using System.Linq;


using Galaxy.Communication.Packets.Outgoing.Inventory.Achievements;

namespace Galaxy.Communication.Packets.Incoming.Inventory.Achievements
{
    class GetAchievementsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new AchievementsComposer(Session, GalaxyServer.GetGame().GetAchievementManager()._achievements.Values.ToList()));
        }
    }
}
