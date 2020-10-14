using System.Collections.Generic;
using log4net;
using Galaxy.Database.Interfaces;
using System;
using System.Data;

namespace Galaxy.HabboHotel.Badges
{
    public class BadgeManager
    {
        private static readonly ILog log = LogManager.GetLogger("Galaxy.HabboHotel.Badges.BadgeManager");

        private readonly Dictionary<string, BadgeDefinition> _badges;

        public BadgeManager()
        {
			_badges = new Dictionary<string, BadgeDefinition>();
        }

        public void Init()
        {
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `badge_definitions`;");
                DataTable GetBadges = dbClient.getTable();

                foreach (DataRow Row in GetBadges.Rows)
                {
                    string BadgeCode = Convert.ToString(Row["code"]).ToUpper();

                    if (!_badges.ContainsKey(BadgeCode))
						_badges.Add(BadgeCode, new BadgeDefinition(BadgeCode, Convert.ToString(Row["required_right"])));
                }
            }

            log.Info("» Carregado " + _badges.Count + " emblemas.");
        }

        public bool TryGetBadge(string BadgeCode, out BadgeDefinition Badge)
        {
            return _badges.TryGetValue(BadgeCode.ToUpper(), out Badge);
        }
    }
}