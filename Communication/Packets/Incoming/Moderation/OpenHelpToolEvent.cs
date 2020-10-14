using Galaxy.Communication.Packets.Outgoing.Moderation;

namespace Galaxy.Communication.Packets.Incoming.Moderation
{
    class OpenHelpToolEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendNotification("Você não logou como staff!");
                return;
            }
            Session.SendMessage(new OpenHelpToolComposer());
        }
    }
}
