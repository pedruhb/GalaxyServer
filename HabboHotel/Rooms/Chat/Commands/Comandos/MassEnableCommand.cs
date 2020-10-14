using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MassEnableCommand : IChatCommand
    {
        public string PermissionRequired => "command_massenable";
        public string Parameters => "[EFEITOID]";
        public string Description => "Dâ um efeito para todos os usuários.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, informe o ID do efeito.");
                return;
            }

            int EnableId = 0;
            if (int.TryParse(Params[1], out EnableId))
            {
                if ((EnableId == 102 || EnableId == 187 || EnableId == 918) && !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                {
                    Session.SendWhisper("Você não pode aplicar esse efeito em outros usuários..");
                    return;
                }

                List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
                if (Users.Count > 0)
                {
                    foreach (RoomUser U in Users.ToList())
                    {
                        if (U == null || U.RidingHorse)
                            continue;

                        U.ApplyEffect(EnableId);

                    }
                }
            }
            else
            {
                Session.SendWhisper("Por favor, diga o id do efeito.");
                return;
            }
        }
    }
}
