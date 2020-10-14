using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Core;

namespace Galaxy.Communication.Packets.Incoming.Moderation
{
    class AmbassadorAlert : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendNotification("Você não logou como staff!");
                return;
            }

            if (Session.GetHabbo().Rank == 1)
                return;

            if (Session.GetHabbo().Rank < ExtraSettings.AmbassadorMinRank)
                return;

            int userId = Packet.PopInt();
            GameClient user = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(userId);
            if (user == null) return;
            user.SendMessage(new SuperNotificationComposer("", "${notification.ambassador.alert.warning.title}", "${notification.ambassador.alert.warning.message}", "", ""));
        }
    }
}