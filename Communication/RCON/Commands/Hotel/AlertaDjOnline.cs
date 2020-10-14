﻿
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms;
using System.Data;

namespace Galaxy.Communication.RCON.Commands.User
{
    class AlertaDjOnline : IRCONCommand
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
            GalaxyServer.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("DJAlertNEW", "O DJ " + mee["username"] + " está ao vivo na rádio " + GalaxyServer.HotelName + "!", ""));
            return true;
        }
    }
}