using Galaxy.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms.Camera;
using Galaxy.Communication.Packets.Outgoing.HabboCamera;

namespace Galaxy.Communication.Packets.Incoming.HabboCamera
{
    public class HabboCameraPictureDataEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var len = Packet.PopInt();
            var bytes = Packet.ReadBytes(len);//Not in use when MOD camera
            
            HabboCameraManager.GetUserPurchasePic(Session, true);
            HabboCameraManager.AddNewPicture(Session);
        }
    }
}
