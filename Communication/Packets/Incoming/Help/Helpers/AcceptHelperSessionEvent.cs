﻿using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Helpers;


namespace Galaxy.Communication.Packets.Incoming.Help.Helpers
{
    class AcceptHelperSessionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var Accepted = Packet.PopBoolean();
            var Helper = HelperToolsManager.GetHelper(Session);

            if (Helper == null)
            {
                Session.SendMessage(new Outgoing.Help.Helpers.CloseHelperSessionComposer());
                return;
            }

            if (Accepted)
                Helper.Accept();
            else
                Helper.Decline();



        }
    }
}
