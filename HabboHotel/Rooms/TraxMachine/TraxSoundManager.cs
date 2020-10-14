using log4net;
using System.Collections.Generic;
using System.Data;

namespace Galaxy.HabboHotel.Rooms.TraxMachine
{
    public class TraxSoundManager
    {
        public static List<TraxMusicData> Songs = new List<TraxMusicData>();

        private static ILog Log = LogManager.GetLogger("Galaxy.HabboHotel.Rooms.TraxMachine");
        public static void Init()
        {
            Songs.Clear();

            DataTable table;
            using (var adap = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                adap.RunQuery("SELECT * FROM jukebox_songs_data");
                table = adap.getTable();
            }

            foreach (DataRow row in table.Rows)
            {
                Songs.Add(TraxMusicData.Parse(row));
            }

    //        Log.Info("» Jukebox -> PRONTO!");
        }

        public static TraxMusicData GetMusic(int id)
        {
            foreach (var item in Songs)
                if (item.Id == id)
                    return item;

            return null;
        }
    }
}
