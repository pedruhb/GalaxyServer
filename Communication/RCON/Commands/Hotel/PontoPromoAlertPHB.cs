
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms;
using System.Data;

namespace Galaxy.Communication.RCON.Commands.User
{
    class PontoPromoAlertPHB : IRCONCommand
    {
        public string Description
        {
            get { return "Envia um alerta quando o usuario ganha ponto no hall."; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public bool TryExecute(string[] parameters)
        {

            DataRow mee = null;
            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT look,username from users where username = '"+parameters[0]+"'");
                mee = dbClient.getRow();
            }
            GalaxyServer.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("fig/" + mee["look"], parameters[0] + " ganhou uma promoção no " + GalaxyServer.HotelName + "."));

            return true;
        }
    }
}