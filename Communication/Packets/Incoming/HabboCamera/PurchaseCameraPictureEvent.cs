using Galaxy.Communication.Packets.Outgoing.HabboCamera;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Rooms.Camera;
using Galaxy.Core;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Galaxy.Communication.Packets.Incoming.HabboCamera
{
    class PurchaseCameraPictureEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int PictureBaseId = 202030;
            var conf = ExtraSettings.CAMERA_ITEMID;
            if (!int.TryParse(conf, out PictureBaseId))
            {
                Session.SendMessage(new RoomNotificationComposer("Por favor, fale com a equipe de desenvolvedores que sua foto não foi identificada.\n Desculpe pelo inconveniente!", "error"));
                return;
            }
            int InsetId;
            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CameraPhotoCount", 1);
            var pic = HabboCameraManager.GetUserPurchasePic(Session);
            ItemData ibase = null;
            if (pic == null || !GalaxyServer.GetGame().GetItemManager().GetItem(PictureBaseId, out ibase))
                return;

            Session.GetHabbo().GetInventoryComponent().AddNewItem(0, ibase.Id, pic.Id.ToString(), 0, true, false, 0, 0);
            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
            
            Session.SendMessage(new CamereFinishPurchaseComposer());

   
            DataRow PHBCamera = null;
            using (var Adap = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                Adap.SetQuery("SELECT COUNT(ID)from server_pictures_publish where picture_id = @pic");
                Adap.AddParameter("pic", pic.Id);
                PHBCamera = Adap.getRow();
            }

            if (Convert.ToInt32(PHBCamera["COUNT(ID)"]) == 0)
            {
                using (var Adap = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    Adap.SetQuery("INSERT INTO `server_pictures_publish` (`picture_id`, `usuario`) VALUES (@pic, @usuario);");
                    Adap.AddParameter("pic", pic.Id);
                    Adap.AddParameter("usuario", Session.GetHabbo().Id);
                    InsetId = (int)Adap.InsertQuery();
                }
            }

        }
    }
}
