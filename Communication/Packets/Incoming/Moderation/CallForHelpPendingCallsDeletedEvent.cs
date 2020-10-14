using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Moderation;

namespace Galaxy.Communication.Packets.Incoming.Moderation
{
    class CallForHelpPendingCallsDeletedEvent : IPacketEvent
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
            if (GalaxyServer.GetGame().GetModerationManager().UserHasTickets(session.GetHabbo().Id))
            {
                ModerationTicket PendingTicket = GalaxyServer.GetGame().GetModerationManager().GetTicketBySenderId(session.GetHabbo().Id);
                if (PendingTicket != null)
                {
                    PendingTicket.Answered = true;
                    GalaxyServer.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(session.GetHabbo().Id, PendingTicket), "mod_tool");
                }
            }
        }
    }
}
