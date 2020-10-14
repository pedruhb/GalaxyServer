namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadItemsCommand : IRCONCommand
    {
        public string Description => "Atualizar Items";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetItemManager().Init();

            return true;
        }
    }
}