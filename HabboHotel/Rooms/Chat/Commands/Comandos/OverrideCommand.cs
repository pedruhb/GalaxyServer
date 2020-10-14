namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class OverrideCommand : IChatCommand
    {
        public string PermissionRequired => "command_override";
        public string Parameters => ""; 
        public string Description => "Caminhe sobre qualquer coisa.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.AllowOverride = !User.AllowOverride;

            if (User.AllowOverride)
                Session.SendWhisper("Override Ativado!");
            else
                Session.SendWhisper("Override Desativado!");
        }
    }
}
