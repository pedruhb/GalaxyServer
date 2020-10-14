﻿namespace Galaxy.Communication.Packets.Outgoing.Handshake
{
	class GenericErrorComposer : ServerPacket
    {
        public GenericErrorComposer(int errorId)
            : base(ServerPacketHeader.GenericErrorMessageComposer)
        {
			WriteInteger(errorId);
        }
    }
}
