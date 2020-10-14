namespace Galaxy.Communication.Packets.Outgoing.Users
{
	class UserTagsComposer : ServerPacket
    {
        public UserTagsComposer(HabboHotel.GameClients.GameClient Session, int UserId)
            : base(ServerPacketHeader.UserTagsMessageComposer)
        {
            var room = GalaxyServer.GetGame().GetRoomManager().LoadRoom(Session.GetHabbo().CurrentRoomId);
            if (room == null)
                return;

            var roomUserByHabbo = room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
            if (roomUserByHabbo == null || roomUserByHabbo.IsBot)
                return;

            base.WriteInteger(roomUserByHabbo.GetClient().GetHabbo().Id);
            base.WriteInteger(roomUserByHabbo.GetClient().GetHabbo().Tags.Count);//Count of the tags.
            foreach (string current in roomUserByHabbo.GetClient().GetHabbo().Tags)
                base.WriteString(current);

            if (Session != roomUserByHabbo.GetClient())
                return;

            if (Session.GetHabbo().Tags.Count >= 5)
                GalaxyServer.GetGame()
                    .GetAchievementManager()
                    .ProgressAchievement(roomUserByHabbo.GetClient(), "ACH_UserTags", 5, false);
        }
    }
}
