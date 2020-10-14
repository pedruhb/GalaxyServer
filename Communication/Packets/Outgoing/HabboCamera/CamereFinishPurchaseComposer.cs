using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Communication.Packets.Outgoing.HabboCamera
{
    class CamereFinishPurchaseComposer : ServerPacket
    {
        public CamereFinishPurchaseComposer()
            : base(ServerPacketHeader.CamereFinishPurchaseMessageComposer)
        {
        }
    }
}
