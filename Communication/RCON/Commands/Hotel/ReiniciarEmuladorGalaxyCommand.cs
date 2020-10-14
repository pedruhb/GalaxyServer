namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReiniciarEmuladorGalaxyCommand : IRCONCommand
    {
        public string Description => "Reinicia emulador.";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.PerformShutDown(true);
            return true;
        }
    }
}