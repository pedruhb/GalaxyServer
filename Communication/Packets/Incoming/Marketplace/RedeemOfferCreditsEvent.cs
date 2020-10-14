using System;
using System.Data;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Database.Interfaces;


namespace Galaxy.Communication.Packets.Incoming.Marketplace
{
    class RedeemOfferCreditsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int DiamondsOwed = 0;

            DataTable Table = null;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `asking_price` FROM `catalog_marketplace_offers` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND `state` = '2'");
                Table = dbClient.getTable();
            }

            if (Table != null)
            {
                foreach (DataRow row in Table.Rows)
                {
                    DiamondsOwed += Convert.ToInt32(row["asking_price"]);
                }

                if (DiamondsOwed >= 1)
                {
                    Session.GetHabbo().Diamonds += DiamondsOwed;
                    Session.SendMessage(new ActivityPointsComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Diamonds, Session.GetHabbo().GOTWPoints));
                    Session.SendMessage(RoomNotificationComposer.SendBubble("diamondsff", "Você acaba de receber " + DiamondsOwed + " Diamantes de vendas na Feira Livre!", ""));
                }

                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("INSERT INTO `log_feira_livre` (`user`, `valor`, `tipo`, `dados`, `data`) VALUES ('" + Session.GetHabbo().Id + "', '" + DiamondsOwed + "', '3', '', '"+GalaxyServer.GetIUnixTimestamp()+"');"); 
                }


                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("DELETE FROM `catalog_marketplace_offers` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND `state` = '2'");
                }
            }
        }
    }
}