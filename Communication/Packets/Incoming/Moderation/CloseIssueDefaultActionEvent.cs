using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Moderation
{
    class CloseIssueDefaultActionEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session == null || session.GetHabbo() == null)
                return;

            if (session.GetHabbo().isLoggedIn == false)
            {
                session.SendNotification("Você não logou como staff!");
                return;
            }
        }
    }
}
