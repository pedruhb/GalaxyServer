using Galaxy.HabboHotel.GameClients;
using System.Linq;
using Galaxy.Communication.Packets.Outgoing.Help.Helpers;
using Galaxy.HabboHotel.Helpers;

namespace Galaxy.Communication.Packets.Incoming.Help.Helpers
{
    class CallForHelperEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var category = Packet.PopInt();
            var message = Packet.PopString();

            var helper = HelperToolsManager.GetHelper(Session);
            if (helper != null)
            {
                Session.SendNotification("TEST");
                Session.SendMessage(new CloseHelperSessionComposer());
                return;
            }

            var call = HelperToolsManager.AddCall(Session, message, category);
            var helpers = HelperToolsManager.GetHelpersToCase(call).FirstOrDefault();

            if (helpers != null)
            {
                HelperToolsManager.InvinteHelpCall(helpers, call);
                Session.SendMessage(new CallForHelperWindowComposer(false, call));
                return;
            }

            Session.SendMessage(new CallForHelperErrorComposer(1));

        }
    }
}
