using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Galaxy.HabboHotel.Rooms;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RoomBadgeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_room_badge"; }
        }

        public string Parameters
        {
            get { return "[CODIGO]"; }
        }

        public string Description
        {
            get { return "Da emblema a todos do quarto!"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, insira o nome do emblema que gostaria de dar ao quarto.");
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                    continue;

                if (!User.GetClient().GetHabbo().GetBadgeComponent().HasBadge(Params[1]))
                {
                    User.GetClient().GetHabbo().GetBadgeComponent().GiveBadge(Params[1], true, User.GetClient());
                    User.GetClient().SendNotification("Você recebeu um emblema!");
                }
                else
                    User.GetClient().SendWhisper(Session.GetHabbo().Username + " tentou te dar um emblema, porém você já tem ele.");
            }

            Session.SendWhisper("Você deu o emblema " + Params[1] + " para todos do quarto!");
        }
    }
}
