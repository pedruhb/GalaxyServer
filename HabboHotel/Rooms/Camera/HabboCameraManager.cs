using Galaxy.Communication.Packets.Outgoing.HabboCamera;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Camera
{
    public delegate void ImageUrlReceived(string url, HabboCameraPictureRequest Request);

    public static class HabboCameraManager
    {
        public static int RequestIndex { get; private set; }
        public static List<HabboCameraPictureRequest> Requests;

        public static List<HabboCameraPictureData> CachedPictures;

        public static Dictionary<GameClient, HabboCameraPictureData> UsersPic;

        public static string CAMERA_API_HTTP = ExtraSettings.CAMERA_API;
        public static string CAMERA_OUTPUT_PICTURES = ExtraSettings.CAMERA_OUTPUT_PICTURES;

        public static void FlushConfig()
        {
            CAMERA_API_HTTP = ExtraSettings.CAMERA_API;
            CAMERA_OUTPUT_PICTURES = ExtraSettings.CAMERA_OUTPUT_PICTURES;
        }

        public static void Init()
        {
            FlushConfig();

            UsersPic = new Dictionary<GameClient, HabboCameraPictureData>();
            CachedPictures = new List<HabboCameraPictureData>();

            if (Requests == null)
                Requests = new List<HabboCameraPictureRequest>();
        }

        public static HabboCameraPictureData GetPicture(int id)
        {
            if (!CachedPictures.Any(c => c.Id == id))
                return HabboCameraPictureData.Generate(id);

            return CachedPictures.Where(c => c.Id == id).FirstOrDefault();
        }

        public static HabboCameraPictureData GetUserPurchasePic(GameClient client, bool remove = false)
        {
            if (!UsersPic.ContainsKey(client))
                return null;


            var data = UsersPic[client];
            if (remove)
                UsersPic.Remove(client);

            return data;
        }

        public static void AddNewPicture(GameClient Session)
        {
            var Pic = HabboCameraPictureData.Generate(Session);
            Session.SendMessage(new CameraSendImageUrlComposer("?mode=get&name=" + Pic.Id));
            UsersPic.Add(Session, Pic);
        }

        /*public static void AddRequest(GameClient Session, byte[] Data)
        {
            Requests.Add(new HabboCameraPictureRequest(RequestIndex++, Session, Data, OnImageUrlReceived));
        }*/

        private static void OnImageUrlReceived(string url, HabboCameraPictureRequest Request)
        {
            Request.Session.SendMessage(new CameraSendImageUrlComposer(url));
        }
    }
}