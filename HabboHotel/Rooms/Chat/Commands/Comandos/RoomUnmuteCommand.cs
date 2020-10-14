
using Galaxy.Core;
using System;
using System.Collections.Generic;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RoomUnmuteCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_room_unmute"; }
		}

		public string Parameters
		{
			get { return ""; }
		}

		public string Description
		{
			get { return "Desmutar a sala"; }
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (!Room.RoomMuted)
			{
				Session.SendWhisper("Este quarto não está mutado.");
				return;
			}

			Room.RoomMuted = false;

			List<RoomUser> RoomUsers = Room.GetRoomUserManager().GetRoomUsers();
			if (RoomUsers.Count > 0)
			{
				foreach (RoomUser User in RoomUsers)
				{
					if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null || User.GetClient().GetHabbo().Username == Session.GetHabbo().Username)
						continue;

					User.GetClient().SendWhisper("Esta sala foi desmutada.");
				}
			}
		}
	}
}