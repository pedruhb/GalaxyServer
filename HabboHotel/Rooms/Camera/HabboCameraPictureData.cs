using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System.Data;

namespace Galaxy.HabboHotel.Rooms.Camera
{
    public class HabboCameraPictureData
    {
        public int Id;
        public int UserId;
        public long Timestamp;
        public string Url;

        public HabboCameraPictureData(int id, int userid, long timestamp, string url)
        {
            this.Id = id;
            this.UserId = userid;
            this.Timestamp = timestamp;
            this.Url = url;
        }

        public static HabboCameraPictureData Generate(GameClient Client)
        {
            int id;
            var now = GalaxyServer.Now();
            var url = "";
            using (var adap = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                adap.SetQuery(string.Concat("INSERT INTO server_pictures (user_id, timestamp) VALUES ('", Client.GetHabbo().Id, "', '", now, "')"));
                id = (int)adap.InsertQuery();
                url = ExtraSettings.CAMERA_OUTPUT_PICTURES + id + ".png";

                using (var adap2 = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    adap2.runFastQuery("UPDATE server_pictures SET url = '" + url + "' WHERE id = '" + id + "'");
                }
            }
            return new HabboCameraPictureData(id, Client.GetHabbo().Id, (long)now, url);
        }

        public static HabboCameraPictureData Generate(int id)
        {
            DataTable table;
            using (var adap = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                adap.RunQuery("SELECT * FROM server_pictures WHERE id = '" + id + "'");
                table = adap.getTable();
            }

            if (table.Rows.Count == 0)
                return null;

            var row = table.Rows[0];

            var pid = int.Parse(row["id"].ToString());
            var userid = int.Parse(row["user_id"].ToString());
            var time = long.Parse(row["timestamp"].ToString());
            var url = row["url"].ToString();

            var pic = new HabboCameraPictureData(pid, userid, time, url);
            HabboCameraManager.CachedPictures.Add(pic);
            return pic;

        }
    }
}
