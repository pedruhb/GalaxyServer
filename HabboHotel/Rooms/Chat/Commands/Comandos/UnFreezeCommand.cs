using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class UnFreezeCommand : IChatCommand
    {
        public string PermissionRequired => "command_unfreeze";
        public string Parameters => "[USUARIO]";
        public string Description => "Descongele um usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, introduza o nome do usuário.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Usuário não encontrado, ele está online?");
                return;
            }

            RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
            if (TargetUser != null)
                TargetUser.Frozen = false;

            Session.SendWhisper("Você descongelou o usuário " + TargetClient.GetHabbo().Username + "!");
        }
    }
}
