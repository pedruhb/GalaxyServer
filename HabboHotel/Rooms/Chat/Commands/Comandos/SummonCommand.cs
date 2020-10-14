using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SummonCommand : IChatCommand
    {
        public string PermissionRequired => "command_summon";
        public string Parameters => "[USUARIO]";
        public string Description => "Traga um usuário para a sala atual.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, introduza o nome do usuário.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Usuário não encontrado, talvez ele não esteja online.");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Usuário não encontrado, talvez ele não esteja online.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Sério isso? kkkk");
                return;
            }

            TargetClient.SendNotification("Você foi chamado por " + Session.GetHabbo().Username + "!");
            if (!TargetClient.GetHabbo().InRoom)
                TargetClient.SendMessage(new RoomForwardComposer(Session.GetHabbo().CurrentRoomId));
            else
                TargetClient.GetHabbo().PrepareRoom(Session.GetHabbo().CurrentRoomId, "");
        }
    }
}