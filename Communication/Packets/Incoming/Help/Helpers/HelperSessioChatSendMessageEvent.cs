using Galaxy.Communication.Packets.Outgoing.Help.Helpers;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Helpers;

namespace Galaxy.Communication.Packets.Incoming.Help.Helpers
{
    class HelperSessioChatSendMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var Element = HelperToolsManager.GetElement(Session);
            var message = Packet.PopString();
            if (Element.OtherElement != null)
            {
                Session.SendMessage(new HelperSessionSendChatComposer(Session.GetHabbo().Id, message));
                Element.OtherElement.Session.SendMessage(new HelperSessionSendChatComposer(Session.GetHabbo().Id, message));
            }
            else
            {
                Session.SendMessage(new CallForHelperErrorComposer(0));
            }
        }
    }
}
