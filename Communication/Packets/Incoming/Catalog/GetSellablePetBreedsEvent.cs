using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Items;

namespace Galaxy.Communication.Packets.Incoming.Catalog
{
    public class GetSellablePetBreedsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            ItemData Item = GalaxyServer.GetGame().GetItemManager().GetItemByName(Type);
            if (Item == null)
                return;

            int PetId = Item.BehaviourData;

            Session.SendMessage(new SellablePetBreedsComposer(Type, PetId, GalaxyServer.GetGame().GetCatalog().GetPetRaceManager().GetRacesForRaceId(PetId)));
        }
    }
}