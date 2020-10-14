using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing.Users;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms;
using System;

namespace Galaxy.HabboHotel.Items.Interactor
{
    class InteractorCaixaThiago : IFurniInteractor
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
            RoomUser User = null;
            if (Session != null)
                User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            //  +-+-+-+-+-+ +-+-+-+-+-+
            //  |Arca Grega Fixado 
            //   +-+-+-+-+-+ +-+-+-+-+-+

            if (Item.BaseItem == 94593)
            {
                if (Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id).CurrentEffect == 186)
                {
                    Random random = new Random();
                    int mobi1 = 94594; // Chimera Grega
                    int mobi2 = 94595; // Hydra Grega
                    int mobi3 = 1100001496; // Minotauro Grego
                    int mobi4 = 94592; // Centauro Grego
                    string mobi01 = "Chimera Grega"; //Certo
                    string imagem01 = "santorini_r17_chimera"; //Certo
                    string mobi02 = "Hydra Grega"; //Certo
                    string imagem02 = "santorini_r17_hydra"; //Certo
                    string mobi03 = "Minotauro Grego"; //Certo
                    string imagem03 = "santorini_r17_minotaur"; //Certo
                    string mobi04 = "Centauro Grego"; //Certo
                    string imagem04 = "santorini_r17_centaur"; //Certo
                    int randomNumber = random.Next(1, 4);
                    if (Item.UserID != Session.GetHabbo().Id)
                    {
                        Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Esta caixa grega não te pertence."));
                        return;
                    }
                    Room Room = Session.GetHabbo().CurrentRoom;
                    Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);
                    if (randomNumber == 1)
                    {
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi1 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                        Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi1, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.SendMessage(new RoomNotificationComposer("icons/" + imagem01 + "_icon", 3, "Você acaba de ganha o raro grego : " + mobi01 + " Clique aqui para abrir o inventário!", "inventory/open/furni"));
                        if (Session.GetHabbo().Rank == 1)
                        {
                            GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem01 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na caixa grega o raro: " + mobi01, " !"));
                        }
                        return;
                    }
                    if (randomNumber == 2)
                    {
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi2 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                        Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi2, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.SendMessage(new RoomNotificationComposer("icons/" + imagem02 + "_icon", 3, "Você acaba de ganha o raro grego : " + mobi02 + " Clique aqui para abrir o inventário!", "inventory/open/furni"));
                        if (Session.GetHabbo().Rank == 1)
                        {
                            GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem02 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na caixa grega o raro: " + mobi02, " !"));
                        }
                        return;
                    }
                    if (randomNumber == 3)
                    {
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi3 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                        Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi3, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.SendMessage(new RoomNotificationComposer("icons/" + imagem03 + "_icon", 3, "Você acaba de ganha o raro grego : " + mobi03 + " Clique aqui para abrir o inventário!", "inventory/open/furni"));
                        if (Session.GetHabbo().Rank == 1)
                        {
                            GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem03 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na caixa grega o raro: " + mobi03, " !"));
                        }
                        return;
                    }
                    if (randomNumber == 4)
                    {
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi4 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                        Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi4, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.SendMessage(new RoomNotificationComposer("icons/" + imagem04 + "_icon", 3, "Você acaba de ganha o raro grego : " + mobi04 + " Clique aqui para abrir o inventário!", "inventory/open/furni"));
                        if (Session.GetHabbo().Rank == 1)
                        {
                            GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem04 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na caixa grega o raro: " + mobi04, " !"));
                        }
                        return;
                    }
                }
                else
                {
                    Session.SendWhisper("Ops, você não esta com a varinha! digite o comando :efeito 186");
                    return;
                }
            } 
            else
			{
                if (Item.UserID != Session.GetHabbo().Id)
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Este mobi não te pertence."));
                    return;
                }

                Room Room = Session.GetHabbo().CurrentRoom;
                if (Room == null)
                    return;

                RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (Actor == null)
                    return;

                if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) > 2)
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Chegue mais perto para abrir."));
                    return;

                }

                GalaxyServer.GetGame().GetPinataManager().ReceiveCrackableReward(Actor, Room, Item);
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
