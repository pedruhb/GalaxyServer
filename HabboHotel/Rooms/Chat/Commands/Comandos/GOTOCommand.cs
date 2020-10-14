using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class GOTOCommand : IChatCommand
    {
        public string PermissionRequired => "command_goto";
        public string Parameters => "[ID SALA]";
        public string Description => "Ir a uma sala.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Você deve especificar uma ID do quarto!");
                return;
            }

            int roomId = 0;
            if (!int.TryParse(Params[1], out roomId))
            {
                Session.SendWhisper("Você deve inserir uma ID de quarto válida");
            }
            else
            {
                Room room = null;
                if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(roomId, out room))
                {
                    Session.SendWhisper("Este quarto não existe!");
                    return;
                }

                Session.GetHabbo().PrepareRoom(room.Id, "");
            }
        }
    }
}