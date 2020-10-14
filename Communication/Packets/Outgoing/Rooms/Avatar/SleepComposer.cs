using Galaxy.HabboHotel.Rooms;

namespace Galaxy.Communication.Packets.Outgoing.Rooms.Avatar
{
    public class SleepComposer : ServerPacket
    {
        public SleepComposer(RoomUser User, bool IsSleeping)
            : base(ServerPacketHeader.SleepMessageComposer)
        {
			WriteInteger(User.VirtualId);
			WriteBoolean(IsSleeping);

            //// Wired AFK
            if(IsSleeping == true)
            {
            Room PHB = null;
            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(User.GetClient().GetHabbo().CurrentRoomId, out PHB))
                return;
            PHB.GetWired().TriggerEvent(HabboHotel.Items.Wired.WiredBoxType.TriggerUserAFKBox, User.GetClient().GetHabbo(), this); /// WIRED AFK PHB
            }
            ///////
			
			/// tira efeito aus
			if(IsSleeping == false && User.CurrentEffect == 517)
					User.ApplyEffect(0);

		}
    }
}