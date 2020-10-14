using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.LandingView.Promotions;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;

namespace Galaxy.HabboHotel.LandingView
{
    public class LandingViewManager
    {
        private static readonly ILog log = LogManager.GetLogger("Galaxy.HabboHotel.LandingView.LandingViewManager");

        private Dictionary<int, Promotion> _promotionItems;

        public LandingViewManager()
        {
            this._promotionItems = new Dictionary<int, Promotion>();

            this.LoadPromotions();
        }

        public void LoadPromotions()
        {
            if (this._promotionItems.Count > 0)
                this._promotionItems.Clear();

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `server_landing` ORDER BY `id` DESC");
                DataTable GetData = dbClient.getTable();

                if (GetData != null)
                {
                    foreach (DataRow Row in GetData.Rows)
                    {
                        this._promotionItems.Add(Convert.ToInt32(Row[0]), new Promotion((int)Row[0], Row[1].ToString().Replace("HOTELNAME", GalaxyServer.HotelName), Row[2].ToString().Replace("HOTELNAME", GalaxyServer.HotelName), Row[3].ToString().Replace("HOTELNAME", GalaxyServer.HotelName), Convert.ToInt32(Row[4]), Row[5].ToString(), Row[6].ToString().Replace("HOTELNAME", GalaxyServer.HotelName)));
                    }
                }
            }


           // log.Info("Landing View Manager -> LOADED");
        }

        public ICollection<Promotion> GetPromotionItems()
        {
            return this._promotionItems.Values;
        }
    }
}