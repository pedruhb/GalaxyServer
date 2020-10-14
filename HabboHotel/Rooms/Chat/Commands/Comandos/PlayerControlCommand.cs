using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class PlayerControlCommand : IChatCommand
    {
        public string PermissionRequired => "";
        public string Parameters => "[VARIÁVEL]";
        public string Description => "Configura o player da rádio do hotel.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Params.Length == 1)
            {
                StringBuilder List = new StringBuilder();
                List.Append("Por favor, escolha a função desejada.\n\n");
                List.Append(":player desativar - O player será desativado/ativado de sua client.\n");
                List.Append(":player autoplay - O player tocará/não tocará automaticamente ao abrir a client.\n");
                List.Append(":player padrao - O player será redefinido as configurações padrões.\n\n");
                List.Append("É necessário reentrar no hotel após as devidas alterações.");
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

         

            switch (Params[1])
            {
                case "desativar":
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        System.Data.DataRow UserData = null;
                        dbClient.SetQuery("SELECT player_on FROM users WHERE id = " + Session.GetHabbo().Id + " LIMIT 1;");
                        UserData = dbClient.getRow();
                        bool playeron = System.Convert.ToBoolean(UserData["player_on"]);

                        if (playeron == true)
                        {
                            dbClient.SetQuery("UPDATE users SET player_on = @player WHERE id = @id  LIMIT 1;");
                            dbClient.AddParameter("id", Session.GetHabbo().Id);
                            dbClient.AddParameter("player", "false");
                            dbClient.RunQuery();

                            Session.SendWhisper("O player não irá aparecer.");
                        }
                        else
                        {
                            dbClient.SetQuery("UPDATE users SET player_on = @player WHERE id = @id  LIMIT 1;");
                            dbClient.AddParameter("id", Session.GetHabbo().Id);
                            dbClient.AddParameter("player", "true");
                            dbClient.RunQuery();

                            Session.SendWhisper("O player irá aparecer.");
                        }

                    }
                    break;
                case "autoplay":
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        System.Data.DataRow UserData = null;
                        dbClient.SetQuery("SELECT autoplay FROM users WHERE id = " + Session.GetHabbo().Id + " LIMIT 1;");
                        UserData = dbClient.getRow();
                        bool playeron = System.Convert.ToBoolean(UserData["autoplay"]);

                        if (playeron == true)
                        {
                            dbClient.SetQuery("UPDATE users SET autoplay = @player WHERE id = @id  LIMIT 1;");
                            dbClient.AddParameter("id", Session.GetHabbo().Id);
                            dbClient.AddParameter("player", "false");
                            dbClient.RunQuery();

                            Session.SendWhisper("O player não irá iniciar automaticamente.");
                        }
                        else
                        {
                            dbClient.SetQuery("UPDATE users SET autoplay = @player WHERE id = @id  LIMIT 1;");
                            dbClient.AddParameter("id", Session.GetHabbo().Id);
                            dbClient.AddParameter("player", "true");
                            dbClient.RunQuery();

                            Session.SendWhisper("O player irá iniciar automaticamente.");
                        }

                    }
                    break;
                case "padrao":
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {

                            dbClient.SetQuery("UPDATE users SET player_on = true, autoplay = true WHERE id = @id  LIMIT 1;");
                            dbClient.AddParameter("id", Session.GetHabbo().Id);
                            dbClient.RunQuery();

                            Session.SendWhisper("Padrões redefinidos.");

                    }
                    break;
                default:
                    Session.SendWhisper("Argumento inválido, tente novamente.");
                    break;
            }
        }
    }
}