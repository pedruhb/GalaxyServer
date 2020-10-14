using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ColourNameCommand : IChatCommand
    {

        public string PermissionRequired => "command_ncolor";
        public string Parameters => "";
        public string Description => "Troca a cor do nick";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendMessage(new RoomNotificationComposer("Lista de cores:",
                     "<font color='#FF8000'><b>LISTA DE CORES:</b>\n" +
                     "<font size=\"12\" color=\"#1C1C1C\">Esse comando permite que você mude a cor de seu nick, você também pode mudar pela cms nas configurações de sua conta!\r\r" +
                     "<font size =\"11\" color=\"#00e676\"><b>:ncolor verdec</b> » Nome Verde Claro</font>\r\n" +
                     "<font size =\"11\" color=\"#00bcd4\"><b>:ncolor ciano</b> » Nome Ciano</font>\r\n" +
                     "<font size =\"11\" color=\"#0000FF\"><b>:ncolor azul</b> » Nome Azu</font>\r\n" +
                     "<font size =\"11\" color=\"#e91e63\"><b>:ncolor rosa</b> » Nome Rosa</font>\r\n" +
                     "<font size =\"11\" color=\"#f50101\"><b>:ncolor vermelho</b> » Nome Vermelho</font>\r\n" +
                     "<font size =\"11\" color=\"#ff0000\"><b>:ncolor laranja</b> » Nome Laranja</font>\r\n" +
                     "<font size =\"11\" color=\"#31B404\"><b>:ncolor verde</b> » Nome Verde</font>\r\n" +
                     "<font size =\"11\" color=\"#ff9100\"><b>:ncolor laranjac</b> » Nome Laranja Claro</font>\r\n" +
                     "<font size =\"11\" color=\"\"><b>:ncolor preto</b> » Nome Preto</font>\r\n" +
                     "<font size =\"11\" color=\"" + GalaxyServer.Rainbow() + "\"><b>:ncolor rainbow</b> » Nome Arco-íris</font>\r\n" +
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
                    Session.GetHabbo().chatHTMLColour = "";
                    Session.SendWhisper("A cor do seu nome foi desativada!");
                    break;
                case "rainbow":
                    Session.GetHabbo().chatHTMLColour = "rainbow";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = 'rainbow' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "cgreen":
                case "verdec":
                    Session.GetHabbo().chatHTMLColour = "#00e676";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#00e676' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "tblue":
                case "azul":
                    Session.GetHabbo().chatHTMLColour = "#0000FF";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#0000FF' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "laranjac":
                    Session.GetHabbo().chatHTMLColour = "#ff9100";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#ff9100' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "pink":
                case "rosa":
                    Session.GetHabbo().chatHTMLColour = "#e91e63";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#e91e63' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "blue":
                    Session.GetHabbo().chatHTMLColour = "#0000FF";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#0000FF' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "red":
                case "vermelho":
                    Session.GetHabbo().chatHTMLColour = "#f50101";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#f50101' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "green":
                case "verde":
                    Session.GetHabbo().chatHTMLColour = "#31B404";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#31B404' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "cyan":
                case "ciano":
                    Session.GetHabbo().chatHTMLColour = "#00bcd4";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#00bcd4' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    }
                    Session.SendWhisper("A cor escolhida foi ativada!");
                    break;
                case "orange":
                case "laranja":
                    Session.GetHabbo().chatHTMLColour = "#ff9100";
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `users` SET `name_color` = '#ff9100' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
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