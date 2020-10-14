namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class FastwalkCommand : IChatCommand
    {
        public string PermissionRequired => "command_fastwalk";
        public string Parameters => "";
        public string Description => "Faz você correr rápido.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.FastWalking = !User.FastWalking;

            if (User.SuperFastWalking)
                User.SuperFastWalking = false;

            if (User.FastWalking)
                Session.SendWhisper("Ativado!");
            else
                Session.SendWhisper("Desativado!");
        }
    }
}
