using Galaxy.Database.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;

namespace Galaxy.HabboHotel.Subscriptions
{
    public class SubscriptionManager
    {
        private static ILog log = LogManager.GetLogger("Galaxy.HabboHotel.Subscriptions.SubscriptionManager");
        private readonly Dictionary<int, SubscriptionData> _subscriptions = new Dictionary<int, SubscriptionData>();

        public SubscriptionManager()
        {
        }

        public void Init()
        {
            if (_subscriptions.Count > 0)
                _subscriptions.Clear();

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `subscriptions`;");
                DataTable GetSubscriptions = dbClient.getTable();

                if (GetSubscriptions != null)
                {
                    foreach (DataRow Row in GetSubscriptions.Rows)
                    {
                        if (!_subscriptions.ContainsKey(Convert.ToInt32(Row["id"])))
                             _subscriptions.Add(Convert.ToInt32(Row["id"]), new SubscriptionData(Convert.ToInt32(Row["id"]), Convert.ToString(Row["name"]), Convert.ToString(Row["badge_code"]), Convert.ToInt32(Row["credits"]), Convert.ToInt32(Row["duckets"]), Convert.ToInt32(Row["respects"]), Convert.ToInt32(Row["diamonds"])));
                    }
                }
            }
    //        log.Info("» Cobrado: " + _subscriptions.Count + " assinaturas!");
        }

        public bool TryGetSubscriptionData(int Id, out SubscriptionData Data)
        {
            return _subscriptions.TryGetValue(Id, out Data);
        }
    }
}
