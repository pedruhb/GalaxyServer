using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Communication.Packets.Outgoing.HabboCamera
{
    class CameraFinishParticipateCompetitionComposer : ServerPacket
    {
        public CameraFinishParticipateCompetitionComposer()
            : base(ServerPacketHeader.CameraFinishParticipateCompetitionMessageComposer)
        {
            base.WriteBoolean(true);
            base.WriteString("Teste");
        }
    }
}
