using System;
namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadFilterCommand : IRCONCommand
    {
        public string Description => "Atualizar filtro de palavras.";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetChatManager().GetFilter().InitWords();
            GalaxyServer.GetGame().GetChatManager().GetFilter().InitCharacters();
         //   Console.WriteLine(parameters[0]);
            return true;
        }
    }
}