using Galaxy.Communication.Packets.Outgoing.Groups;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Groups.Forums;

namespace Galaxy.Communication.Packets.Incoming.Groups
{
    class GetForumStatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var GroupForumId = Packet.PopInt();

            GroupForum Forum;
            if (!GalaxyServer.GetGame().GetGroupForumManager().TryGetForum(GroupForumId, out Forum))
            {
                GalaxyServer.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("forums_thread_hidden", "O fórum que você está tentando acessar não existe mais.", ""));
                return;
            }

            Session.SendMessage(new ForumDataComposer(Forum, Session));

        }
    }
}
