using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class CarCommand : IChatCommand
    {
        public string PermissionRequired => "command_carro";
        public string Parameters => "";
        public string Description => "Te da um carro!";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;
            Random random = new Random();
            int randomNumber = random.Next(17, 23);
            User.ApplyEffect(randomNumber);
            Session.SendWhisper("Vruuuuuuuuuuuuuuum!");
        }
    }
}