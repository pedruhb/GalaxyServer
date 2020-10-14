using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Quests;
using Galaxy.Communication.Packets.Outgoing.Pets;
using Galaxy.Communication.Packets.Outgoing.Rooms.Avatar;

namespace Galaxy.Communication.Packets.Incoming.Rooms.AI.Pets
{
    class RespectPetEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom || Session.GetHabbo().GetStats() == null || Session.GetHabbo().GetStats().DailyPetRespectPoints == 0)
                return;

            Room Room;

            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            int PetId = Packet.PopInt();

            RoomUser Pet = null;
            if (!Session.GetHabbo().CurrentRoom.GetRoomUserManager().TryGetPet(PetId, out Pet)) 
            {
                //Okay so, we've established we have no pets in this room by this virtual Id, let us check out users, maybe they're creeping as a pet?!
                RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(PetId);
                if (TargetUser == null)
                    return;

                //Check some values first, please!
                if (TargetUser.GetClient() == null || TargetUser.GetClient().GetHabbo() == null)
                    return;

                if (TargetUser.GetClient().GetHabbo().Id == Session.GetHabbo().Id)
                {
                    Session.SendWhisper("Você não pode usar isso em si mesmo!");
                    return;
                }

                //And boom! Let us send some respect points.
                GalaxyServer.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_RESPECT);
                GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_RespectGiven", 1);
                GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(TargetUser.GetClient(), "ACH_RespectEarned", 1);
                //Take away from pet respect points, just in-case users abuse this..
                Session.GetHabbo().GetStats().DailyPetRespectPoints -= 1;
                Session.GetHabbo().GetStats().RespectGiven += 1;
                TargetUser.GetClient().GetHabbo().GetStats().Respect += 1;

                //Apply the effect.
                ThisUser.CarryItemID = 999999999;
                ThisUser.CarryTimer = 5;

                //Send the magic out.
                if (Room.RespectNotificationsEnabled)
                    Room.SendMessage(new RespectPetNotificationMessageComposer(TargetUser.GetClient().GetHabbo(), TargetUser));
                Room.SendMessage(new CarryObjectComposer(ThisUser.VirtualId, ThisUser.CarryItemID));
                return;
            }

            if (Pet == null || Pet.PetData == null || Pet.RoomId != Session.GetHabbo().CurrentRoomId)
                return;

            Session.GetHabbo().GetStats().DailyPetRespectPoints -= 1;
            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetRespectGiver", 1, false);

            ThisUser.CarryItemID = 999999999;
            ThisUser.CarryTimer = 5;
            Pet.PetData.OnRespect();
            Room.SendMessage(new CarryObjectComposer(ThisUser.VirtualId, ThisUser.CarryItemID));
        }
    }
}