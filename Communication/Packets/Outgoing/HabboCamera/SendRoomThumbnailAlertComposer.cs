using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Communication.Packets.Outgoing.HabboCamera
{
    class SendRoomThumbnailAlertComposer : ServerPacket
    {
        public SendRoomThumbnailAlertComposer()
            : base(ServerPacketHeader.SendRoomThumbnailAlertMessageComposer)
        { 

        }
    }
}
