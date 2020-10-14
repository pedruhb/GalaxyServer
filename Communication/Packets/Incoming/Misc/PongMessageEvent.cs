using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Galaxy.Communication.Packets.Outgoing.Misc;

namespace Galaxy.Communication.Packets.Incoming.Misc
{
    class PongMessageEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.TimePingedReceived = DateTime.Now;
        }
    }
}
