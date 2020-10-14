using System;
using System.Linq;
using Galaxy.Core;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Catalog;
using Galaxy.HabboHotel.Catalog.Utilities;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Outgoing.Catalog
{
    public class CatalogPageComposer : ServerPacket
    {
        public CatalogPageComposer(CatalogPage Page, string CataMode, GameClient Session)
            : base(ServerPacketHeader.CatalogPageMessageComposer)
        {
            WriteInteger(Page.Id);
            WriteString(CataMode);
            WriteString(Page.Template);

            WriteInteger(Page.PageStrings1.Count);
            foreach (string s in Page.PageStrings1)
            {
                WriteString(s);
            }

            WriteInteger(Page.PageStrings2.Count);
            foreach (string s in Page.PageStrings2)
            {
                WriteString(s);
            }

            if (!Page.Template.Equals("frontpage") && !Page.Template.Equals("loyalty_vip_buy") && !Page.PageLink.Equals("ultimas_compras"))
            {
                WriteInteger(Page.Items.Count);
                foreach (CatalogItem Item in Page.Items.Values)
                {
                    WriteInteger(Item.Id);
                    WriteString(Item.Name);
                    WriteBoolean(false);
                    WriteInteger(Item.CostCredits);

                    if (Item.CostDiamonds > 0)
                    {
                        WriteInteger(Item.CostDiamonds);
                        WriteInteger(5);
                    }
                    else if (Item.CostPixels > 0)
                    {
                        WriteInteger(Item.CostPixels);
                        WriteInteger(0); 

                    }
                    else
                    {
                        WriteInteger(Item.CostGotw);
                        WriteInteger(103); 
                    }

                    WriteBoolean(Item.PredesignedId > 0 ? false : ItemUtility.CanGiftItem(Item));
                    if (Item.PredesignedId > 0)
                    {
                        WriteInteger(Page.PredesignedItems.Items.Count);
                        foreach (var predesigned in Page.PredesignedItems.Items.ToList())
                        {
                            ItemData Data = null;
                            if (GalaxyServer.GetGame().GetItemManager().GetItem(predesigned.Key, out Data)) { }
                            WriteString(Data.Type.ToString());
                            WriteInteger(Data.SpriteId);
                            WriteString(string.Empty);
                            WriteInteger(predesigned.Value);
                            WriteBoolean(false);
                        }

                        WriteInteger(0);
                        WriteBoolean(false);
                        WriteBoolean(true); 
                        WriteString("");
                    }
                    else if (Page.Deals.Count > 0)
                    {
                        foreach (var Deal in Page.Deals.Values)
                        {
                            WriteInteger(Deal.ItemDataList.Count);
                            foreach (var DealItem in Deal.ItemDataList.ToList())
                            {
                                WriteString(DealItem.Data.Type.ToString());
                                WriteInteger(DealItem.Data.SpriteId);
                                WriteString(string.Empty);
                                WriteInteger(DealItem.Amount);
                                WriteBoolean(false);
                            }

                            WriteInteger(0);
                            WriteBoolean(false);
                        }
                    }
                    else
                    {
                        WriteInteger(string.IsNullOrEmpty(Item.Badge) ? 1 : 2);
                        {
                            if (!string.IsNullOrEmpty(Item.Badge))
                            {
                                WriteString("b");
                                WriteString(Item.Badge);
                            }

                            WriteString(Item.Data.Type.ToString());
                            if (Item.Data.Type.ToString().ToLower() == "b")
                            {
                                WriteString(Item.Data.ItemName);
                            }
                            else
                            {
                                WriteInteger(Item.Data.SpriteId);
                                if (Item.Data.InteractionType == InteractionType.WALLPAPER || Item.Data.InteractionType == InteractionType.FLOOR || Item.Data.InteractionType == InteractionType.LANDSCAPE)
                                {
                                    WriteString(Item.Name.Split('_')[2]);
                                }
                                else if (Item.Data.InteractionType == InteractionType.BOT)
                                {
                                    CatalogBot CatalogBot = null;
                                    if (!GalaxyServer.GetGame().GetCatalog().TryGetBot(Item.ItemId, out CatalogBot))
                                        WriteString("hd-180-7.ea-1406-62.ch-210-1321.hr-831-49.ca-1813-62.sh-295-1321.lg-285-92");
                                    else
                                        WriteString(CatalogBot.Figure);
                                }
                                else if (Item.ExtraData != null)
                                {
                                    WriteString(Item.ExtraData ?? string.Empty);
                                }
                                WriteInteger(Item.Amount);
                                WriteBoolean(Item.IsLimited); 
                                if (Item.IsLimited)
                                {
                                    WriteInteger(Item.LimitedEditionStack);
                                    WriteInteger(Item.LimitedEditionStack - Item.LimitedEditionSells);
                                }
                            }
                            WriteInteger(0);
                            WriteBoolean(ItemUtility.CanSelectAmount(Item));

                            WriteBoolean(true);
                            WriteString(""); 
                        }
                    }
                }
            }
            else if (Page.PageLink == "ultimas_compras")
            {
                var UltimasCompras = Session.GetHabbo()._lastitems.Reverse();
                base.WriteInteger(UltimasCompras.Count());
                foreach (var Item in UltimasCompras.ToList())
                {
                    base.WriteInteger(Item.Value.Id);
                    base.WriteString(Item.Value.Name);
                    base.WriteBoolean(false);
                    base.WriteInteger(Item.Value.CostCredits);

                    if (Item.Value.CostDiamonds > 0)
                    {
                        base.WriteInteger(Item.Value.CostDiamonds);
                        base.WriteInteger(5);
                    }
                    else if (Item.Value.CostGotw > 0)
                    {
                        base.WriteInteger(Item.Value.CostGotw);
                        base.WriteInteger(103);
                    }
                    else
                    {
                        base.WriteInteger(Item.Value.CostPixels);
                        base.WriteInteger(0);
                    }
                    base.WriteBoolean(false);
                    base.WriteInteger(string.IsNullOrEmpty(Item.Value.Badge) ? 1 : 2);

                    if (!string.IsNullOrEmpty(Item.Value.Badge))
                    {
                        base.WriteString("b");
                        base.WriteString(Item.Value.Badge);
                    }

                    base.WriteString(Item.Value.Data.Type.ToString());
                    if (Item.Value.Data.Type.ToString().ToLower() == "b")
                    {
                        base.WriteString(Item.Value.Data.ItemName);
                    }
                    else
                    {
                        base.WriteInteger(Item.Value.Data.SpriteId);
                        if (Item.Value.Data.InteractionType == InteractionType.WALLPAPER || Item.Value.Data.InteractionType == InteractionType.FLOOR || Item.Value.Data.InteractionType == InteractionType.LANDSCAPE)
                        {
                            base.WriteString(Item.Value.Name.Split('_')[2]);
                        }
                        else if (Item.Value.Data.InteractionType == InteractionType.BOT)
                        {
                            CatalogBot CatalogBot = null;
                            if (!GalaxyServer.GetGame().GetCatalog().TryGetBot(Item.Value.ItemId, out CatalogBot))
                                base.WriteString("hd-180-7.ea-1406-62.ch-210-1321.hr-831-49.ca-1813-62.sh-295-1321.lg-285-92");
                            else
                                base.WriteString(CatalogBot.Figure);
                        }
                        else if (Item.Value.ExtraData != null)
                        {
                            base.WriteString(Item.Value.ExtraData != null ? Item.Value.ExtraData : string.Empty);
                        }
                        base.WriteInteger(Item.Value.Amount);
                        base.WriteBoolean(Item.Value.IsLimited);
                        if (Item.Value.IsLimited)
                        {
                            base.WriteInteger(Item.Value.LimitedEditionStack);
                            base.WriteInteger(Item.Value.LimitedEditionStack - Item.Value.LimitedEditionSells);
                        }
                    }
                    base.WriteInteger(0);
                    base.WriteBoolean(false);
                    base.WriteBoolean(true);
                    base.WriteString("");
                }

            }
            else
                base.WriteInteger(0);

            WriteInteger(-1);
            WriteBoolean(false);
            if (Page.Template.Equals("frontpage4"))
            {
                WriteInteger(4); 
                WriteInteger(1);
                WriteString(CatalogSettings.CATALOG_NOTICE_1); 
                WriteString(CatalogSettings.CATALOG_IMG_NOTICE_1); 
                WriteInteger(0);
                WriteString(CatalogSettings.CATALOG_URL_NOTICE_1); 
                WriteInteger(-1);
                WriteInteger(2);
                WriteString(CatalogSettings.CATALOG_NOTICE_2); 
                WriteString(CatalogSettings.CATALOG_IMG_NOTICE_2); 
                WriteInteger(0);
                WriteString(CatalogSettings.CATALOG_URL_NOTICE_2); 
                WriteInteger(-1);
                WriteInteger(3);
                WriteString(CatalogSettings.CATALOG_NOTICE_3); 
                WriteString(CatalogSettings.CATALOG_IMG_NOTICE_3); 
                WriteInteger(0);
                WriteString(CatalogSettings.CATALOG_URL_NOTICE_3);
                WriteInteger(-1);
                WriteInteger(4);
                WriteString(CatalogSettings.CATALOG_NOTICE_4); 
                WriteString(CatalogSettings.CATALOG_IMG_NOTICE_4);
                WriteInteger(0);
                WriteString(CatalogSettings.CATALOG_URL_NOTICE_4); 
                WriteInteger(-1);

                if (Page.Template.Equals("loyalty_vip_buy"))
                {
                    WriteInteger(0); 
                    WriteString("NORMAL");
                    WriteString("loyalty_vip_buy");
                    WriteInteger(2);
                    WriteString("hc2_clubtitle");
                    WriteString("clubcat_pic");
                    WriteInteger(0); 
                    WriteInteger(0);
                    WriteInteger(-1);
                    WriteBoolean(false);

                    if (Page.Template.Equals("club_gifts"))
                    {
                        WriteString("club_gifts");
                        WriteInteger(1);
                        WriteString(Convert.ToString(Page.PageStrings2));
                        WriteInteger(1);
                        WriteString(Convert.ToString(Page.PageStrings2));
                    }
                }
            }
        }
    }
}