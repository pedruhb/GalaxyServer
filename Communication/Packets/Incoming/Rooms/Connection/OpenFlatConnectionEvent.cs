using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Connection
{
    public class OpenFlatConnectionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            int RoomId = Packet.PopInt();
            string Password = Packet.PopString();

			Session.GetHabbo().PrepareRoom(RoomId, Password);

		}
	}
}