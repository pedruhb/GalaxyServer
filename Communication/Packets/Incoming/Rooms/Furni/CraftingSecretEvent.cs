using System;
using System.Linq;
using System.Collections.Generic;

using Galaxy.HabboHotel.Items;
using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;

using Galaxy.Communication.Packets.Outgoing.Rooms.Furni;
using Galaxy.HabboHotel.Items.Crafting;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Furni
{
    class CraftSecretEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
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

            CraftingRecipe recipe = null;
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

                    using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor()) dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + inv.Id + "' AND `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    Session.GetHabbo().GetInventoryComponent().RemoveItem(inv.Id);
                }
            }

            Session.GetHabbo().GetInventoryComponent().UpdateItems(true);

            Session.SendMessage(new CraftingResultComposer(recipe, true));
            Session.SendMessage(new CraftableProductsComposer());
            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CrystalCracker", 1);
            Session.SendNotification("Olá " + Session.GetHabbo().Username + " você craftou o item " + resultItem.Id + "!\n\n Você teve sorte!");

            Session.GetHabbo().GetInventoryComponent().AddNewItem(0, resultItem.Id, "", 0, true, false, 0, 0);
            Session.SendMessage(new FurniListUpdateComposer());
            Session.GetHabbo().GetInventoryComponent().UpdateItems(true);

            if (success)
            {
                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, resultItem.Id, "", 0, true, false, 0, 0);
                Session.SendMessage(new FurniListUpdateComposer());
                Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                Session.SendMessage(new CraftableProductsComposer());


                switch (recipe.Type)
                {
                    case 1:
                    GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CrystalCracker", 1);
                    break;

                    case 2:
                    GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                    break;

                    case 3:
                    GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                    break;
                }
            }

            Session.SendMessage(new CraftingResultComposer(recipe, success));
        }
    }
}