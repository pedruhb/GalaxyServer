using Galaxy.Communication.Packets.Outgoing.HabboCamera;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Communication.Packets.Incoming.HabboCamera
{
    class PublishCameraPictureEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var pic = HabboCameraManager.GetUserPurchasePic(Session);
            if (pic == null)
                return;

            int InsetId;
            using (var Adap = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                Adap.SetQuery("INSERT INTO server_pictures_publish (picture_id, usuario) VALUES (@pic, @usuario)");
                Adap.AddParameter("pic", pic.Id);
                Adap.AddParameter("usuario", Session.GetHabbo().Username);
                InsetId = (int)Adap.InsertQuery();
            }

            Session.SendMessage(new CameraFinishPublishComposer(InsetId));

        }
    }
}
