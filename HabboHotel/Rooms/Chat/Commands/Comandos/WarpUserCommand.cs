using System.Drawing;
using Galaxy.HabboHotel.Users;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class WarpUserCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_warpuser"; }
		}

		public string Parameters
		{
			get { return ""; }
		}

		public string Description
		{
			get { return "Traz um usuário do quarto para você."; }
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{

			if (Params.Length == 1)
			{
				Session.SendWhisper("Digite o nome do usuário que deseja trazer.");
				return;
			}


			Habbo Habbo = GalaxyServer.GetHabboByUsername(Params[1]);
			if (Habbo == null)
			{
				Session.SendWhisper("Ocorreu um erro ao procurar o usuário no banco de dados.");
				return;
			}

			if (Habbo == null || Habbo.GetClient() == null || Session.GetHabbo().Id == Habbo.Id)
			{
				Session.SendWhisper("Ocorreu um erro ao procurar o usuário.");
				return;
			}
				
				RoomUser SessionTarget = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (SessionTarget == null)
					return;

				RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Habbo.Id);
				if (TargetUser == null)
					return;


				TargetUser.Frozen = true;
				Room.SendMessage(Room.GetRoomItemHandler().UpdateUserOnRoller(TargetUser, new Point(SessionTarget.X, SessionTarget.Y), 0, SessionTarget.Z));

				if (TargetUser.Statusses.ContainsKey("sit"))
				TargetUser.Z -= 0.35;

				TargetUser.UpdateNeeded = true;
				Room.GetGameMap().GenerateMaps();
				TargetUser.Frozen = false;

		}
	}
}