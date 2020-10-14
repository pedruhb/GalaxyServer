using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.GameClients;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class CatalogUpdateAlert : IChatCommand
    {
        public string PermissionRequired => "command_cataltd";
        public string Parameters => "[MENSAGEM]";
        public string Description => "Avisar de uma atualização no catálogo do hotel.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);

            GalaxyServer.GetGame().GetCatalog().Init(GalaxyServer.GetGame().GetItemManager());
            GalaxyServer.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
            GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Atualizamos o Catálago!",
              "O catálogo do <font color=\"#2E9AFE\"><b>" + GalaxyServer.HotelName + "</b></font> acaba de receber um novo  <b>Raro LTD!</b>", "cata", "Abrir catálogo", "event:catalog/open/" + Message));

            Session.SendWhisper("Catalogo atualizado com sucesso.");
        }
    }
}

