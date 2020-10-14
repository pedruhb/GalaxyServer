using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Pathfinding;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.Rooms.Chat.Commands;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DiscoCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_disco"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Easter Egg"; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {

			if (Session.GetHabbo().Rank < 5 && Session.GetHabbo().Id != Room.OwnerId)
			{
				Session.SendWhisper("Somente o dono pode ativar o disco.");
				return;
			}

			if (Room != null || !Room.CheckRights(Session))
            {

                  Session.SendWhisper("Você precisa de um toner para usar esse comando.");
                  Room.DiscoMode = !Room.DiscoMode;
            }
        }
    }
}