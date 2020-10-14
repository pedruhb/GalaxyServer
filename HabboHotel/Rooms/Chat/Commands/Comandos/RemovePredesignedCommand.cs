using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RemovePredesignedCommand : IChatCommand
    {
        public string PermissionRequired => "command_update";
        public string Parameters => "";
        public string Description => "Tira um quarto de pack do hotel!";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Room == null) return;
            var predesignedId = 0U;
            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT id FROM catalog_predesigned_rooms WHERE room_id = " + Room.Id + ";");
                predesignedId = (uint)dbClient.getInteger();

                dbClient.RunQuery("DELETE FROM catalog_predesigned_rooms WHERE room_id = " + Room.Id + " AND id = " +
                    predesignedId + ";");
            }

            GalaxyServer.GetGame().GetCatalog().GetPredesignedRooms().predesignedRoom.Remove(predesignedId);
            Session.SendWhisper("Você tiro a sala dos pack do hotel!");
        }
    }
}