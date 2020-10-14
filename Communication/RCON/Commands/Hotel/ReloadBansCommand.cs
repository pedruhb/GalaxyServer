namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadBansCommand : IRCONCommand
    {
        public string Description => "Atualizar banimentos";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetModerationManager().ReCacheBans();

            return true;
        }
    }
}