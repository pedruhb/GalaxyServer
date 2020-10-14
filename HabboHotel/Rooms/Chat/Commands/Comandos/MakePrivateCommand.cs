using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MakePrivateCommand : IChatCommand
    {
        public string PermissionRequired => "command_make_public";
        public string Parameters => "";
        public string Description => "Converta esta sala em uma privada.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            var room = Session.GetHabbo().CurrentRoom;
            using (var queryReactor = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                queryReactor.runFastQuery(string.Format("UPDATE rooms SET roomtype = 'private' WHERE id = {0}",
                    room.RoomId));

            var roomId = Session.GetHabbo().CurrentRoom.RoomId;
            var users = new List<RoomUser>(Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUsers().ToList());

            GalaxyServer.GetGame().GetRoomManager().UnloadRoom(Session.GetHabbo().CurrentRoom);

            RoomData Data = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(roomId);
            Session.GetHabbo().PrepareRoom(roomId, "");

            GalaxyServer.GetGame().GetRoomManager().LoadRoom(roomId);

            var data = new RoomForwardComposer(roomId);

            foreach (var user in users.Where(user => user != null && user.GetClient() != null))
                user.GetClient().SendMessage(data);
        }
    }
}
