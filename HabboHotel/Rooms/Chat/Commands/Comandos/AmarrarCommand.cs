using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class AmarrarCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_amarrar"; }
        }

        public string Parameters
        {
            get { return "[USUARIO]"; }
        }

        public string Description
        {
            get { return "Amarrar um jogador"; }
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

                TargetUser.Frozen = true; // Impede que o usuário se mova.
                TargetUser.ApplyEffect(995); // Aplica o efeito.

                /* Começa SIT */
                if (!TargetUser.Statusses.ContainsKey("sit"))
                {
                    if ((TargetUser.RotBody % 2) == 0)
                    {
                        if (TargetUser == null)
                            return;

                        try
                        {
                            TargetUser.Statusses.Add("sit", "1.0");
                            TargetUser.Z -= 0.35;
                            TargetUser.isSitting = true;
                            TargetUser.UpdateNeeded = true;
                        }
                        catch { }
                    }
                    else
                    {
                        TargetUser.RotBody--;
                        TargetUser.Statusses.Add("sit", "1.0");
                        TargetUser.Z -= 0.35;
                        TargetUser.isSitting = true;
                        TargetUser.UpdateNeeded = true;
                    }
                }
                /* Termina SIT */
                Session.SendWhisper("Você amarrou " + TargetClient.GetHabbo().Username + "!"); // Amarrado com sucesso!
                TargetClient.SendWhisper("Você foi amarrado!");
            }

        }
    }
}
