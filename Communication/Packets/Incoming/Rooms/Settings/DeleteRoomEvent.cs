using System.Linq;
using System.Collections.Generic;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Database.Interfaces;
using Galaxy.Communication.Packets.Outgoing.Rooms.Settings;
using Galaxy.Communication.Packets.Outgoing.Navigator;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Settings
{
    class DeleteRoomEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().Rank > 10 && Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().UsersRooms == null)
                return;

            int RoomId = Packet.PopInt();
            if (RoomId == 0)
                return;

            Room Room;

            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(RoomId, out Room))
                return;

            RoomData data = Room.RoomData;
            if (data == null)
                return;

            if (Room.OwnerId != Session.GetHabbo().Id && !Session.GetHabbo().GetPermissions().HasRight("room_delete_any"))
                return;

            List<Item> ItemsToRemove = new List<Item>();
            foreach (Item Item in Room.GetRoomItemHandler().GetWallAndFloor.ToList())
            {
                if (Item == null)
                    continue;

                if (Item.GetBaseItem().InteractionType == InteractionType.MOODLIGHT)
                {
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("DELETE FROM `room_items_moodlight` WHERE `item_id` = @itemId LIMIT 1");
                        dbClient.AddParameter("itemId", Item.Id);
                        dbClient.RunQuery();
                    }
                }

                ItemsToRemove.Add(Item);
            }

            foreach (Item Item in ItemsToRemove)
            {
                GameClient targetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(Item.UserID);
                if (targetClient != null && targetClient.GetHabbo() != null)
                {
                    Room.GetRoomItemHandler().RemoveFurniture(targetClient, Item.Id);
                    targetClient.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, Item.BaseItem, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    targetClient.GetHabbo().GetInventoryComponent().UpdateItems(false);
                }
                else
                {
                    Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("UPDATE `items` SET `room_id` = '0' WHERE `id` = @itemId LIMIT 1");
                        dbClient.AddParameter("itemId", Item.Id);
                        dbClient.RunQuery();
                    }
                }
            }

            GalaxyServer.GetGame().GetRoomManager().UnloadRoom(Room.Id);

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("DELETE FROM `user_roomvisits` WHERE `room_id` = '" + RoomId + "'");
                dbClient.runFastQuery("DELETE FROM `rooms` WHERE `id` = '" + RoomId + "' LIMIT 1");
                dbClient.runFastQuery("DELETE FROM `user_favorites` WHERE `room_id` = '" + RoomId + "'");
                dbClient.runFastQuery("DELETE FROM `items` WHERE `room_id` = '" + RoomId + "'");
                dbClient.runFastQuery("DELETE FROM `room_rights` WHERE `room_id` = '" + RoomId + "'");
                dbClient.runFastQuery("UPDATE `users` SET `home_room` = '0' WHERE `home_room` = '" + RoomId + "'");
                dbClient.runFastQuery("DELETE FROM `navigator_staff_picks` WHERE `room_id` = '" + RoomId + "'");
            }

			Room.SendMessage(new RoomSettingsSavedComposer(RoomId));
			Room.SendMessage(new RoomInfoUpdatedComposer(RoomId));

			RoomData removedRoom = (from p in Session.GetHabbo().UsersRooms where p.Id == RoomId select p).SingleOrDefault();
            if (removedRoom != null)
                Session.GetHabbo().UsersRooms.Remove(removedRoom);
        }
    }
}