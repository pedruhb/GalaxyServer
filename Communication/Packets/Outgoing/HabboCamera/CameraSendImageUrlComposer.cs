using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Communication.Packets.Outgoing.HabboCamera
{
    class CameraSendImageUrlComposer : ServerPacket
    {
        public CameraSendImageUrlComposer(string url)
            : base(ServerPacketHeader.CameraSendImageUrlMessageComposer)
        {
            base.WriteString(url);
        }
    }
}
