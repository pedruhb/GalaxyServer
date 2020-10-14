namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadBadgesPHB : IRCONCommand
    {
        public string Description => "Atualizar emblemas";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
                 GalaxyServer.GetGame().GetBadgeManager().Init();
                return true;
        }
    }
}