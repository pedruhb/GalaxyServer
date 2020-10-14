using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class TakeUserBadgeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_takebadge"; }
        }

        public string Parameters
        {
            get { return "[USER] [EMBLEMA]"; }
        }

        public string Description
        {
            get { return "Remove o emblema de alguém"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 3)
            {
                GameClient TargetClient = null;
                TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
                if (TargetClient != null)
                    if (!TargetClient.GetHabbo().GetBadgeComponent().HasBadge(Params[2]))
                    {
                        {
                            Session.SendNotification("Este usuário não possui o emblema " + Params[2] + "");
                        }
                    }
                    else
                    {
                        RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                        TargetClient.GetHabbo().GetBadgeComponent().RemoveBadge(Params[2], TargetClient);
                        TargetClient.SendNotification("Seu emblema " + Params[2] + " foi removido!");
                        Session.SendNotification("O emblema foi removido!");

                    }
            }
            else
            {
                Session.SendNotification("Usuario não encontrado.");
                return;
            }
        }
    }
}
