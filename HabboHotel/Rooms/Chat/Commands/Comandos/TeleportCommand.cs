namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class TeleportCommand : IChatCommand
    {
        public string PermissionRequired => "command_teleport";
        public string Parameters => ""; 
        public string Description => "Ativar/Desativar teletransporte.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;
            Room.GetGameMap().GenerateMaps();
            User.TeleportEnabled = !User.TeleportEnabled;
            Room.GetGameMap().GenerateMaps();
            Room.GetGameMap().GenerateMaps();
        }
    }
}
