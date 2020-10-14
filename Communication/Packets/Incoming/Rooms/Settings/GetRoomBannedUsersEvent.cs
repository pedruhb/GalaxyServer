using Galaxy.HabboHotel.Rooms;
using Galaxy.Communication.Packets.Outgoing.Rooms.Settings;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Settings
{
	class GetRoomBannedUsersEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().Rank > 10 && Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (!Session.GetHabbo().InRoom)
                return;

            Room Instance = Session.GetHabbo().CurrentRoom;
            if (Instance == null || !Instance.CheckRights(Session, true))
                return;

            if (Instance.GetBans().BannedUsers().Count > 0)
                Session.SendMessage(new GetRoomBannedUsersComposer(Instance));
        }
    }
}
