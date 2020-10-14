using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Galaxy.Communication.Packets.Outgoing.Rooms.Furni.RentableSpaces;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Items.RentableSpaces;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Furni.RentableSpaces
{
    class CancelRentableSpaceEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {

            int itemId = Packet.PopInt();

            Room room;
            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out room))
                return;

            if (room == null || room.GetRoomItemHandler() == null)
                return;

            Item item = room.GetRoomItemHandler().GetItem(itemId);
            if (item == null)
                return;

            RentableSpaceItem _rentableSpace;
            if (!GalaxyServer.GetGame().GetRentableSpaceManager().GetRentableSpaceItem(itemId, out _rentableSpace))
                return;

            int errorCode = GalaxyServer.GetGame().GetRentableSpaceManager().GetCancelErrorCode(Session, _rentableSpace);

            if (errorCode > 0)
            {
                Session.SendMessage(new RentableSpaceComposer(_rentableSpace.IsRented(), errorCode, _rentableSpace.OwnerId, _rentableSpace.OwnerUsername, _rentableSpace.GetExpireSeconds(), _rentableSpace.Price));
                return;
            }


            if (!GalaxyServer.GetGame().GetRentableSpaceManager().ConfirmCancel(Session, _rentableSpace))
            {
                Session.SendNotification("global.error");
                return;
            }

            Session.SendMessage(new RentableSpaceComposer(false, 0, 0, "", 0, _rentableSpace.Price));
        }
    }
}