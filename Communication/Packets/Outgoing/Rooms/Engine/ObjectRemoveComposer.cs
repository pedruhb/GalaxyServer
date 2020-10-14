﻿
using Galaxy.HabboHotel.Items;

namespace Galaxy.Communication.Packets.Outgoing.Rooms.Engine
{
	class ObjectRemoveComposer : ServerPacket
    {
        public ObjectRemoveComposer(Item Item, int UserId)
            : base(ServerPacketHeader.ObjectRemoveMessageComposer)
        {
			WriteString(Item.Id.ToString());
			WriteBoolean(false);
			WriteInteger(UserId);
			WriteInteger(0);
        }
    }
}