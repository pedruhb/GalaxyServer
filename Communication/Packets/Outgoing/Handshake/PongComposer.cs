﻿namespace Galaxy.Communication.Packets.Outgoing.Handshake
{
	class PongComposer :ServerPacket
    {
        public PongComposer()
            : base(ServerPacketHeader.PongMessageComposer)
        {

        }
    }
}
