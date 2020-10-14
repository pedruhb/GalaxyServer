using Galaxy.Communication.Packets.Outgoing.Catalog;

namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadCatalogCommand : IRCONCommand
    {
        public string Description => "Atualizar o catálogo";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetCatalog().Init(GalaxyServer.GetGame().GetItemManager());
            GalaxyServer.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
            return true;
        }
    }
}