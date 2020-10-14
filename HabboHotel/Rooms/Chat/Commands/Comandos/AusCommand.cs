namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class AusenteCommand : IChatCommand
    {
        public string PermissionRequired => "command_aus";
        public string Parameters => "";
        public string Description => "Te deixa ausente.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if(User.CurrentEffect == 517)
                User.ApplyEffect(0);
            else 
                 User.ApplyEffect(517);

        }
    }
}