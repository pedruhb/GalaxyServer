using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Users;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class TradeBanCommand : IChatCommand
    {
        public string PermissionRequired => "command_tradeban";
        public string Parameters => "[USUARIO] [TEMPO]";
        public string Description => "Proiba as trocas de um usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, introduza um nome de usuário e um tempo válido (1-365).");
                return;
            }

            Habbo Habbo = GalaxyServer.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Usuário não encontrado no banco de dados.");
                return;
            }

            if (Convert.ToDouble(Params[2]) == 0)
            {
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE `user_info` SET `trading_locked` = '0' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
                }

                if (Habbo.GetClient() != null)
                {
                    Habbo.TradingLockExpiry = 0;
                    Habbo.GetClient().SendNotification("Você agora pode fazer trocas!");
                }

                Session.SendWhisper("Você desativou as trocas do usuário " + Habbo.Username + ".");
                return;
            }

            double Days;
            if (double.TryParse(Params[2], out Days))
            {
                if (Days < 1)
                    Days = 1;

                if (Days > 365)
                    Days = 365;

                double Length = (GalaxyServer.GetUnixTimestamp() + (Days * 86400));
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `user_info` SET `trading_locked` = '" + Length + "', `trading_locks_count` = `trading_locks_count` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
                }

                if (Habbo.GetClient() != null)
                {
                    Habbo.TradingLockExpiry = Length;
                    Habbo.GetClient().SendNotification("Suas trocas foram desativadas por " + Days + " dia(s)!");
                }

                Session.SendWhisper("Você baniu as trocas do usuário " + Habbo.Username + " por " + Days + " dia(s).");
            }
            else
                Session.SendWhisper("Por favor, introduza um número válido.");
        }
    }
}
