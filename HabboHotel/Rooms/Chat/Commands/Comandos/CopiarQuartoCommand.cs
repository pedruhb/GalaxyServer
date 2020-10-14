using Galaxy.Communication.Packets.Outgoing.Navigator;
using Galaxy.Core;
using Galaxy.HabboHotel.Catalog.PredesignedRooms;
using Galaxy.HabboHotel.Rooms.Chat.Commands;
using Galaxy.Utilities;
using System;
using System.Linq;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class CopiarQuartoCommand : IChatCommand
	{
		public string PermissionRequired => "command_addpredesigned";
		public string Parameters => "";
		public string Description => "Copia o quarto de um usuário!";

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{
			if (Session.GetHabbo().isLoggedIn == false)
			{
				Session.SendWhisper("Você não logou como staff!");
				return;
			}
			if (Room == null) return;
			StringBuilder itemAmounts = new StringBuilder(), floorItemsData = new StringBuilder(), wallItemsData = new StringBuilder(),
				decoration = new StringBuilder();
			var floorItems = Room.GetRoomItemHandler().GetFloor;
			var wallItems = Room.GetRoomItemHandler().GetWall;
			foreach (var roomItem in floorItems)
			{
				var itemCount = floorItems.Count(item => item.BaseItem == roomItem.BaseItem);
				if (!itemAmounts.ToString().Contains(roomItem.BaseItem + "," + itemCount + ";"))
					itemAmounts.Append(roomItem.BaseItem + "," + itemCount + ";");

				floorItemsData.Append(roomItem.BaseItem + "$$$$" + roomItem.GetX + "$$$$" + roomItem.GetY + "$$$$" + roomItem.GetZ +
					"$$$$" + roomItem.Rotation + "$$$$" + roomItem.ExtraData + ";");
			}
			foreach (var roomItem in wallItems)
			{
				var itemCount = wallItems.Count(item => item.BaseItem == roomItem.BaseItem);
				if (!itemAmounts.ToString().Contains(roomItem.BaseItem + "," + itemCount + ";"))
					itemAmounts.Append(roomItem.BaseItem + "," + itemCount + ";");

				wallItemsData.Append(roomItem.BaseItem + "$$$$" + roomItem.wallCoord + "$$$$" + roomItem.ExtraData + ";");
			}

			decoration.Append(Room.RoomData.FloorThickness + ";" + Room.RoomData.WallThickness + ";" +
				Room.RoomData.Model.WallHeight + ";" + Room.RoomData.Hidewall + ";" + Room.RoomData.Wallpaper + ";" +
				Room.RoomData.Landscape + ";" + Room.RoomData.Floor);
			var idPredesigned = (uint)0;
			using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
			{
				dbClient.SetQuery("INSERT INTO catalog_predesigned_rooms(room_model,flooritems,wallitems,catalogitems,room_id,room_decoration) VALUES('" + Room.RoomData.ModelName +
					"', '" + floorItemsData + "', '" + wallItemsData + "', '" + itemAmounts + "', " + Room.Id + ", '" + decoration + "');");
				var predesignedId = (uint)dbClient.InsertQuery();
				idPredesigned = predesignedId;

				GalaxyServer.GetGame().GetCatalog().GetPredesignedRooms().predesignedRoom.Add(predesignedId,
					new PredesignedRooms(predesignedId, (uint)Room.Id, Room.RoomData.ModelName,
						floorItemsData.ToString().TrimEnd(';'), wallItemsData.ToString().TrimEnd(';'),
						itemAmounts.ToString().TrimEnd(';'), decoration.ToString()));
			}

			/// cria quarto
			 
			#region SELECT ROOM AND CREATE NEW
			var predesigned = GalaxyServer.GetGame().GetCatalog().GetPredesignedRooms().predesignedRoom[idPredesigned];
			var decoration2 = predesigned.RoomDecoration;
			//NOMBRES DE LA SALA & Sú descripción.
			var createRoom = GalaxyServer.GetGame().GetRoomManager().CreateRoom(Session, "Cópia - "+Room.RoomData.Name, "Cópia do quarto " + Room.RoomData.Id + ".", predesigned.RoomModel, 1, 25, 1);

			createRoom.FloorThickness = int.Parse(decoration2[0]);
			createRoom.WallThickness = int.Parse(decoration2[1]);
			createRoom.Model.WallHeight = int.Parse(decoration2[2]);
			createRoom.Hidewall = ((decoration2[3] == "True") ? 1 : 0);
			createRoom.Wallpaper = decoration2[4];
			createRoom.Landscape = decoration2[5];
			createRoom.Floor = decoration2[6];
			var newRoom = GalaxyServer.GetGame().GetRoomManager().LoadRoom(createRoom.Id);
			#endregion

			#region CREATE FLOOR ITEMS
			if (predesigned.FloorItems != null)
				foreach (var floorItems2 in predesigned.FloorItemData)
					using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
						dbClient.RunQuery("INSERT INTO items VALUES (null, " + Session.GetHabbo().Id + ", " + newRoom.RoomId + ", " + floorItems2.BaseItem + ", '" + floorItems2.ExtraData + "', " +
							floorItems2.X + ", " + floorItems2.Y + ", " + TextHandling.GetString(floorItems2.Z) + ", " + floorItems2.Rot + ", '', 0, 0, false);");
			#endregion

			#region CREATE WALL ITEMS
			if (predesigned.WallItems != null)
				foreach (var wallItems2 in predesigned.WallItemData)
					using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
						dbClient.RunQuery("INSERT INTO items VALUES (null, " + Session.GetHabbo().Id + ", " + newRoom.RoomId + ", " + wallItems2.BaseItem + ", '" + wallItems2.ExtraData +
							"', 0, 0, 0, 0, '" + wallItems2.WallCoord + "', 0, 0, false);");
			#endregion


			#region GENERATE ROOM AND SEND PACKET
			Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
			GalaxyServer.GetGame().GetRoomManager().LoadRoom(newRoom.Id).GetRoomItemHandler().LoadFurniture();
			var newFloorItems = newRoom.GetRoomItemHandler().GetFloor;
			foreach (var roomItem in newFloorItems) newRoom.GetRoomItemHandler().SetFloorItem(roomItem, roomItem.GetX, roomItem.GetY, roomItem.GetZ);
			var newWallItems = newRoom.GetRoomItemHandler().GetWall;
			foreach (var roomItem in newWallItems) newRoom.GetRoomItemHandler().SetWallItem(Session, roomItem);
			Session.SendMessage(new FlatCreatedComposer(newRoom.Id, newRoom.Name));
			#endregion

			using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				dbClient.RunQuery("DELETE FROM catalog_predesigned_rooms WHERE id = "+ idPredesigned+" LIMIT 1");

		}
	}
}