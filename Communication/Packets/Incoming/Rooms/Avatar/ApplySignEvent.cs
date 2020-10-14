using Galaxy.HabboHotel.Rooms;
using System;
using System.Threading.Tasks;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Avatar
{
    class ApplySignEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int SignId = Packet.PopInt();
            Room Room;

            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;


            User.UnIdle();

            User.SetStatus("sign", Convert.ToString(SignId));
            User.UpdateNeeded = true;
            User.SignTime = GalaxyServer.GetUnixTimestamp() + 5;
            RemoveSinal(User);

        }
        private async Task RemoveSinal(RoomUser User)
        {
                await Task.Delay(5000);
                User.Statusses.Remove("sign");
                User.UpdateNeeded = true;
        }
    }
}