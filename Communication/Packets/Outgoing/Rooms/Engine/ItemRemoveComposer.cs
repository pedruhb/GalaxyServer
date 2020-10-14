﻿

using Galaxy.HabboHotel.Items;

namespace Galaxy.Communication.Packets.Outgoing.Rooms.Engine
{
    class ItemRemoveComposer : ServerPacket
    {
        public ItemRemoveComposer(Item Item, int UserId)
            : base(ServerPacketHeader.ItemRemoveMessageComposer)
        {
            WriteString(Item.Id.ToString());
            WriteBoolean(false);
            WriteInteger(UserId);
        }
    }
}
