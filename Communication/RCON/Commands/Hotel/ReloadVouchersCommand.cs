namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadVouchersCommand : IRCONCommand
    {
        public string Description => "Atualizar Vouchers";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {

            GalaxyServer.GetGame().GetCatalog().GetVoucherManager().Init();
            return true;
        }
    }
}