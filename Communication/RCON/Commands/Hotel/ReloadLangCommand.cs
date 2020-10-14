namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadLangCommand : IRCONCommand
    {
        public string Description => "Recarrega as langs.";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetLanguageManager().Init();
            return true;
        }
    }
}