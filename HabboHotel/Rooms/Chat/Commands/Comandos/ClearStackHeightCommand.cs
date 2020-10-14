using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Rooms.Chat.Commands;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
     class ClearStackHeightCommand : IChatCommand
    {
        public string PermissionRequired => "command_clearsh";
        public string Parameters => "";
        public string Description => "Volta a altura pro estado original";

        public void Execute(GameClient session, Room room, string[] Params)
        {
            if (!room.CheckRights(session, true)) { session.SendWhisper("Você não pode usar isso nesse quarto!"); return; }
          
            session.GetHabbo().StackHeight = 0;
            session.SendWhisper("Altura redefinida para a padrão!");
        }
    }
}