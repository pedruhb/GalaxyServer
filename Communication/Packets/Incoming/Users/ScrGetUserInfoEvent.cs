using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Users;
using Galaxy.Communication.Packets.Outgoing.Handshake;

namespace Galaxy.Communication.Packets.Incoming.Users
{
    class ScrGetUserInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
            Session.SendMessage(new UserRightsComposer(Session.GetHabbo()));
        }
    }
}
