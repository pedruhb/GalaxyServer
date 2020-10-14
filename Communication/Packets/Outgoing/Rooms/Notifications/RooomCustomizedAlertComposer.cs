using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Galaxy.Communication.Packets.Outgoing.Rooms.Notifications
{
    class RoomCustomizedAlertComposer : ServerPacket
    {
        public RoomCustomizedAlertComposer(string Message)
            : base(ServerPacketHeader.RoomCustomizedAlertComposer)
        {
            base.WriteInteger(1);
            base.WriteString(Message);
        }
    }
}