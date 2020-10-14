using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using System;
using System.Data;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class LoteriaCommandPHB : IChatCommand
    {
        public string PermissionRequired => "command_loteria";
        public string Parameters => "";
        public string Description => "Premia a loteria";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                StringBuilder List = new StringBuilder("");
                List.AppendLine("Intruções:");
                List.AppendLine("Você deve usar o comando :loteria premiar para gerar um ganhador, todo hotel será alertado com o usuário que ganhar, esse comando não tem volta, após fazer isso, será resetado todos os tickets de loteria e disponibilizados para venda novamente.");
                List.AppendLine("-------------------------");
                List.AppendLine(":loteria premiar - Finaliza a loteria e alerta resultado.");
                List.AppendLine(":loteria quantidade - Mostra a quantidade de participantes.");
                List.AppendLine(":loteria lista - Lista todos os usuários participantes.");
                Session.SendNotification(List.ToString());
                return;
            }

            if(Params[1].ToLower() == "premiar")
            {
                DataRow Premiar = null;
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT id,username,look,loteria FROM users WHERE loteria > '0' and online = '1' ORDER BY rand() LIMIT 1");
                    Premiar = dbClient.getRow();

                    if (Premiar["look"] == null)
                    {
                        Session.SendWhisper("Nenhum usuário comprou bilhete.");
                    return;
                    }

                    GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + Premiar["look"], 3, Premiar["username"] + " acaba de ganhar na loteria, parabéns!",""));
                    GalaxyServer.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer("O usuário "+ Premiar["username"] + " acaba de ganhar na loteria do " + GalaxyServer.HotelName + ", parabéns!"));
                    dbClient.runFastQuery("UPDATE users SET loteria = '0';");
                }
            }
            else if (Params[1].ToLower() == "quantidade")
            {
                DataRow Quantidade = null;
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT count(id) as total FROM users WHERE loteria > '0'");
                    Quantidade = dbClient.getRow();
                    Session.SendWhisper("Tem um total de " + Quantidade["total"] + " usuários participando da loteria.");
                }
            }
            else if (Params[1].ToLower() == "lista")
            {
                IQueryAdapter adapter;
                DataTable table = null;
                StringBuilder builder = new StringBuilder();

                builder.AppendLine("Listagem de todos os participantes da loteria ");
                using (adapter = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    adapter.SetQuery("Select id,username,loteria from users where loteria > '0' order by loteria");
                    table = adapter.getTable();
                    foreach (DataRow row in table.Rows)
                    {
                        builder.AppendLine(string.Concat(new object[] { "ID : ", row["id"], " - Nick: ", row["username"], " - Número: ", row["loteria"], }));
                    }
                }

                Session.SendMessage(new MOTDNotificationComposer(builder.ToString()));
            } else
            {
                Session.SendWhisper("Variável inexistente.");
            }

        }
    }
}