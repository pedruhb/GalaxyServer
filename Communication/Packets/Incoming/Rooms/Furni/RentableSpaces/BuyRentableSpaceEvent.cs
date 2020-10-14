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
    class BuyRentableSpaceEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {

            int itemId = Packet.PopInt();

            Room room;
            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out room))
                return;

            if (room == null || room.GetRoomItemHandler() == null)
                return;

            RentableSpaceItem rsi;
            if (GalaxyServer.GetGame().GetRentableSpaceManager().GetRentableSpaceItem(itemId, out rsi))
            {
                GalaxyServer.GetGame().GetRentableSpaceManager().ConfirmBuy(Session, rsi, 3600);
            }


        }
    }
}