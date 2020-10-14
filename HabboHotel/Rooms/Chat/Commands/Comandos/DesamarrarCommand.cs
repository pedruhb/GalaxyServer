using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DesamarrarCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_desamarrar"; }
        }

        public string Parameters
        {
            get { return "[USUARIO]"; }
        }

        public string Description
        {
            get { return "Desamarre um jogador"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, digite o nome de usuário do usuário que deseja congelar.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Erro usuário não está online ou não está neste quarto.");
                return;
            }

            RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
            if (TargetUser != null)
            {

                TargetUser.Frozen = false; // Impede que o usuário se mova.
                TargetUser.ApplyEffect(0); // Aplica o efeito.
                if (TargetUser.isSitting == true)
                {
                    TargetUser.Z += 0.35;
                    TargetUser.Statusses.Remove("sit");
                    TargetUser.Statusses.Remove("1.0");
                    TargetUser.isSitting = false;
                }

                TargetUser.UpdateNeeded = true;

                Session.SendWhisper("Você soltou " + TargetClient.GetHabbo().Username + "!"); // Amarrado com sucesso!
                TargetClient.SendWhisper("Você foi solto!");
            }

        }
    }
}
