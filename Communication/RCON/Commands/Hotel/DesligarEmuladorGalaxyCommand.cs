namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class DesligarEmuladorGalaxyCommand : IRCONCommand
    {
        public string Description => "Desliga emulador.";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.PerformShutDown();
            return true;
        }
    }
}