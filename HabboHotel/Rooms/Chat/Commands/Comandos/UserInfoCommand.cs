using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Users;
using System;
using System.Data;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class UserInfoCommand : IChatCommand
    {
        public string PermissionRequired => "command_user_info";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Visualizar informações sobre um usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome do usuário que deseja visualizar.");
                return;
            }

            DataRow UserData = null;
            DataRow UserInfo = null;
            string Username = Params[1];

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `id`,`username`,`mail`,`rank`,`motto`,`credits`,`activity_points`,`vip_points`,`gotw_points`,`online`,`rank_vip` FROM users WHERE `username` = @Username LIMIT 1");
                dbClient.AddParameter("Username", Username);
                UserData = dbClient.getRow();
            }

            if (UserData == null)
            {
                Session.SendNotification("Ops, não há usuário no banco de dados com esse nome de usuário (" + Username + ")!");
                return;
            }

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `user_info` WHERE `user_id` = '" + Convert.ToInt32(UserData["id"]) + "' LIMIT 1");
                UserInfo = dbClient.getRow();
                if (UserInfo == null)
                {
                    dbClient.runFastQuery("INSERT INTO `user_info` (`user_id`) VALUES ('" + Convert.ToInt32(UserData["id"]) + "')");
                    #region Insere HC na conta do usuário
                    Habbo UsuarioPHB = GalaxyServer.GetHabboById(Convert.ToInt32(UserData["id"]));
                    UsuarioPHB.GetClient().GetHabbo().GetClubManager().AddOrExtendSubscription("habbo_vip", 9999 * 24 * 3600, UsuarioPHB.GetClient());
                    UsuarioPHB.GetClient().GetHabbo().GetBadgeComponent().GiveBadge("HC1", true, UsuarioPHB.GetClient());
                    #endregion 
                    dbClient.SetQuery("SELECT * FROM `user_info` WHERE `user_id` = '" + Convert.ToInt32(UserData["id"]) + "' LIMIT 1");
                    UserInfo = dbClient.getRow();
                }
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Username);

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToDouble(UserInfo["trading_locked"]));

            StringBuilder HabboInfo = new StringBuilder();
            HabboInfo.Append("Conta de " + Convert.ToString(UserData["username"]) + ":\r\r");
            HabboInfo.Append("Informações:\r");
            HabboInfo.Append("ID: " + Convert.ToInt32(UserData["id"]) + "\r");
            HabboInfo.Append("Rank: " + Convert.ToInt32(UserData["rank"]) + "\r");
            HabboInfo.Append("Rank VIP: " + Convert.ToInt32(UserData["rank_vip"]) + "\r");
            HabboInfo.Append("Email: " + Convert.ToString(UserData["mail"]) + "\r");
            HabboInfo.Append("Status Online: " + (TargetClient != null ? "Sim" : "Não") + "\r\r");

            HabboInfo.Append("Informações de Moedas:\r");
            HabboInfo.Append(ExtraSettings.NomeMoedas + ": " + Convert.ToInt32(UserData["credits"]) + "\r");
            HabboInfo.Append(ExtraSettings.NomeDuckets+": " + Convert.ToInt32(UserData["activity_points"]) + "\r");
            HabboInfo.Append(ExtraSettings.NomeDiamantes + ": " + Convert.ToInt32(UserData["vip_points"]) + "\r");
            HabboInfo.Append(ExtraSettings.NomeGotw + ": " + Convert.ToInt32(UserData["gotw_points"]) + "\r\r");

            HabboInfo.Append("Informações de Moderação:\r");
            HabboInfo.Append("Banido: " + Convert.ToInt32(UserInfo["bans"]) + "\r");
            HabboInfo.Append("CFHs Sent: " + Convert.ToInt32(UserInfo["cfhs"]) + "\r");
            HabboInfo.Append("Abusive CFHs: " + Convert.ToInt32(UserInfo["cfhs_abusive"]) + "\r");
            HabboInfo.Append("Bloqueio Tradeo: " + (Convert.ToInt32(UserInfo["trading_locked"]) == 0 ? "Sem bloqueio excepcional" : "Expira: " + (origin.ToString("dd/MM/yyyy")) + "") + "\r");
            HabboInfo.Append("Número de fechaduras comerciais: " + Convert.ToInt32(UserInfo["trading_locks_count"]) + "\r\r");

            if (TargetClient != null)
            {
                HabboInfo.Append("Quarto atual:\r");
                if (!TargetClient.GetHabbo().InRoom)
                    HabboInfo.Append("Atualmente não está em uma sala.\r");
                else
                {
                    HabboInfo.Append("Sala: " + TargetClient.GetHabbo().CurrentRoom.Name + " (" + TargetClient.GetHabbo().CurrentRoom.RoomId + ")\r");
                    HabboInfo.Append("Proprietário do quarto: " + TargetClient.GetHabbo().CurrentRoom.OwnerName + "\r");
                    HabboInfo.Append("Visitantes atuais: " + TargetClient.GetHabbo().CurrentRoom.UserCount + "/" + TargetClient.GetHabbo().CurrentRoom.UsersMax);
                }
            }
            Session.SendNotification(HabboInfo.ToString());
        }
    }
}
