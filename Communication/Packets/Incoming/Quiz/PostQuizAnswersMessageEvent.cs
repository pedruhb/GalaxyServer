using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Quiz;

namespace Galaxy.Communication.Packets.Incoming.Quiz
{
    class PostQuizAnswersMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new PostQuizAnswersMessageComposer(Session));
        }
    }
}
