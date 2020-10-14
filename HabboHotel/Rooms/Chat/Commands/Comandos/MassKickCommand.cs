using System.Collections.Generic;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class MassKickCommand : IChatCommand
	{
		public string PermissionRequired => "command_massitem";
		public string Parameters => "";
		public string Description => "Faz todos os usuários kikarem.";

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{
			if (Session.GetHabbo().isLoggedIn == false)
			{
				Session.SendWhisper("Você não logou como staff!");
				return;
			}


			List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
			if (Users.Count > 0)
			{
					foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
					{

					RoomUser.ApplyEffect(502);

					}
						
			}

		}
	}
}