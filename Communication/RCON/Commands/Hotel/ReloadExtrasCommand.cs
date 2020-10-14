namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadExtrasCommand : IRCONCommand
    {
        public string Description => "Atualizar banimentos";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            Core.ExtraSettings.RunExtraSettings();
           // System.Console.WriteLine("Comando Executado");
            return true;
        }
    }
}