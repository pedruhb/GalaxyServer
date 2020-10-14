using Galaxy.Communication.Packets.Outgoing.Rooms.Avatar;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Quests;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System.Threading.Tasks;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Avatar
{
    public class ActionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;
            int Efeito = Session.GetHabbo().Effects().CurrentEffect;
            int Action = Packet.PopInt();

            Room Room = null;
            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (User.DanceId > 0)
                User.DanceId = 0;

            if (Session.GetHabbo().Effects().CurrentEffect > 0)
                Room.SendMessage(new AvatarEffectComposer(User.VirtualId, 0));

            User.UnIdle();
            Room.SendMessage(new ActionComposer(User.VirtualId, Action));

            if (Action == 5) // idle
            {
				User.IsAsleep = true;
				Room.SendMessage(new SleepComposer(User, true));
			}

            GalaxyServer.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_WAVE);

            if(Efeito > 0 && User.IsAsleep == false)
            {
                VoltaOEfeito(Efeito, User, Action);
            }
        }
        private async Task VoltaOEfeito(int efeito, RoomUser User, int Action)
        {
            int Delay = -1;

            if (Action == 3)
                Delay = 2100;
            else if (Action == 1)
                Delay = 0;
            if (Delay != -1)
            {
            await Task.Delay(Delay);
            User.GetClient().GetHabbo().Effects().ApplyEffect(efeito);
            }
        }
    }
}