using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.GameClients;

using Galaxy.Database.Interfaces;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class LastConsoleMessagesCommand : IChatCommand
    {
        public string PermissionRequired => "command_last_console_messages";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Verifique as últimas mensagens do usuário no console.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
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
                Session.SendMessage(new RoomAlertComposer("Não há nenhum usuário com o nome " + Username + "."));
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Username);

            DataTable GetLogs = null;
            StringBuilder HabboInfo = new StringBuilder();

            HabboInfo.Append("Estas são as últimas mensagens do usuário suspeito, lembre-se sempre de verificar esses casos antes de prosseguir a proibição, a menos que seja um caso óbvio de spam.\n\n");

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `message` FROM `chatlogs_console` WHERE `from_id` = '" + TargetClient.GetHabbo().Id + "' ORDER BY `id` DESC LIMIT 10");
                GetLogs = dbClient.getTable();

                if (GetLogs == null)
                {
                    Session.SendMessage(new RoomAlertComposer("Infelizmente, o usuário que você solicitou não possui mensagens no registro."));
                }

                else if (GetLogs != null)
                {
                    int Number = 11;
                    foreach (DataRow Log in GetLogs.Rows)
                    {
                        Number -= 1;
                        HabboInfo.Append("<font size ='8' color='#B40404'><b>[" + Number + "]</b></font>" + " " + Convert.ToString(Log["message"]) + "\r");
                        //Session.SendMessage(new RoomNotificationComposer("usuário: " + Username + " - " + Number + ":", Convert.ToString(Log["message"]) + "", "", ""));
                    }
                }

                Session.SendMessage(new RoomNotificationComposer("Últimas mensagens de " + Username + ":", (HabboInfo.ToString()), "fig/" + TargetClient.GetHabbo().Look + "", "", ""));

            }
        }
    }
}
