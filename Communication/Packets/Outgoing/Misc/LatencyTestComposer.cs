using System;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Outgoing.Misc
{
	class LatencyTestComposer : ServerPacket
    {
        public LatencyTestComposer(GameClient Session, int testResponce)
            : base(ServerPacketHeader.LatencyResponseMessageComposer)
        {
            if (Session == null)
                return;

            Session.TimePingedReceived = DateTime.Now;

			WriteInteger(testResponce);
            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_AllTimeHotelPresence", 1);
        }
    }
}
