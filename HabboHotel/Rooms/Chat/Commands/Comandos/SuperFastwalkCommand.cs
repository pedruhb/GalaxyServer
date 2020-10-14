namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SuperFastwalkCommand : IChatCommand
    {
        public string PermissionRequired => "command_super_fastwalk";
        public string Parameters => "";
        public string Description => "Te da a capacidade de andar mt rápido.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.SuperFastWalking = !User.SuperFastWalking;

            if (User.FastWalking)
                User.FastWalking = false;

            if (User.SuperFastWalking)
                Session.SendWhisper("Caminhar Super Rapido Ativado!");
            else
                Session.SendWhisper("Caminhar Super Rapido Desativado!");
        }
    }
}
