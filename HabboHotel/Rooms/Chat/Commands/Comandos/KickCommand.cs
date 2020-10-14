using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class KickCommand : IChatCommand
    {
        public string PermissionRequired => "command_kick";
        public string Parameters => "[USUÁRIO] [MENSAGE]";
        public string Description => "Expulse o usuário e envie o motivo.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome do usuário que deseja kickar.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez eles não estejam online.");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez eles não estejam online.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode kick você mesmo!");
                return;
            }

            if (!TargetClient.GetHabbo().InRoom)
            {
                Session.SendWhisper("Este usuário não está atualmente em uma sala.");
                return;
            }

            Room TargetRoom;
            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(TargetClient.GetHabbo().CurrentRoomId, out TargetRoom))
                return;

            if (Params.Length > 2)
                TargetClient.SendNotification("Um moderador expulsou você da sala pelo seguinte motivo: " + CommandManager.MergeParams(Params, 2));
            else
                TargetClient.SendNotification("Um moderador expulsou você da sala.");

			// TargetRoom.GetRoomUserManager().RemoveUserFromRoom(TargetClient, true, false);

			TargetClient.GetHabbo().PrepareRoom(0, "");
		}
    }
}
