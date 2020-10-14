using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Quiz
{
	class CheckQuizTypeEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SafetyQuizGraduate", 1, false);
        }
    }
}
