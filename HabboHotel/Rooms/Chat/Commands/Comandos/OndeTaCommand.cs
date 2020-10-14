using Galaxy.Communication.Interfaces;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Galaxy.HabboHotel.Items;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	internal class OndeTaCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get
			{
				return "";
			}
		}

		public string Parameters
		{
			get
			{
				return "";
			}
		}

		public string Description
		{
			get
			{
				return "Fala que página do catálogo o mobi se encontra.";
			}
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{
			var itemid = "";
			RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
			List<Item> Items = Room.GetGameMap().GetAllRoomItemForSquare(User.SquareInFront.X, User.SquareInFront.Y);

            if(Items.Count == 0)
            {
                Session.SendWhisper("Não encontramos nenhum mobi em sua frente!");
                return;
            }

			foreach (Item _item in Items)
			{
				using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.SetQuery("SELECT base_item FROM items WHERE id='" + _item.Id + "' LIMIT 1");
					DataTable gUsersTable = dbClient.getTable();
					foreach (DataRow Row in gUsersTable.Rows)
					{
						itemid = Convert.ToString(Row["base_item"]);
					}
				}

				using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.SetQuery("SELECT page_id,(SELECT caption FROM catalog_pages WHERE id = page_id) AS nome FROM catalog_items WHERE item_id= " + itemid + " LIMIT 1");
					DataTable gUsersTable2 = dbClient.getTable();
					foreach (DataRow Row in gUsersTable2.Rows)
					{
						Session.SendWhisper("O mobi está na página " + Convert.ToString(Row["nome"]).Replace("HOTELNAME", GalaxyServer.HotelName));
					}
				}
			}
		}
	}
}

