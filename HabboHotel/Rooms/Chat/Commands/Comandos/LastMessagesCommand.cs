using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Data;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class LastMessagesCommand : IChatCommand
    {
        public string PermissionRequired => "command_lastmsg";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Verifique as últimas mensagens de usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (ExtraSettings.STAFF_EFFECT_ENABLED_ROOM)
            {
                if (Session.GetHabbo().isLoggedIn && Session.GetHabbo().Rank > Convert.ToInt32(GalaxyServer.GetConfig().data["MineRankStaff"]))
                {
                }
                else
                {
                    Session.SendWhisper("Você precisa estar logado como staff para usar este comando.");
                    return;
                }
            }
            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome do usuário que deseja ver, revise suas informações.");
                return;
            }

            DataRow UserData = null;
            string Username = Params[1];

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `username` FROM users WHERE `username` = @Username LIMIT 1");
                dbClient.AddParameter("Username", Username);
                UserData = dbClient.getRow();
            }

            if (UserData == null)
            {
                Session.SendNotification("Não há nenhum usuário com o nome " + Username + ".");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Username);

            DataTable GetLogs = null;
            StringBuilder HabboInfo = new StringBuilder();

            HabboInfo.Append("Estas são as últimas mensagens do usuário suspeito, lembre-se sempre de verificar esses casos antes de prosseguir o banimento, a menos que seja um caso óbvio de spam.\n\n");

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `message` FROM `chatlogs` WHERE `user_id` = '" + TargetClient.GetHabbo().Id + "' ORDER BY `id` DESC LIMIT 10");
                GetLogs = dbClient.getTable();

                if (GetLogs != null)
                {
                    int Number = 11;
                    foreach (DataRow Log in GetLogs.Rows)
                    {
                        Number -= 1;
                        HabboInfo.Append("<font size ='8' color='#B40404'><b>[" + Number + "]</b></font>" + " " + Convert.ToString(Log["message"]) + "\r");
                     //   Session.SendMessage(new RoomNotificationComposer("Usuário: " + Username + " - " + Number + ":", Convert.ToString(Log["message"]) + "", "", ""));
                    }
                }
                Session.SendMessage(new RoomNotificationComposer("Últimas mensagens de " + Username + ":", (HabboInfo.ToString()), "fig/" + TargetClient.GetHabbo().Look + "", "", ""));
            }
        }
    }
}
    
