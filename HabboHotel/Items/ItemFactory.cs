using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Galaxy.HabboHotel.Users;

using Galaxy.Database.Interfaces;
using System.Threading.Tasks;
using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Utilities;
using System.Threading;

namespace Galaxy.HabboHotel.Items
{
    public class ItemFactory
    {
        public static Item CreateSingleItemNullable(ItemData Data, Habbo Habbo, string ExtraData, string DisplayFlags, int GroupId = 0, int LimitedNumber = 0, int LimitedStack = 0)
        {
            if(Data.InteractionType.ToString().ToLower() == "exchange")
            {
                Thread.Sleep(400);
            }

            if (Data == null) throw new InvalidOperationException("Data cannot be null.");

            Item Item = new Item(0, 0, Data.Id, ExtraData, 0, 0, 0, 0, Habbo.Id, GroupId, LimitedNumber, LimitedStack, "");

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `items` (base_item,user_id,room_id,x,y,z,wall_pos,rot,extra_data,`limited_number`,`limited_stack`) VALUES (@did,@uid,@rid,@x,@y,@z,@wall_pos,@rot,@extra_data, @limited_number, @limited_stack)");
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("rid", 0);
                dbClient.AddParameter("x", 0);
                dbClient.AddParameter("y", 0);
                dbClient.AddParameter("z", 0);
                dbClient.AddParameter("wall_pos", "");
                dbClient.AddParameter("rot", 0);
                dbClient.AddParameter("extra_data", ExtraData);
                dbClient.AddParameter("limited_number", LimitedNumber);
                dbClient.AddParameter("limited_stack", LimitedStack);
                Item.Id = Convert.ToInt32(dbClient.InsertQuery());

                if (GroupId > 0)
                {
                    dbClient.SetQuery("INSERT INTO `items_groups` (`id`, `group_id`) VALUES (@id, @gid)");
                    dbClient.AddParameter("id", Item.Id);
                    dbClient.AddParameter("gid", GroupId);
                    dbClient.RunQuery();
                }
                return Item;
            }
        }

        public static Item CreateSingleItem(ItemData Data, Habbo Habbo, string ExtraData, string DisplayFlags, int ItemId, int LimitedNumber = 0, int LimitedStack = 0)
        {

            if (Data.InteractionType.ToString().ToLower() == "exchange")
            {
                Thread.Sleep(400);
            }

            if (Data == null) throw new InvalidOperationException("Data cannot be null.");

            Item Item = new Item(ItemId, 0, Data.Id, ExtraData, 0, 0, 0, 0, Habbo.Id, 0, LimitedNumber, LimitedStack, "");

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `items` (`id`,base_item,user_id,room_id,x,y,z,wall_pos,rot,extra_data,`limited_number`,`limited_stack`) VALUES (@id, @did,@uid,@rid,@x,@y,@z,@wall_pos,@rot,@extra_data, @limited_number, @limited_stack)");
                dbClient.AddParameter("id", ItemId);
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("rid", 0);
                dbClient.AddParameter("x", 0);
                dbClient.AddParameter("y", 0);
                dbClient.AddParameter("z", 0);
                dbClient.AddParameter("wall_pos", "");
                dbClient.AddParameter("rot", 0);
                dbClient.AddParameter("extra_data", ExtraData);
                dbClient.AddParameter("limited_number", LimitedNumber);
                dbClient.AddParameter("limited_stack", LimitedStack);
                dbClient.RunQuery();

                return Item;
            }
        }

        public static Item CreateGiftItem(ItemData Data, Habbo Habbo, string ExtraData, string DisplayFlags, int ItemId, int LimitedNumber = 0, int LimitedStack = 0)
        {

            if (Data.InteractionType.ToString().ToLower() == "exchange")
            {
                Thread.Sleep(400);
            }

            if (Data == null) throw new InvalidOperationException("Data cannot be null.");

            Item Item = new Item(ItemId, 0, Data.Id, ExtraData, 0, 0, 0, 0, Habbo.Id, 0, LimitedNumber, LimitedStack, "");

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `items` (`id`,base_item,user_id,room_id,x,y,z,wall_pos,rot,extra_data,`limited_number`,`limited_stack`) VALUES (@id, @did,@uid,@rid,@x,@y,@z,@wall_pos,@rot,@extra_data, @limited_number, @limited_stack)");
                dbClient.AddParameter("id", ItemId);
                dbClient.AddParameter("did", Data.Id);
                dbClient.AddParameter("uid", Habbo.Id);
                dbClient.AddParameter("rid", 0);
                dbClient.AddParameter("x", 0);
                dbClient.AddParameter("y", 0);
                dbClient.AddParameter("z", 0);
                dbClient.AddParameter("wall_pos", "");
                dbClient.AddParameter("rot", 0);
                dbClient.AddParameter("extra_data", ExtraData);
                dbClient.AddParameter("limited_number", LimitedNumber);
                dbClient.AddParameter("limited_stack", LimitedStack);
                dbClient.RunQuery();

                return Item;
            }
        }

        public static void ForItems(int Amount, Habbo Habbo)
        {
            Random randnumbers = new Random();
            int Random;
            for (int b = 0; b < Amount; b++)
            {
                Random = randnumbers.Next(0, 9999999);
                Habbo.GetClient().SendMessage(new FurniListNotificationComposer(Random, 1));
            }
        }
        public static List<Item> CreateMultipleItems(ItemData Data, Habbo Habbo, string ExtraData, int Amount, string InteractionCase = "default", int GroupId = 0)
        {

            if (Data.InteractionType.ToString().ToLower() == "exchange")
            {
                Thread.Sleep(400);

                if(Amount > 1)
                Habbo.GetClient().Disconnect();

            }
            if (Data == null) throw new InvalidOperationException("Data cannot be null.");
            List<Item> Items = new List<Item>();
            Task t = Task.Run(() => ForItems(Amount, Habbo));
            ThreadPool.SetMinThreads(10, 10);
            Parallel.For(0, Amount, x => TaskManager.MultipleItems(Data, Habbo, ExtraData, Amount, GroupId, InteractionCase.ToLower()));
            ThreadPool.SetMinThreads(4, 4);
            Habbo.GetClient().SendMessage(new FurniListUpdateComposer());
            return Items;
        }

        public static void CreateTeleporterItems(ItemData Data, Habbo Habbo, int AmountPurchase, int GroupId = 0)
        {
            ThreadPool.SetMinThreads(10, 10);
            Parallel.For(0, AmountPurchase, x => TaskManager.MultipleTeleport(Data, Habbo));
            ThreadPool.SetMinThreads(4, 4);
            //Task t = null;
            //for (int i = 0; i < AmountPurchase; i++)
            //{
            //    //t = Task.Factory.StartNew(() => TaskManager.MultipleTeleport(Data, Habbo));
            //    t = Task.Run(() => TaskManager.MultipleTeleport(Data, Habbo));
            //}

        }

        public static void CreateMoodlightData(Item Item)
        {
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `room_items_moodlight` (`id`, `enabled`, `current_preset`, `preset_one`, `preset_two`, `preset_three`) VALUES (@id, '0', 1, @preset, @preset, @preset);");
                dbClient.AddParameter("id", Item.Id);
                dbClient.AddParameter("preset", "#000000,255,0");
                dbClient.RunQuery();
            }
        }

        public static void CreateTonerData(Item Item)
        {
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `room_items_toner` (`id`, `data1`, `data2`, `data3`, `enabled`) VALUES (@id, 0, 0, 0, '0')");
                dbClient.AddParameter("id", Item.Id);
                dbClient.RunQuery();
            }
        }
    }
}
