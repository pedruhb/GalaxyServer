using Galaxy.Core;

namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadServerSettingsCommand : IRCONCommand
    {
        public string Description => "Atualizar configurações";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetSettingsManager().Init();
            return true;
        }
    }
}