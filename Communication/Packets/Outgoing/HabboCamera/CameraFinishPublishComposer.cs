using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Communication.Packets.Outgoing.HabboCamera
{
    class CameraFinishPublishComposer : ServerPacket
    {
        public CameraFinishPublishComposer(int PicId) : base(ServerPacketHeader.CameraFinishPublishMessageComposer)
        {
            base.WriteBoolean(true);
            base.WriteInteger(1);
            base.WriteString(PicId.ToString());
        }
    }
}
