using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing.Users;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ReloadUserrVIPRankCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_update"; }
        }
        public string Parameters
        {
            get { return "[USUARIO]"; }
        }
        public string Description
        {
            get { return "Dar vip para uma pessoa."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `users` SET `rank` = '2' WHERE `id` = '" + TargetClient.GetHabbo().Id + "'");
                dbClient.runFastQuery("UPDATE `users` SET `rank_vip` = '1' WHERE `id` = '" + TargetClient.GetHabbo().Id + "'");
                TargetClient.GetHabbo().Rank = 2;
                TargetClient.GetHabbo().VIPRank = 1;
            }

            TargetClient.GetHabbo().GetClubManager().AddOrExtendSubscription("club_vip", 1 * 24 * 3600, Session);
            TargetClient.GetHabbo().GetBadgeComponent().GiveBadge("DVIP", true, Session);

            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_VipClub", 1);
            TargetClient.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));

            string figure = TargetClient.GetHabbo().Look;
            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + figure, 3, "O " + Params[1] + " agora é um usuário VIP!", ""));
            Session.SendWhisper("VIP dado com exito!");
        }
    }
}