using System.Linq;
using System.Collections.Generic;

using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Items;
using Galaxy.Communication.Packets.Outgoing.Rooms.FloorPlan;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using System.Drawing;

namespace Galaxy.Communication.Packets.Incoming.Rooms.FloorPlan
{
    class FloorPlanEditorRoomPropertiesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            if (Session.GetHabbo().Rank > 9 && Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendNotification("Você não logou como staff!");
                return;
            }

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            if (Room.GetGameMap().Model == null)
                return;

            List<Point> Squares = new List<Point>();
            Room.GetRoomItemHandler().GetFloor.ToList().ForEach(Item =>
            {
                Item.GetCoords.ForEach(Point =>
                {
                    if (!Squares.Contains(Point))
                        Squares.Add(Point);
                });
            });

            Session.SendMessage(new FloorPlanFloorMapComposer(Squares));
            Session.SendMessage(new FloorPlanSendDoorComposer(Room.GetGameMap().Model.DoorX, Room.GetGameMap().Model.DoorY, Room.GetGameMap().Model.DoorOrientation));
            Session.SendMessage(new RoomVisualizationSettingsComposer(Room.WallThickness, Room.FloorThickness, GalaxyServer.EnumToBool(Room.Hidewall.ToString())));

            Squares.Clear();
            Squares = null;
        }
    }
}