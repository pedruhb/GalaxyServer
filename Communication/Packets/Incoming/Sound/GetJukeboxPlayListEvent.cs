using Galaxy.Communication.Packets.Outgoing.Sound;
using Galaxy.HabboHotel.GameClients;


namespace Galaxy.Communication.Packets.Incoming.Sound
{
    class GetJukeboxPlayListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().CurrentRoom != null)
                Session.SendMessage(new SetJukeboxPlayListComposer(Session.GetHabbo().CurrentRoom));
            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_MusicPlayer", 1);
        }
    }
}
