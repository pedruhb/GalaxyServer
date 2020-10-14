using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ColourPrefixCommand : IChatCommand
    {

        public string PermissionRequired => "command_pcolor"; 
        public string Parameters => ""; 
        public string Description => "Troca a cor da sua TAG"; 

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendMessage(new RoomNotificationComposer("Lista de cores:",
                     "<font color='#FF8000'><b>LISTA DE CORES:</b>\n" +
                     "<font size=\"12\" color=\"#1C1C1C\">Esse comando permite que você mude a cor de sua tag, você também pode mudar pela cms nas configurações de sua conta!\r\r" +
                     "<font size =\"11\" color=\"#00e676\"><b>:pcolor verdec</b> » Prefixo Verde Claro</font>\r\n" +
                     "<font size =\"11\" color=\"#00bcd4\"><b>:pcolor ciano</b> » Prefixo Ciano</font>\r\n" +
                     "<font size =\"11\" color=\"#0000FF\"><b>:pcolor azul</b> » Prefixo Azu</font>\r\n" +
                     "<font size =\"11\" color=\"#e91e63\"><b>:pcolor rosa</b> » Prefixo Rosa</font>\r\n" +
                     "<font size =\"11\" color=\"#f50101\"><b>:pcolor vermelho</b> » Prefixo Vermelho</font>\r\n" +
                     "<font size =\"11\" color=\"#ff0000\"><b>:pcolor laranja</b> » Prefixo Laranja</font>\r\n" +
                     "<font size =\"11\" color=\"#31B404\"><b>:pcolor verde</b> » Prefixo Verde</font>\r\n" +
                     "<font size =\"11\" color=\"#ff9100\"><b>:pcolor laranjac</b> » Prefixo Laranja Claro</font>\r\n" +
                     "<font size =\"11\" color=\"\"><b>:pcolor preto</b> » Prefixo Preto</font>\r\n" +
                     "<font size =\"11\" color=\"" + GalaxyServer.Rainbow() + "\"><b>:pcolor rainbow</b> » Prefixo Arco-íris</font>\r\n" +
                     "", "", ""));
                return;
            }
            string chatColour = Params[1];
            string Colour = chatColour.ToUpper();
            switch (chatColour)
            {
                case "none":
                case "black":
                case "off":
                case "preto":
                    Session.GetHabbo()._NamePrefixColor = "";
                    Session.SendWhisper("A cor da sua tag foi desativada!");
                    break;
                case "rainbow":
                    Session.GetHabbo()._NamePrefixColor = "rainbow";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = 'rainbow' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "cgreen":
                case "verdec":
                    Session.GetHabbo()._NamePrefixColor = "#00e676";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = '#00e676' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "tblue":
                case "azul":
                    Session.GetHabbo()._NamePrefixColor = "#0000FF";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = '#0000FF' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "laranjac":
                    Session.GetHabbo()._NamePrefixColor = "#ff9100";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = '#ff9100' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "pink":
                case "rosa":
                    Session.GetHabbo()._NamePrefixColor = "#e91e63";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = '#e91e63' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "blue":
                    Session.GetHabbo()._NamePrefixColor = "#0000FF";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = '#0000FF' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "red":
                case "vermelho":
                    Session.GetHabbo()._NamePrefixColor = "#f50101";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = '#f50101' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "green":
                case "verde":
                    Session.GetHabbo()._NamePrefixColor = "#31B404";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = '#31B404' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "cyan":
                case "ciano":
                    Session.GetHabbo()._NamePrefixColor = "#00bcd4";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = '#00bcd4' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "orange":
                case "laranja":
                    Session.GetHabbo()._NamePrefixColor = "#ff9100";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `prefix_name_color` = '#ff9100' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                default:
                    Session.SendWhisper("A cor " + Colour + " não existe");
                    break;
            }
            return;
            }
        }
    }