using System;
using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Moderation;
using Galaxy.Database.Interfaces;
using Galaxy.Core;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class IPBanCommand : IChatCommand
    {
        public string PermissionRequired => "command_ip_ban";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Banir usuário por IP.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome de usuário do usuário que deseja Ban IP e banar conta.");
                return;
            }

            Habbo Habbo = GalaxyServer.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Usuário não encontrado no banco de dados.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Uau, você não pode proibir o usuário.");
                return;
            }

            String IPAddress = String.Empty;
            Double Expire = GalaxyServer.GetUnixTimestamp() + 78892200;
            string Username = Habbo.Username;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
                dbClient.runFastQuery("UPDATE users SET rank = '1' WHERE id = '" + Habbo.Id + "' LIMIT 1");

                dbClient.SetQuery("SELECT `ip_last` FROM `users` WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
                IPAddress = dbClient.getString();
            }

            string Reason = null;
            if (Params.Length >= 3)
                Reason = CommandManager.MergeParams(Params, 2);
            else
                Reason = "Nenhuma razão especificada.";

            if (!string.IsNullOrEmpty(IPAddress))
                GalaxyServer.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.IP, IPAddress, Reason, Expire);
            GalaxyServer.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Reason, Expire);

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient != null)
                TargetClient.Disconnect();


            Session.SendWhisper("Sucesso, baniu o ip e a conta do usuário '" + Username + "' pelo motivo '" + Reason + "'!");
        }
    }
}