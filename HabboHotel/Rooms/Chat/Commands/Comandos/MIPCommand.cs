using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Moderation;
using Galaxy.HabboHotel.Users;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MIPCommand : IChatCommand
    {
        public string PermissionRequired => "command_mip";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Bane a máquina, ip e conta do usuário.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome de usuário do usuário que deseja banir.");
                return;
            }

            Habbo Habbo = GalaxyServer.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário no banco de dados.");
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
            if (Username == "PHB")
            {
                Session.SendWhisper("Você não pode banir o todo poderoso...");
                return;
            }
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");

                dbClient.SetQuery("SELECT `ip_last` FROM `users` WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
                IPAddress = dbClient.getString();
            }

            string Reason = null;
            if (Params.Length >= 3)
                Reason = CommandManager.MergeParams(Params, 2);
            else
                Reason = "Não há motivo específico.";

          //  if (!string.IsNullOrEmpty(IPAddress))
           //     GalaxyServer.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.IP, IPAddress, Reason, Expire);

			GalaxyServer.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Reason, Expire);

            if (!string.IsNullOrEmpty(Habbo.MachineId))
                GalaxyServer.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.MACHINE, Habbo.MachineId, Reason, Expire);

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient != null)
                TargetClient.Disconnect();
            Session.SendWhisper("Sucesso, você baniu o usuário '" + Username + "' pelo seguinte motivo: '" + Reason + "'!");
        }
    }
}