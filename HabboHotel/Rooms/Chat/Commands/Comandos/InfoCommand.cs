
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Data;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class InfoCommand : IChatCommand
    {
        public string PermissionRequired => "command_info";
        public string Parameters => "";
        public string Description => "Mostra as informações do "+ GalaxyServer.VersionGalaxy + ".";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            TimeSpan Uptime = DateTime.Now - GalaxyServer.ServerStarted;
            int OnlineUsers = GalaxyServer.GetGame().GetClientManager().Count;
            int RoomCount = GalaxyServer.GetGame().GetRoomManager().Count;
            DataRow DadosPHB = null;
            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT record_on FROM server_status");
                DadosPHB = dbClient.getRow();

            }            
            Session.SendMessage(new RoomNotificationComposer(GalaxyServer.VersionGalaxy,
                 "<font color=\"#0653b4\"><b>Infomações do Servidor:</b></font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">Esse emulador privado contém todas as características necessárias para um hotel estável com o que há de melhor.</font>\n\n" +
                 "<font color=\"#0653b4\" size=\"13\"><b>Informações:</b></font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Usuários: </b> " + OnlineUsers + "</font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Recorde de Usuários: </b> " + DadosPHB["record_on"] + " </font>\n" +
                  "<font size=\"11\" color=\"#1C1C1C\">  <b> · Recorde desde a última inicialização: </b> " + GalaxyServer.recordreinicio + " </font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Quartos: </b> " + RoomCount + "</font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Uptime: </b> " + Uptime.Days + " dia(s), " + Uptime.Hours + " horas e " + Uptime.Minutes + " minutos.</font>\n" +
				 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Última atualização: </b> " + GalaxyServer.LastUpdate + "</font>\n\n" +
				 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Build: </b> " + GalaxyServer.Build + "</font>\n\n" +
				 "<font size=\"11\" color=\"#1C1C1C\">  <b>           " + GalaxyServer.SWFRevision + " </b></font>\n"
				 , "", ""));
        }
    }
}
