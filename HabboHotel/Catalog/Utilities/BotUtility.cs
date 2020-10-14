using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Catalog;
using Galaxy.HabboHotel.Rooms.AI;
using Galaxy.HabboHotel.Users.Inventory.Bots;
using System;
using System.Data;

namespace Galaxy.HabboHotel.Items.Utilities
{
    public static class BotUtility
    {
        public static Bot CreateBot(ItemData Data, int OwnerId)
        {
            DataRow BotData = null;
            CatalogBot CataBot = null;
            if (!GalaxyServer.GetGame().GetCatalog().TryGetBot(Data.Id, out CataBot))
                return null;
            /// Missão Bot
            string MissaoBotSpace = null;
            if (String.IsNullOrEmpty(CataBot.Motto)) { 
                MissaoBotSpace = "Sou um robô!"; } else { MissaoBotSpace = CataBot.Motto; }
            /// Fim Missão bot PHB

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO bots (`user_id`,`name`,`motto`,`look`,`gender`,`ai_type`) VALUES ('" + OwnerId + "', '" + CataBot.Name + "', '" + MissaoBotSpace + "', '" + CataBot.Figure + "', '" + CataBot.Gender + "', '" + CataBot.AIType + "')");
                int Id = Convert.ToInt32(dbClient.InsertQuery());

                dbClient.SetQuery("SELECT `id`,`user_id`,`name`,`motto`,`look`,`gender` FROM `bots` WHERE `user_id` = '" + OwnerId + "' AND `id` = '" + Id + "' LIMIT 1");
                BotData = dbClient.getRow();
            }

            return new Bot(Convert.ToInt32(BotData["id"]), Convert.ToInt32(BotData["user_id"]), Convert.ToString(BotData["name"]), Convert.ToString(BotData["motto"]), Convert.ToString(BotData["look"]), Convert.ToString(BotData["gender"]));
        }


        public static BotAIType GetAIFromString(string Type)
        {
            switch (Type)
            {
                case "pet":
                    return BotAIType.PET;
                case "generic":
                    return BotAIType.GENERIC;
                case "bartender":
                    return BotAIType.BARTENDER;
                case "welcome":
                    return BotAIType.WELCOME;
                case "visitor_logger":
                    return BotAIType.VISITOR_LOGGER;
                case "user_say":
                    return BotAIType.SAY_BOT;
                case "casino_bot":
                    return BotAIType.CASINO_BOT;
                case "roulette_bot":
                    return BotAIType.ROULLETE_BOT;
                default:
                    return BotAIType.GENERIC;
            }
        }
    }
}