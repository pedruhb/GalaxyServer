using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms;

namespace Galaxy.HabboHotel.Items.Interactor
{
    public class InteractorInformationTerminal : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {

        }
        public void BeforeRoomLoad(Item Item)
        { }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (Item == null || Item.GetRoom() == null || Session == null || Session.GetHabbo() == null)
                return;

            RoomUser User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.LastInteraction = GalaxyServer.GetUnixTimestamp();
            //Session.SendWhisper("Galaxy Emulador");
            //Session.SendWhisper("Teste:" + Item.ExtraData);
        }

        public void OnWiredTrigger(Item Item)
        {
        }
    }
}