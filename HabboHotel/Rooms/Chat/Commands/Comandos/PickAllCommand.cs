using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Items;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class PickAllCommand : IChatCommand
    {
        public string PermissionRequired => "command_pickall";
        public string Parameters => "";
        public string Description => "Remover todos mobis da sala";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {


            if (!Room.CheckRights(Session, true))
                return;

            Room.GetRoomItemHandler().RemoveItems(Session);
            Room.GetGameMap().GenerateMaps();

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `items` SET `room_id` = '0' WHERE `room_id` = @RoomId AND `user_id` = @UserId");
                dbClient.AddParameter("RoomId", Room.Id);
                dbClient.AddParameter("UserId", Session.GetHabbo().Id);
                dbClient.RunQuery();
                Session.LogsNotif("Todos mobis foram coletados", "command_notification");
            }

            List<Item> Items = Room.GetRoomItemHandler().GetWallAndFloor.ToList();
            if (Items.Count > 0)
                Session.SendWhisper("Ainda há mais elementos nesta sala, removidos manualmente ou usar: ejectall expulsá-los!");

            Session.SendMessage(new FurniListUpdateComposer());
        }
    }
}