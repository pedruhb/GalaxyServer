using System;
using System.Linq;
using System.Collections.Generic;

using Galaxy.HabboHotel.Items;
using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;

using Galaxy.Communication.Packets.Outgoing.Rooms.Furni;
using Galaxy.HabboHotel.Items.Crafting;
using Galaxy.Database.Interfaces;
using System.Data;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Furni
{
	class SetCraftingRecipeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            var result = Packet.PopString();

            CraftingRecipe recipe = null;
            foreach (CraftingRecipe Receta in GalaxyServer.GetGame().GetCraftingManager().CraftingRecipes.Values)
            {
                if (Receta.Result.Contains(result))
                {
                    recipe = Receta;
                    break;
                }
            }

            var Final = GalaxyServer.GetGame().GetCraftingManager().GetRecipe(recipe.Id);
            if (Final == null) return;
            Session.SendMessage(new CraftingRecipeComposer(Final));
            if (Final != null)
            {
                int craftingTable = Packet.PopInt();

                List<Item> items = new List<Item>();

                var count = Packet.PopInt();
                for (var i = 1; i <= count; i++)
                {
                    var id = Packet.PopInt();

                    var item = Session.GetHabbo().GetInventoryComponent().GetItem(id);
                    if (item == null || items.Contains(item))
                        return;

                    items.Add(item);
                }

                foreach (var Receta in GalaxyServer.GetGame().GetCraftingManager().CraftingRecipes)
                {
                    bool found = false;

                    foreach (var item in Receta.Value.ItemsNeeded)
                    {
                        if (item.Value != items.Count(item2 => item2.GetBaseItem().ItemName == item.Key))
                        {
                            found = false;
                            break;
                        }
                        found = true;
                    }

                    if (found == false)
                        continue;

                    recipe = Receta.Value;
                    break;
                }

                if (recipe == null) return;
                ItemData resultItem = GalaxyServer.GetGame().GetItemManager().GetItemByName(recipe.Result);
                if (resultItem == null) return;
                bool success = true;
                foreach (var need in recipe.ItemsNeeded)
                {
                    for (var i = 1; i <= need.Value; i++)
                    {
                        ItemData item = GalaxyServer.GetGame().GetItemManager().GetItemByName(need.Key);
                        if (item == null)
                        {
                            success = false;
                            continue;
                        }

                        var inv = Session.GetHabbo().GetInventoryComponent().GetFirstItemByBaseId(item.Id);
                        if (inv == null)
                        {
                            success = false;
                            continue;
                        }

                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT * FROM `items` WHERE user_id = '" + Session.GetHabbo() + "'");
                            DataRow Table = dbClient.getRow();
                        }
                        if (success)
                        {
                        using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor()) dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + inv.Id + "' AND `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                        Session.GetHabbo().GetInventoryComponent().RemoveItem(inv.Id);

                        Session.SendMessage(new CraftingResultComposer(recipe, true));
                        Session.SendMessage(new CraftableProductsComposer());
                        GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CrystalCracker", 1);
                        Session.SendNotification("Opa," + Session.GetHabbo().Username + " você craftou o item " + resultItem.Id + "!\n\n Status: " + Session.GetHabbo().craftThiago + "!\n\n!");

                        Session.GetHabbo().GetInventoryComponent().AddNewItem(0, resultItem.Id, "", 0, true, false, 0, 0);
                        Session.SendMessage(new FurniListUpdateComposer());
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.GetHabbo().craftThiago = false;
                        }
                    }
                }
            }
        }
    }
}