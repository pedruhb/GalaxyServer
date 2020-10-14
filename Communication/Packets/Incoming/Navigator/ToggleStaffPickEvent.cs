using Galaxy.HabboHotel.Navigator;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms;
using Galaxy.Communication.Packets.Outgoing.Navigator;
using Galaxy.Communication.Packets.Outgoing.Rooms.Settings;

namespace Galaxy.Communication.Packets.Incoming.Navigator
{
    class ToggleStaffPickEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(session.GetHabbo().CurrentRoom.OwnerName);
            if (!session.GetHabbo().GetPermissions().HasRight("room.staff_picks.management"))
                return;

            if (session.GetHabbo().isLoggedIn == false)
            {
                session.SendNotification("Você não logou como staff!");
                return;
            }

            Room room = null;
            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(packet.PopInt(), out room))
                return;

            StaffPick staffPick = null;
            if (!GalaxyServer.GetGame().GetNavigator().TryGetStaffPickedRoom(room.Id, out staffPick))
            {
                if (GalaxyServer.GetGame().GetNavigator().TryAddStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("INSERT INTO `navigator_staff_picks` (`room_id`,`image`) VALUES (@roomId, null)");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                    GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(TargetClient, "ACH_Spr", 1, false);
                }
            }
            else
            {
                if (GalaxyServer.GetGame().GetNavigator().TryRemoveStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("DELETE FROM `navigator_staff_picks` WHERE `room_id` = @roomId LIMIT 1");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                }
            }

            room.SendMessage(new RoomSettingsSavedComposer(room.RoomId));
            room.SendMessage(new RoomInfoUpdatedComposer(room.RoomId));
        }
    }
}