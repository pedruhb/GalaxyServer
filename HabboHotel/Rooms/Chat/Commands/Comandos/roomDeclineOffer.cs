namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class roomDeclineOfferr : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Negue a oferta de preço para compra de seu quarto."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room CurrentRoom, string[] Params)
        {

            RoomUser RoomOwner = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (RoomOwner.RoomOfferPending)
            {
                if (RoomOwner.GetClient().GetHabbo().CurrentRoom.RoomData.roomForSale)
                {
                    if (RoomOwner.GetClient().GetHabbo().CurrentRoom.RoomData.OwnerId == RoomOwner.GetClient().GetHabbo().Id)
                    {
                        RoomUser OfferingUser = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(RoomOwner.RoomOfferUser);
                        OfferingUser.GetClient().SendWhisper("Este usuário negou sua oferta");
                        RoomOwner.RoomOfferPending = false;
                        RoomOwner.RoomOfferUser = 0;
                        RoomOwner.RoomOffer = "";
                    }
                }
            }
        }
    }
}