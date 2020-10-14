using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MegaOferta : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_update"; }
        }

        public string Parameters
        {
            get { return "%LIGAR% ou %DESLIGAR%"; }
        }

        public string Description
        {
            get { return "Ligar ou desligar uma mega oferta."; ; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendMessage(new RoomNotificationComposer("erro", "message", "Ops, você deve digita assim: ':megaoferta ligar ou :megaoferta desligar'!"));
                return;
            }

            if (Params[1] == "ligar")
            {
                // Comando editaveu abaixo mais cuidado pra não faze merda
                using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE targeted_offers SET active = 'true' WHERE active = 'false'");
                    dbClient.RunQuery("UPDATE users SET targeted_buy = '0'");
                }
                GalaxyServer.GetGame().GetTargetedOffersManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("volada", "message", "Corre, nova mega oferta foi colocada!"));
                Session.SendWhisper("Nova mega oferta iniciada!");
            }

            if (Params[1] == "desligar")
            {
                // Comando editaveu abaixo mais cuidado pra não faze merda
                using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE targeted_offers SET active = 'false' WHERE active = 'true'");
                    dbClient.RunQuery("UPDATE users SET targeted_buy = '0'");
                }
                GalaxyServer.GetGame().GetTargetedOffersManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("ADM", "message", "Que pena, a mega oferta foi retirada!"));
                Session.SendWhisper("Mega oferta retirada!");
            }

            if (Params[1] != "ligar" || Params[1] != "desligar")
            {
                Session.SendMessage(new RoomNotificationComposer("erro", "message", "Ops, você deve digita assim: ':megaoferta ligar ou :megaoferta desligar'!"));
            }
        }
    }
}
