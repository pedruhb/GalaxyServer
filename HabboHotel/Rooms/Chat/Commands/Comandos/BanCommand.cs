using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Moderation;
using Galaxy.HabboHotel.Users;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class BanCommand : IChatCommand
    {

        public string PermissionRequired => "command_ban";
        public string Parameters => "[USUÁRIO] [TEMPO] [RAZÃO]";
        public string Description => "Banir um usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome do usuário que deseja banir.");
                return;
            }
            if (Params.Length == 2)
            {
                Session.SendWhisper("Digite o tempo que o usuário ficará banido.");
                return;
            }

            Habbo Habbo = GalaxyServer.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário no banco de dados.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_soft_ban") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any") || Session.GetHabbo().Id == Habbo.Id || Habbo.Rank > 14 || Habbo.Username == "PHB" || Session.GetHabbo().Id == 1)
            {
                Session.SendWhisper("Opa, você não pode banir o usuário.");
                return;
            }

            Double Expire = 0;

            if (String.IsNullOrEmpty(Params[2]) || Params[2] == null)
            {
               
            }

                string Hours = Params[2];
            if (String.IsNullOrEmpty(Hours) || Hours == "perm")
                Expire = GalaxyServer.GetUnixTimestamp() + 78892200;
            else
                Expire = (GalaxyServer.GetUnixTimestamp() + (Convert.ToDouble(Hours) * 3600));

            string Reason = null;
            if (Params.Length >= 4)
                Reason = CommandManager.MergeParams(Params, 3);
            else
                Reason = "Nenhuma razão especificada.";

            string Username = Habbo.Username;


            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
            
                dbClient.runFastQuery("UPDATE users SET rank = '1' WHERE id = '"+ Habbo.Id + "' LIMIT 1");

            }

            GalaxyServer.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Reason, Expire);

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient != null)
                TargetClient.Disconnect();

            Session.SendWhisper("Sucesso, você baniu a conta '" + Username + "' por " + Hours + " hora(s) com razão: '" + Reason + "'!");
        }
    }
}