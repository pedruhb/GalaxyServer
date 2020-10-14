using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class CarryCommand : IChatCommand
    {
        public string PermissionRequired => "command_carry";
        public string Parameters => "[ID]";
        public string Description => "Ganhe um item de mão.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {

            int ItemId = 0;
            if (!int.TryParse(Convert.ToString(Params[1]), out ItemId))
            {
                Session.SendWhisper("Por favor, introduza um número válido.");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.CarryItem(ItemId);
        }
    }
}
