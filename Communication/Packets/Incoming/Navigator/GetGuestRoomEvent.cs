using Galaxy.Communication.Packets.Outgoing.Navigator;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms;
using System.Data;

namespace Galaxy.Communication.Packets.Incoming.Navigator
{
    class GetGuestRoomEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int roomID = Packet.PopInt();
			RoomData roomData = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(roomID);
            if (roomData == null)
                return;

            bool isLoading = Packet.PopInt() == 1;
            bool checkEntry = Packet.PopInt() == 1;

            //// Bloqueio de preso
            DataRow preso = null;
            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT Presidio FROM users WHERE id = '" + Session.GetHabbo().Id + "'");
                preso = dbClient.getRow();
            }

            if (System.Convert.ToBoolean(preso["Presidio"]) == true && roomID != GalaxyServer.Prisao)
            {
                Session.SendMessage(new RoomNotificationComposer("police_announcement", "message", "Você está preso e não pode ir para outros quartos."));
                return;
            }
            //////


            Session.SendMessage(new GetGuestRoomResultComposer(Session, roomData, isLoading, checkEntry));

		}
	}
}
