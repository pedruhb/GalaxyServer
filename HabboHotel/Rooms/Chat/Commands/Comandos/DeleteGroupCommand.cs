using Galaxy.Communication.Packets.Outgoing.Messenger;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DeleteGroupCommand : IChatCommand
    {
        public string PermissionRequired => "command_delete_group";
        public string Parameters => "";
        public string Description => "Apaga um grupo do hotel.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            if (Room.Group == null)
            {
                Session.SendWhisper("Bem, não há nenhum grupo aqui?");
                return;
            }

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("DELETE FROM `groups` WHERE `id` = '" + Room.Group.Id + "'");
                dbClient.runFastQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + Room.Group.Id + "'");
                dbClient.runFastQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + Room.Group.Id + "'");
                dbClient.runFastQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + Room.Group.Id + "' LIMIT 1");
                dbClient.runFastQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + Room.Group.Id + "' LIMIT 1");
                dbClient.runFastQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + Room.Group.Id + "'");
            }

            GalaxyServer.GetGame().GetGroupManager().DeleteGroup(Room.RoomData.Group.Id);

            Room.Group = null;
            Room.RoomData.Group = null;

            GalaxyServer.GetGame().GetRoomManager().UnloadRoom(Room);
            if (Room.RoomData.Group.HasChat)
            {
                var Client = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(Session.GetHabbo().Id);
                if (Client != null)
                {
                    Client.SendMessage(new FriendListUpdateComposer(Room.RoomData.Group, -1));
                    Client.SendMessage(new BroadcastMessageAlertComposer(GalaxyServer.GetGame().GetLanguageManager().TryGetValue("server.console.alert") + "\n\n Você deixou o grupo, por favor, se você ver o grupo de chat, no entanto, relogue no jogo."));
                }
            }

            var roomId = Session.GetHabbo().CurrentRoomId;
            List<RoomUser> UsersToReturn = new List<RoomUser>(Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUsers().ToList());

            RoomData Data = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(roomId);
            Session.GetHabbo().PrepareRoom(Session.GetHabbo().CurrentRoom.RoomId, "");
            GalaxyServer.GetGame().GetRoomManager().LoadRoom(roomId);

            foreach (RoomUser User in UsersToReturn)
            {
                if (User == null || User.GetClient() == null)
                    continue;

                User.GetClient().SendMessage(new RoomForwardComposer(roomId));
            }

            Session.SendNotification("Grupo eliminado.");
            return;
        }
    }
}
