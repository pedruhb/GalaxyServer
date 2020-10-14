using Galaxy.Communication.Packets.Outgoing.HabboCamera;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Communication.Packets.Incoming.HabboCamera
{
    class ParticipatePictureCameraCompetitionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            Session.SendMessage(new CameraFinishParticipateCompetitionComposer());
        }
    }
}
