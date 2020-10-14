namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadQuestsCommand : IRCONCommand
    {
        public string Description => "Atualizar questões";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetQuestManager().Init();

            return true;
        }
    }
}