using Galaxy.Core;
using Galaxy.HabboHotel.Catalog.PredesignedRooms;
using Galaxy.HabboHotel.Rooms.Chat.Commands;
using System;
using System.Linq;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class AddPredesignedCommand : IChatCommand
    {
        public string PermissionRequired => "command_addpredesigned";
        public string Parameters => "";
        public string Description => "Coloca um pack de um quarto na loja!";

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

            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO catalog_predesigned_rooms(room_model,flooritems,wallitems,catalogitems,room_id,room_decoration) VALUES('" + Room.RoomData.ModelName +
                    "', '" + floorItemsData + "', '" + wallItemsData + "', '" + itemAmounts + "', " + Room.Id + ", '" + decoration + "');");
                var predesignedId = (uint)dbClient.InsertQuery();

                GalaxyServer.GetGame().GetCatalog().GetPredesignedRooms().predesignedRoom.Add(predesignedId,
                    new PredesignedRooms(predesignedId, (uint)Room.Id, Room.RoomData.ModelName,
                        floorItemsData.ToString().TrimEnd(';'), wallItemsData.ToString().TrimEnd(';'),
                        itemAmounts.ToString().TrimEnd(';'), decoration.ToString()));
            }

            Session.SendWhisper("O quarto foi colocado na tabela, porém o mesmo deve ser inserido manualmente no catálogo.");



        }
    }
}