using Galaxy.Core;
using Galaxy.Database.Interfaces;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class FilterCommand : IChatCommand
    {

        public string PermissionRequired => "command_filter";
        public string Parameters => "[PALAVRA]";
        public string Description => "Adicione palavras ao filtro.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite uma palavra.");
                return;
            }

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("INSERT INTO `wordfilter` (id, word, replacement, strict, addedby, bannable) VALUES (NULL, '" + Params[1] + "', '" + GalaxyServer.HotelName + "', '1', '" + Session.GetHabbo().Username + "', '0')");
            }

            GalaxyServer.GetGame().GetChatManager().GetFilter().InitWords();
            GalaxyServer.GetGame().GetChatManager().GetFilter().InitCharacters();
            Session.SendWhisper("Sucesso, continue lutando contra spammers");
        }
    }
}