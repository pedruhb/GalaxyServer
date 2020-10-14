using System.Data;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Users;

namespace Galaxy.Communication.Packets.Incoming.Moderation
{
    class GetModeratorUserInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendNotification("Você não logou como staff!");
                return;
            }

            int UserId = Packet.PopInt();

            DataRow User = null;
            DataRow Info = null;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `id`,`username`,`online`,`mail`,`ip_last`,`look`,`account_created`,`last_online` FROM `users` WHERE `id` = '" + UserId + "' LIMIT 1");
                User = dbClient.getRow();

                if (User == null)
                {
                    Session.SendNotification(GalaxyServer.GetGame().GetLanguageManager().TryGetValue("user.not_found"));
                    return;
                }

                dbClient.SetQuery("SELECT `cfhs`,`cfhs_abusive`,`cautions`,`bans`,`trading_locked`,`trading_locks_count` FROM `user_info` WHERE `user_id` = '" + UserId + "' LIMIT 1");
                Info = dbClient.getRow();
                if (Info == null)
                {
                    dbClient.runFastQuery("INSERT INTO `user_info` (`user_id`) VALUES ('" + UserId + "')");
                    #region Insere HC na conta do usuário
                    Habbo UsuarioPHB = GalaxyServer.GetHabboById(UserId);
                    UsuarioPHB.GetClient().GetHabbo().GetClubManager().AddOrExtendSubscription("habbo_vip", 9999 * 24 * 3600, UsuarioPHB.GetClient());
                    UsuarioPHB.GetClient().GetHabbo().GetBadgeComponent().GiveBadge("HC1", true, UsuarioPHB.GetClient());
                    #endregion 
                    dbClient.SetQuery("SELECT `cfhs`,`cfhs_abusive`,`cautions`,`bans`,`trading_locked`,`trading_locks_count` FROM `user_info` WHERE `user_id` = '" + UserId + "' LIMIT 1");
                    Info = dbClient.getRow();
                }
            }


            Session.SendMessage(new ModeratorUserInfoComposer(User, Info));
        }
    }
}