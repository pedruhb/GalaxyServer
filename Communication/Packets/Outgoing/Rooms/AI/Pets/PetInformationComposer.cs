using System;
using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.Rooms.AI;
using Galaxy.HabboHotel.Rooms;

namespace Galaxy.Communication.Packets.Outgoing.Rooms.AI.Pets
{
    class PetInformationComposer : ServerPacket
    {
        public PetInformationComposer(Pet Pet, bool isMounted = false)
            : base(ServerPacketHeader.PetInformationMessageComposer)
        {
            Room Room;

            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(Pet.RoomId, out Room))
                return;

			WriteInteger(Pet.PetId);
			WriteString(Pet.Name);
			WriteInteger(Pet.Level);
			WriteInteger(Pet.MaxLevel);
			WriteInteger(Pet.experience);
            WriteInteger(Pet.experienceGoal);
            WriteInteger(Pet.Energy);
            WriteInteger(Pet.MaxEnergy);
            WriteInteger(Pet.Nutrition);
            WriteInteger(Pet.MaxNutrition);
            WriteInteger(Pet.Respect);
            WriteInteger(Pet.OwnerId);
            WriteInteger(Pet.Age);
            WriteString(Pet.OwnerName);
            WriteInteger(1);//3 on hab
            if(Pet.Type == 15)
            WriteBoolean(true);
            else
            WriteBoolean(false);
            WriteBoolean(isMounted);
            WriteInteger(0);//5 on hab
            WriteInteger(Pet.AnyoneCanRide); // Anyone can ride horse
            WriteInteger(0);
            WriteInteger(0);//512 on hab
            WriteInteger(0);//1536
            WriteInteger(0);//2560
            WriteInteger(0);//3584
            WriteInteger(0);
            WriteString("");
            WriteBoolean(false);
            WriteInteger(-1);//255 on hab
            WriteInteger(-1);
            WriteInteger(-1);
            WriteBoolean(false);
        }

        public PetInformationComposer(Habbo Habbo)
            : base(ServerPacketHeader.PetInformationMessageComposer)
        {
            WriteInteger(Habbo.Id);
            WriteString(Habbo.Username);
            WriteInteger(Habbo.Rank);
            WriteInteger(10);
            WriteInteger(0);
            WriteInteger(0);
            WriteInteger(100);
            WriteInteger(100);
            WriteInteger(100);
            WriteInteger(100);
            WriteInteger(Habbo.GetStats().Respect);
            WriteInteger(Habbo.Id);
            WriteInteger(Convert.ToInt32(Math.Floor((GalaxyServer.GetUnixTimestamp() - Habbo.AccountCreated) / 86400)));//How?
            WriteString(Habbo.Username);
            WriteInteger(1);//3 on hab
            WriteBoolean(false);
            WriteBoolean(false);
            WriteInteger(0);//5 on hab
            WriteInteger(0); // Anyone can ride horse
            WriteInteger(0);
            WriteInteger(0);//512 on hab
            WriteInteger(0);//1536
            WriteInteger(0);//2560
            WriteInteger(0);//3584
            WriteInteger(0);
            WriteString("");
            WriteBoolean(false);
            WriteInteger(-1);//255 on hab
            WriteInteger(-1);
            WriteInteger(-1);
            WriteBoolean(false);
        }
    }
}
