using Galaxy.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users.Effects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Galaxy.HabboHotel.Items.Interactor
{
	public class InteractorPlanetaTotem : IFurniInteractor
	{
		public void OnPlace(GameClient Session, Item Item)
		{
			Item.ExtraData = "0";
			Item.UpdateNeeded = true;
		}

		public void OnRemove(GameClient Session, Item Item)
		{
		}

		public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
		{
			int PlanetX = Item.GetX;
			int PlanetY = Item.GetY;
			int CorpoTotem = 0;
			int CabecaTotem = 0;

			List<Item> Items = Session.GetHabbo().CurrentRoom.GetGameMap().GetAllRoomItemForSquare(PlanetX, PlanetY);

			if (Items != null && Items.Count() > 0)
			{
				foreach (Item uItem in Items.ToList())
				{
					if (uItem == null)
						continue;

					if (uItem.UserID == Session.GetHabbo().Id && uItem.GetBaseItem().InteractionType.ToString().ToUpper() == "CORPOTOTEM")
						CorpoTotem = 1;

					if (uItem.UserID == Session.GetHabbo().Id && uItem.GetBaseItem().InteractionType.ToString().ToUpper() == "CABECATOTEM")
						CabecaTotem = 1;

				}
			}

			if (CorpoTotem == 0 && CabecaTotem == 0)
			{
				Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "O planeta totem só pode ser ativado caso esteja em cima de um totem."));
				return;
			}

			if (Item.UserID != Session.GetHabbo().Id)
			{
				Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Este totem não te pertence."));
				return;
			}

	
			if (Item.ExtraData == "0" || Item.ExtraData == "")
			{
				Item.UpdateState(true, true);
				Item.RequestUpdate(0, true);
				AvatarEffect Effect = null;
				if (Session.GetHabbo().Effects().HasEffect(24))
				{
					Effect = Session.GetHabbo().Effects().GetEffectNullable(24);

					if (Effect != null)
					{
						Effect.AddToQuantity();
					}
				}
				else
					Effect = AvatarEffectFactory.CreateNullable(Session.GetHabbo(), 24, 3600);

				if (Effect != null)
				{
					Session.SendMessage(new AvatarEffectAddedComposer(24, 3600));
					Session.GetHabbo().Effects().ApplyEffect(24);
					Session.GetHabbo().Effects().TryAdd(Effect);
					Session.SendWhisper("Efeito chuva adicionado!");
					Session.SendMessage(new AvatarEffectsComposer(Session.GetHabbo().Effects().GetAllEffects));

				}
				Item.ExtraData = "1";
				return;
			}
			else if (Item.ExtraData == "1")
			{
				Item.UpdateState(true, true);
				Item.RequestUpdate(0, true);
				AvatarEffect Effect = null;
				if (Session.GetHabbo().Effects().HasEffect(25))
				{
					Effect = Session.GetHabbo().Effects().GetEffectNullable(25);

					if (Effect != null)
					{
						Effect.AddToQuantity();
					}
				}
				else
					Effect = AvatarEffectFactory.CreateNullable(Session.GetHabbo(), 25, 3600);

				if (Effect != null)
				{
					Session.SendMessage(new AvatarEffectAddedComposer(25, 3600));
					Session.GetHabbo().Effects().ApplyEffect(25);
					Session.GetHabbo().Effects().TryAdd(Effect);
					Session.SendWhisper("Efeito fogo adicionado!");
					Session.SendMessage(new AvatarEffectsComposer(Session.GetHabbo().Effects().GetAllEffects));

				}
				Item.ExtraData = "2";
				return;
			}
			else if (Item.ExtraData == "2")
			{
				Item.UpdateState(true, true);
				Item.RequestUpdate(0, true);
				AvatarEffect Effect = null;
				if (Session.GetHabbo().Effects().HasEffect(26))
				{
					Effect = Session.GetHabbo().Effects().GetEffectNullable(26);

					if (Effect != null)
					{
						Effect.AddToQuantity();
					}
				}
				else
					Effect = AvatarEffectFactory.CreateNullable(Session.GetHabbo(), 26, 3600);

				if (Effect != null)
				{
					Session.SendMessage(new AvatarEffectAddedComposer(26, 3600));
					Session.GetHabbo().Effects().ApplyEffect(26);
					Session.GetHabbo().Effects().TryAdd(Effect);
					Session.SendWhisper("Efeito cajado adicionado!");
					Session.SendMessage(new AvatarEffectsComposer(Session.GetHabbo().Effects().GetAllEffects));

				}
				Item.ExtraData = "3";
				return;
			}
			else if (Item.ExtraData == "3")
			{
				Item.UpdateState(true, true);
				Item.RequestUpdate(0, true);
				AvatarEffect Effect = null;
				if (Session.GetHabbo().Effects().HasEffect(23))
				{
					Effect = Session.GetHabbo().Effects().GetEffectNullable(23);

					if (Effect != null)
					{
						Effect.AddToQuantity();
					}
				}
				else
					Effect = AvatarEffectFactory.CreateNullable(Session.GetHabbo(), 23, 3600);

				if (Effect != null)
				{
					Session.SendMessage(new AvatarEffectAddedComposer(23, 3600));
					Session.GetHabbo().Effects().ApplyEffect(23);
					Session.GetHabbo().Effects().TryAdd(Effect);
					Session.SendWhisper("Efeito levitação adicionado!");
					Session.SendMessage(new AvatarEffectsComposer(Session.GetHabbo().Effects().GetAllEffects));

				}
				Item.ExtraData = "0";
				return;
			}
		}

		public void OnWiredTrigger(Item Item)
		{
			Item.ExtraData = "-1";
			Item.UpdateState(false, true);
			Item.RequestUpdate(4, true);
		}
	}
}