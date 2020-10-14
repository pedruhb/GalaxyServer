using Galaxy.HabboHotel.Rooms;
using Galaxy.Communication.Packets.Outgoing.Rooms.Settings;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Settings
{
    class GetRoomSettingsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().Rank > 10 && Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            Room Room = GalaxyServer.GetGame().GetRoomManager().LoadRoom(Packet.PopInt());
            if (Room == null )
                return;

            if (!Room.CheckRights(Session, true))
                return;

            Session.SendMessage(new RoomSettingsDataComposer(Room));
        }
    }
}
