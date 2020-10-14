using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class FreezeCommand : IChatCommand
    {
        public string PermissionRequired => "command_freeze";
        public string Parameters => "[USUARIO]";
        public string Description => "Congele um usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {


            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, introduza o nome do usuário que deseja congelar...");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Erro ao encontrar usuário!.");
                return;
            }

            RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
            if (TargetUser != null)
                TargetUser.Frozen = true;

			Session.SendWhisper("Você congelou o usuário " + TargetClient.GetHabbo().Username + "!");
        }
    }
}
