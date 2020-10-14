using Galaxy.HabboHotel.Rooms;

namespace Galaxy.HabboHotel.Items.Interactor
{
    class InteractorTelevisao : IFurniInteractor
    {
        public void OnPlace(GameClients.GameClient Session, Item Item)
        {
            Item.ExtraData = "1";
        }

        public void OnRemove(GameClients.GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClients.GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (Session == null || Session.GetHabbo() == null || Item == null)
                return;

            dynamic product = new Newtonsoft.Json.Linq.JObject();
            product.tipo = "tv";
            product.dono = Item.UserID;

            GalaxyServer.SendUserJson(Session, product);

        }

        public void OnWiredTrigger(Item Item)
        {

        }
    }
}
