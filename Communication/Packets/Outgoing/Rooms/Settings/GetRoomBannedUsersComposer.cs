using System.Linq;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Cache.Type;

namespace Galaxy.Communication.Packets.Outgoing.Rooms.Settings
{
	class GetRoomBannedUsersComposer : ServerPacket
    {
        public GetRoomBannedUsersComposer(Room Instance)
            : base(ServerPacketHeader.GetRoomBannedUsersMessageComposer)
        {
			WriteInteger(Instance.Id);

			WriteInteger(Instance.GetBans().BannedUsers().Count);//Count
            foreach (int Id in Instance.GetBans().BannedUsers().ToList())
            {
                UserCache Data = GalaxyServer.GetGame().GetCacheManager().GenerateUser(Id);

                if (Data == null)
                {
					WriteInteger(0);
					WriteString("Unknown Error");
                }
                else
                {
					WriteInteger(Data.Id);
					WriteString(Data.Username);
                }
            }
        }
    }
}
