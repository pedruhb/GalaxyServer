using Galaxy.Communication.Interfaces;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Galaxy.HabboHotel.Items;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MobiInfoCommand : IChatCommand
    {
        public string PermissionRequired => "command_mobi_info";
        public string Parameters => "[ID]";
        public string Description => "Visualizar informações do mobi.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1) // Pega o mobi da frente se não citar o ID
            {
                var itemid = "";
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                List<Item> Items = Room.GetGameMap().GetAllRoomItemForSquare(User.SquareInFront.X, User.SquareInFront.Y);

                if (Items.Count == 0)
                {
                    Session.SendWhisper("Não encontramos nenhum mobi em sua frente! você também pode usar :mobi [id]");
                    return;
                }

                foreach (Item _item in Items)
                {
                    DataRow ItemData = null;
                    DataRow FurniData = null;
                    DataRow UserData = null;
                    DataRow RoomData = null;
                    int Mobi = _item.Id;

                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT `id`,`user_id`,`room_id`,`base_item`,`limited_number`,`limited_stack` FROM items WHERE `id` = @id LIMIT 1");
                        dbClient.AddParameter("id", Mobi);
                        ItemData = dbClient.getRow();
                    }
                    if (ItemData == null)
                    {
                        Session.SendNotification("Ops, não há nenhum mobi com esse id (" + Mobi + ")!");
                        return;
                    }

                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT * FROM `furniture` WHERE `id` = '" + Convert.ToInt32(ItemData["base_item"]) + "' LIMIT 1");
                        FurniData = dbClient.getRow();
                        if (FurniData == null)
                        {
                            Session.SendWhisper("Não foi encontrado nenhum registro na furniture com esse mobi.");
                            return;
                        }
                    }
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT * FROM `users` WHERE `id` = '" + Convert.ToInt32(ItemData["user_id"]) + "' LIMIT 1");
                        UserData = dbClient.getRow();
                        if (UserData == null)
                        {
                            Session.SendWhisper("Dono do mobi não encontrado no banco de dados.");
                            return;
                        }
                    }
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        if (Convert.ToInt32(ItemData["room_id"]) != 0 || Convert.ToInt32(ItemData["room_id"]) != null)
                        {
                            dbClient.SetQuery("SELECT * FROM `rooms` WHERE `id` = '" + Convert.ToInt32(ItemData["room_id"]) + "' LIMIT 1");
                            RoomData = dbClient.getRow();
                            if (RoomData == null)
                            {
                                Session.SendWhisper("Dono do mobi não encontrado no banco de dados.");
                                return;
                            }
                        }
                    }

					if (GalaxyServer.Tipo == 0)
					{
						dynamic product = new Newtonsoft.Json.Linq.JObject();
						product.tipo = "mencao";
						product.mensagem = FurniData["item_name"] + ": " + Convert.ToInt32(FurniData["id"]);
						product.remetente = ":mobi";
						product.quarto = 0;
						GalaxyServer.SendUserJson(Session, product);

					}

					StringBuilder HabboInfo = new StringBuilder();
                    HabboInfo.Append("<b>Informações do mobi:</b>\r");
                    HabboInfo.Append("<b>ID do Item: </b>" + Convert.ToInt32(Mobi) + "\r");
                    HabboInfo.Append("<b>Dono: </b>" + UserData["username"] + "  (" + Convert.ToInt32(UserData["id"]) + ")" + "\r");
                    if (Convert.ToInt32(ItemData["limited_number"]) > 0)
                        HabboInfo.Append("<b>LTD: </b>Lote " + Convert.ToInt32(ItemData["limited_number"]) + " de " + Convert.ToInt32(ItemData["limited_stack"]) + "\r");

                    HabboInfo.Append("\r");
                    HabboInfo.Append("<b>Informações da furniture:</b>\r");
                    HabboInfo.Append("<b>ID do mobi:</b> " + Convert.ToInt32(FurniData["id"]) + "\r");
                    HabboInfo.Append("<b>Classname:</b> " + FurniData["item_name"] + "\r");
                    HabboInfo.Append("<b>Nome Público: </b>" + FurniData["public_name"] + "\r\r");

                    HabboInfo.Append("<b>Informações do quarto:</b>\r");
                    if (Convert.ToInt32(ItemData["room_id"]) > 0)
                    {
                        HabboInfo.Append("<b>Nome do quarto:</b> " + RoomData["caption"] + "\r");
                        HabboInfo.Append("<b>ID do quarto:</b> " + RoomData["id"] + "\r");
                        DataRow DonoQuarto = null;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT `id`,`username` FROM users WHERE `id` = @id LIMIT 1");
                            dbClient.AddParameter("id", RoomData["owner"]);
                            DonoQuarto = dbClient.getRow();
                        }
                        HabboInfo.Append("<b>Dono do quarto:</b> " + DonoQuarto["username"] + "  (" + DonoQuarto["id"] + ")" + "\r");

                        HabboInfo.Append("<b>Usuários no quarto: </b>" + RoomData["users_now"] + "\r");
                    }
                    else
                    {
                        HabboInfo.Append("<b>O mobi está no inventário.</b>");
                    }

                    Session.SendNotification(HabboInfo.ToString());
                }
            }
            else
            {
                DataRow ItemData = null;
                DataRow FurniData = null;
                DataRow UserData = null;
                DataRow RoomData = null;
                int Mobi = int.Parse(Params[1]);

                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `id`,`user_id`,`room_id`,`base_item`,`limited_number`,`limited_stack` FROM items WHERE `id` = @id LIMIT 1");
                    dbClient.AddParameter("id", Mobi);
                    ItemData = dbClient.getRow();
                }
                if (ItemData == null)
                {
                    Session.SendNotification("Ops, não há nenhum mobi com esse id (" + Mobi + ")!");
                    return;
                }

                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT * FROM `furniture` WHERE `id` = '" + Convert.ToInt32(ItemData["base_item"]) + "' LIMIT 1");
                    FurniData = dbClient.getRow();
                    if (FurniData == null)
                    {
                        Session.SendWhisper("Não foi encontrado nenhum registro na furniture com esse mobi.");
                        return;
                    }
                }
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT * FROM `users` WHERE `id` = '" + Convert.ToInt32(ItemData["user_id"]) + "' LIMIT 1");
                    UserData = dbClient.getRow();
                    if (UserData == null)
                    {
                        Session.SendWhisper("Dono do mobi não encontrado no banco de dados.");
                        return;
                    }
                }
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    if (Convert.ToInt32(ItemData["room_id"]) != 0 || Convert.ToInt32(ItemData["room_id"]) != null)
                    {
                        dbClient.SetQuery("SELECT * FROM `rooms` WHERE `id` = '" + Convert.ToInt32(ItemData["room_id"]) + "' LIMIT 1");
                        RoomData = dbClient.getRow();
                        if (RoomData == null)
                        {
                            Session.SendWhisper("Dono do mobi não encontrado no banco de dados.");
                            return;
                        }
                    }
                }

				if (GalaxyServer.Tipo == 0)
				{
					dynamic product = new Newtonsoft.Json.Linq.JObject();
					product.tipo = "mencao";
					product.mensagem = FurniData["item_name"] + ": " + Convert.ToInt32(FurniData["id"]);
					product.remetente = ":mobi";
					product.quarto = 0;
					GalaxyServer.SendUserJson(Session, product);
				}

				StringBuilder HabboInfo = new StringBuilder();
                HabboInfo.Append("<b>Informações do mobi:</b>\r");
                HabboInfo.Append("<b>ID do Item: </b>" + Convert.ToInt32(Mobi) + "\r");
                HabboInfo.Append("<b>Dono: </b>" + UserData["username"] + "  (" + Convert.ToInt32(UserData["id"]) + ")" + "\r");
                if (Convert.ToInt32(ItemData["limited_number"]) > 0)
                    HabboInfo.Append("<b>LTD: </b>Lote " + Convert.ToInt32(ItemData["limited_number"]) + " de " + Convert.ToInt32(ItemData["limited_stack"]) + "\r");

                HabboInfo.Append("\r");
                HabboInfo.Append("<b>Informações da furniture:</b>\r");
                HabboInfo.Append("<b>ID do mobi:</b> " + Convert.ToInt32(FurniData["id"]) + "\r");
                HabboInfo.Append("<b>Classname:</b> " + FurniData["item_name"] + "\r");
                HabboInfo.Append("<b>Nome Público: </b>" + FurniData["public_name"] + "\r\r");

                HabboInfo.Append("<b>Informações do quarto:</b>\r");
                if (Convert.ToInt32(ItemData["room_id"]) > 0)
                {
                    HabboInfo.Append("<b>Nome do quarto:</b> " + RoomData["caption"] + "\r");
                    HabboInfo.Append("<b>ID do quarto:</b> " + RoomData["id"] + "\r");
                    DataRow DonoQuarto = null;
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT `id`,`username` FROM users WHERE `id` = @id LIMIT 1");
                        dbClient.AddParameter("id", RoomData["owner"]);
                        DonoQuarto = dbClient.getRow();
                    }
                    HabboInfo.Append("<b>Dono do quarto:</b> " + DonoQuarto["username"] + "  (" + DonoQuarto["id"] + ")" + "\r");

                    HabboInfo.Append("<b>Usuários no quarto: </b>" + RoomData["users_now"] + "\r");
                }
                else
                {
                    HabboInfo.Append("<b>O mobi está no inventário.</b>");
                }

                Session.SendNotification(HabboInfo.ToString());
            }
        }

    }
        }






