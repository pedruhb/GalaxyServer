﻿using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Items;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class EjectAllCommand :IChatCommand
    {
        public string PermissionRequired => "command_ejectall";
        public string Parameters => "";
        public string Description => "Eliminar todos os elementos da sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().Id == Room.OwnerId)
            {
                //Let us check anyway.
                if (!Room.CheckRights(Session, true))
                    return;

                foreach (Item Item in Room.GetRoomItemHandler().GetWallAndFloor.ToList())
                {
                    if (Item == null || Item.UserID == Session.GetHabbo().Id)
                        continue;

                    GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(Item.UserID);
                    if (TargetClient != null && TargetClient.GetHabbo() != null)
                    {
                        Room.GetRoomItemHandler().RemoveFurniture(TargetClient, Item.Id);
                        TargetClient.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, Item.BaseItem, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        TargetClient.GetHabbo().GetInventoryComponent().UpdateItems(false);
                    }
                    else
                    {
                        Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.runFastQuery("UPDATE `items` SET `room_id` = '0' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                    }
                }
            }
            else
            {
                foreach (Item Item in Room.GetRoomItemHandler().GetWallAndFloor.ToList())
                {
                    if (Item == null || Item.UserID != Session.GetHabbo().Id)
                        continue;

                    GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(Item.UserID);
                    if (TargetClient != null && TargetClient.GetHabbo() != null)
                    {
                        Room.GetRoomItemHandler().RemoveFurniture(TargetClient, Item.Id);
                        TargetClient.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, Item.BaseItem, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        TargetClient.GetHabbo().GetInventoryComponent().UpdateItems(false);
                    }
                    else
                    {
                        Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.runFastQuery("UPDATE `items` SET `room_id` = '0' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                    }
                }
            }
        }
    }
}
