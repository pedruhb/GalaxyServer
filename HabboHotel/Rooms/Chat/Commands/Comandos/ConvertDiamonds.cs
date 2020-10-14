using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Items;
using System;
using System.Data;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ConvertDiamonds : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_convert_credits"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Gere os diamantes do seu inventário."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            int TotalDuckets = 0;

            try
            {
                DataTable Table = null;
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `id` FROM `items` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND (`room_id`=  '0' OR `room_id` = '')");
                    Table = dbClient.getTable();
                }

                if (Table == null)
                {
                    Session.SendWhisper("Você não possui nenhum diamante em seu inventário!");
                    return;
                }

                foreach (DataRow Row in Table.Rows)
                {
                    Item Item = Session.GetHabbo().GetInventoryComponent().GetItem(Convert.ToInt32(Row[0]));
                    if (Item == null)
                        continue;

                    if (!Item.GetBaseItem().ItemName.StartsWith("DI_") && !Item.GetBaseItem().ItemName.StartsWith("DIA_") && !Item.GetBaseItem().ItemName.StartsWith("DF_"))
                        continue;

                    if (Item.RoomId > 0)
                        continue;

                    string[] Split = Item.GetBaseItem().ItemName.Split('_');
                    int Value = int.Parse(Split[1]);

                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }

                    Session.GetHabbo().GetInventoryComponent().RemoveItem(Item.Id);

                    TotalDuckets += Value;

                    if (Value > 0)
                    {
                        Session.GetHabbo().Diamonds += Value;
                        Session.SendMessage(new ActivityPointsComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Diamonds, Session.GetHabbo().GOTWPoints));
                    }
                }

                if (TotalDuckets > 0)
                    Session.SendWhisper("Foram convertidos " + TotalDuckets + " diamantes de seu inventário!");
                else
                    Session.SendWhisper("Você não possui nenhum diamante em seu inventário!");
            }
            catch
            {
                Session.SendNotification("Você não possui nenhum diamante em seu inventário!");
            }
        }
    }
}

