﻿using Galaxy.HabboHotel.Rooms;

namespace Galaxy.HabboHotel.Items.Interactor
{
	class InteractorArca : IFurniInteractor
	{
		public void OnPlace(GameClients.GameClient Session, Item Item)
		{
			Item.ExtraData = "0";
			Item.UpdateNeeded = true;
		}

		public void OnRemove(GameClients.GameClient Session, Item Item)
		{
		}

		public void OnTrigger(GameClients.GameClient Session, Item Item, int Request, bool HasRights)
		{

			if (Session == null || Session.GetHabbo() == null || Item == null)
				return;

			Room Room = Session.GetHabbo().CurrentRoom;
			if (Room == null)
				return;

			RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (Actor == null)
				return;

			if (Item.ExtraData == "1")
				return;

			if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) > 2)
				return;

			GalaxyServer.GetGame().GetPinataManager().ReceiveCrackableReward(Actor, Room, Item);

		}

		public void OnWiredTrigger(Item Item)
		{
			Item.ExtraData = "-1";
			Item.UpdateState(false, true);
			Item.RequestUpdate(4, true);
		}
	}
}
