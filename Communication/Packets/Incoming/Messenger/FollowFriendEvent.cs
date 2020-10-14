using Galaxy.HabboHotel.GameClients;

using Galaxy.Communication.Packets.Outgoing.Messenger;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;

namespace Galaxy.Communication.Packets.Incoming.Messenger
{
    class FollowFriendEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            int BuddyId = Packet.PopInt();
            if (BuddyId == 0 || BuddyId == Session.GetHabbo().Id)
                return;

            GameClient Client = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(BuddyId);
            if (Client == null || Client.GetHabbo() == null)
                return;

            if (!Client.GetHabbo().InRoom)
            {
                Session.SendMessage(new FollowFriendFailedComposer(2));
                Session.GetHabbo().GetMessenger().UpdateFriend(Client.GetHabbo().Id, Client, true);
                return;
            }
            else if (Session.GetHabbo().CurrentRoom != null && Client.GetHabbo().CurrentRoom != null)
            {
                if (Session.GetHabbo().CurrentRoom.RoomId == Client.GetHabbo().CurrentRoom.RoomId)
                    return;
            }

            if (Client.GetHabbo().FollowStatus == false && Session.GetHabbo().Rank < 10)
            {
                Session.SendWhisper("O usuário desativou a habilidade de ser seguido.");
                return;
            }

            Session.SendMessage(new RoomForwardComposer(Client.GetHabbo().CurrentRoomId));
        }
    }
}
