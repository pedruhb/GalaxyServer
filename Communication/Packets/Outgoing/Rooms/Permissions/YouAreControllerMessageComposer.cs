﻿

namespace Galaxy.Communication.Packets.Outgoing.Rooms.Permissions
{
    class YouAreControllerComposer : ServerPacket
    {
        public YouAreControllerComposer(int Setting)
            : base(ServerPacketHeader.YouAreControllerMessageComposer)
        {
			WriteInteger(Setting);
        }
    }
}
