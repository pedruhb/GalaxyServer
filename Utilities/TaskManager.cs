using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy.Utilities
{
    class TaskManager
    {
        public static void DeleteItemsCache(GameClient _client, ConcurrentDictionary<int, Item> _floorItems, ConcurrentDictionary<int, Item> _wallItems)
        {
            if (_client != null)
            {
                _client.GetHabbo().GetInventoryComponent()._floorItems.Clear();
                _client.GetHabbo().GetInventoryComponent()._wallItems.Clear();
                ICollection<Item> FloorItems = _floorItems.Values;
                ICollection<Item> WallItems = _wallItems.Values;
                _client.SendMessage(new FurniListUpdateComposer());
                //Task t = Task.Factory.StartNew(() => _client.SendMessage(new FurniListComposer(FloorItems.ToList(), WallItems)));
            }
        }
        public static Item MultipleItems(ItemData Data, Habbo Habbo, string ExtraData, int Amount, int GroupId, string InteractionCase, int i = 0)
        {
            Item CreateItem = null;
            int id = 0;
            //CreateItem = new Item(i, 0, Data.Id, ExtraData, 0, 0, 0, 0, Habbo.Id, GroupId, 0, 0, "");
            //if (InteractionCase.Equals("moodlight"))
            //{
            //    ItemFactory.CreateMoodlightData(CreateItem);
            //}
            //else if (InteractionCase.Equals("toner"))
            //{
            //    ItemFactory.CreateTonerData(CreateItem);
            //}
            //Habbo.GetInventoryComponent().TryAddItem(CreateItem);
            //Habbo.GetClient().SendMessage(new FurniListNotificationComposer(CreateItem.Id, 1));
            //Habbo.GetClient().SendMessage(new FurniListUpdateComposer());
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `items` (base_item,user_id,room_id,x,y,z,wall_pos,rot,extra_data) VALUES(@did,@uid,@rid,@x,@y,@z,@wallpos,@rot,@flags);");
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("rid", 0);
                dbClient.AddParameter("x", 0);
                dbClient.AddParameter("y", 0);
                dbClient.AddParameter("z", 0);
                dbClient.AddParameter("wallpos", "");
                dbClient.AddParameter("rot", 0);
                dbClient.AddParameter("flags", ExtraData);
                id = Convert.ToInt32(dbClient.InsertQuery());
            }

            CreateItem = new Item(id, 0, Data.Id, ExtraData, 0, 0, 0, 0, Habbo.Id, GroupId, 0, 0, ""); //cache
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                if (GroupId > 0)
                {
                    dbClient.SetQuery("INSERT INTO `items_groups` (`id`, `group_id`) VALUES (@id, @gid)");
                    dbClient.AddParameter("id", CreateItem.Id);
                    dbClient.AddParameter("gid", GroupId);
                    dbClient.RunQuery();
                }
            }

            if (InteractionCase.Equals("moodlight"))
            {
                ItemFactory.CreateMoodlightData(CreateItem);
            }
            else if (InteractionCase.Equals("toner"))
            {
                ItemFactory.CreateTonerData(CreateItem);
            }
            Habbo.GetInventoryComponent().TryAddItem(CreateItem);
            //Habbo.GetClient().SendMessage(new FurniListNotificationComposer(CreateItem.Id, 1));
            //Habbo.GetClient().SendMessage(new FurniListUpdateComposer());

            return CreateItem;
        }
        public static void MultipleTeleport(ItemData Data, Habbo Habbo, int GroupId = 0)
        {

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `items` (base_item,user_id,room_id,x,y,z,wall_pos,rot,extra_data) VALUES(@did,@uid,@rid,@x,@y,@z,@wallpos,@rot,@flags);");
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("rid", 0);
                dbClient.AddParameter("x", 0);
                dbClient.AddParameter("y", 0);
                dbClient.AddParameter("z", 0);
                dbClient.AddParameter("wallpos", "");
                dbClient.AddParameter("rot", 0);
                dbClient.AddParameter("flags", "");

                int Item1Id = Convert.ToInt32(dbClient.InsertQuery());

                dbClient.SetQuery("INSERT INTO `items` (base_item,user_id,room_id,x,y,z,wall_pos,rot,extra_data) VALUES(@did,@uid,@rid,@x,@y,@z,@wallpos,@rot,@flags);");
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("rid", 0);
                dbClient.AddParameter("x", 0);
                dbClient.AddParameter("y", 0);
                dbClient.AddParameter("z", 0);
                dbClient.AddParameter("wallpos", "");
                dbClient.AddParameter("rot", 0);
                dbClient.AddParameter("flags", Item1Id.ToString());

                int Item2Id = Convert.ToInt32(dbClient.InsertQuery());

                Item Item1 = new Item(Item1Id, 0, Data.Id, "", 0, 0, 0, 0, Habbo.Id, GroupId, 0, 0, "");
                Item Item2 = new Item(Item2Id, 0, Data.Id, "", 0, 0, 0, 0, Habbo.Id, GroupId, 0, 0, "");

                dbClient.SetQuery("INSERT INTO `room_items_tele_links` (`tele_one_id`, `tele_two_id`) VALUES (" + Item1Id + ", " + Item2Id + "), (" + Item2Id + ", " + Item1Id + ")");
                dbClient.RunQuery();
                Habbo.GetInventoryComponent().TryAddItem(Item1);
                Habbo.GetClient().SendMessage(new FurniListNotificationComposer(Item1.Id, 1));
                Habbo.GetInventoryComponent().TryAddItem(Item2);
                Habbo.GetClient().SendMessage(new FurniListNotificationComposer(Item2.Id, 1));
                Habbo.GetClient().SendMessage(new FurniListUpdateComposer());
            }
        }
    }
}
