
using Galaxy.Communication.Packets.Outgoing.GameCenter;

namespace Galaxy.Communication.Packets.Incoming.GameCenter
{
    class GetPlayableGamesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();

            Session.SendMessage(new GameAccountStatusComposer(GameId));
            Session.SendMessage(new PlayableGamesComposer(GameId));
            Session.SendMessage(new GameAchievementListComposer(Session, GalaxyServer.GetGame().GetAchievementManager().GetGameAchievements(GameId), GameId));
        }
    }
}