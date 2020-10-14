namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadNavigatorCommand : IRCONCommand
    {
        public string Description => "Atualizar navegador";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetNavigator().Init();

            return true;
        }
    }
}