using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Users;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class UnBanCommand : IChatCommand
    {

        public string PermissionRequired => "command_unban";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Desbanir usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome de usuário do usuário.");
                return;
            }

            Habbo Habbo = GalaxyServer.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário no banco de dados.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_soft_ban") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Ops, você não pode desbanir esse usuário.");
                return;
            }

            string Username = Habbo.Username;
            string IPAddress = "";
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `ip_last` FROM `users` WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
                IPAddress = dbClient.getString();

                dbClient.runFastQuery("DELETE FROM `bans` WHERE `value` = '" + Habbo.Username + "' or `value` =  '" + IPAddress + "' LIMIT 1");
            }

            Session.SendWhisper("Sucesso, você desbaniu o usuário(a) '" + Username + "'!");
        }
    }
}