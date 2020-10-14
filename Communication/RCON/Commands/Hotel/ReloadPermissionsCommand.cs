namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadPermissionsCommand : IRCONCommand
    {
        public string Description => "Atualizar as permissões";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetPermissionManager().Init();
            return true;
        }
    }
}