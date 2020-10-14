using Galaxy.HabboHotel.Moderation;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Database.Interfaces;

namespace Galaxy.Communication.Packets.Incoming.Moderation
{
    class CloseTicketEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendNotification("Você não logou como staff!");
                return;
            }

            int Result = Packet.PopInt(); // 1 = useless, 2 = abusive, 3 = resolved
            int Junk = Packet.PopInt();
            int TicketId = Packet.PopInt();

            ModerationTicket Ticket = null;
            if (!GalaxyServer.GetGame().GetModerationManager().TryGetTicket(TicketId, out Ticket))
                return;

            if (Ticket.Moderator.Id != Session.GetHabbo().Id)
                return;

            GameClient Client = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(Ticket.Sender.Id);
            if (Client != null)
            {
                Client.SendMessage(new ModeratorSupportTicketResponseComposer(Result));
            }

            if (Result == 2)
            {
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `user_info` SET `cfhs_abusive` = `cfhs_abusive` + 1 WHERE `user_id` = '" + Ticket.Sender.Id + "' LIMIT 1");
                }
            }
            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Ticket.Sender.Username);

            if (Result == 3)
                TargetClient.SendNotification("Sua denúnicia foi fechada como resolvido! obrigado por usar a ferramenta.");
            if (Result == 2)
                TargetClient.SendNotification("Sua denúnicia foi fechada como abusiva e você poderá sofrer consequências por causa disso.");
            if (Result == 1)
                TargetClient.SendNotification("Sua denúnicia foi fechada como inútil, só use a ferramenta quando houver necessidade! evite ser punido fazendo isso.");

            Ticket.Answered = true;
            GalaxyServer.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(Session.GetHabbo().Id, Ticket), "mod_tool");
        }
    }
}