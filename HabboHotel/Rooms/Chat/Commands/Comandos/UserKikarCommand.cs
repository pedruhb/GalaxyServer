namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class UserKikarCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return ""; }
		}

		public string Parameters
		{
			get { return ""; }
		}

		public string Description
		{
			get { return "Ativa kikada no chão."; }
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{

			RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (User == null)
				return;

			if(User.CurrentEffect == 502)
				User.ApplyEffect(0); 
			else
				User.ApplyEffect(502);


		}
	}
}
