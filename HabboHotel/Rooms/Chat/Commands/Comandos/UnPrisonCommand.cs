using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class UnPrisonCommand : IChatCommand
    {

        public string PermissionRequired
        {
            get { return "command_desprender"; }
        }

        public string Parameters
        {
            get { return "[USUARIO]"; }
        }

        public string Description
        {
            get { return "Desprender um usuário"; ; }
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
                Session.SendWhisper("Escolha um jogador para remover da cadeia");
                return;
            }

            GameClient TargetClient = null;
            TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);

            if (TargetClient.GetHabbo().Id == Session.GetHabbo().Id)
            {
                Session.SendWhisper("Você não pode ser prender!");
            }

            if (TargetClient.GetHabbo().Username == null)
            {
                Session.SendWhisper("O usuário não existe!");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_soft_ban") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Você não pode prender este usuário!");
                return;
            }



            if (TargetClient.GetHabbo().Id != Session.GetHabbo().Id )
            {
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE users SET Presidio = 'false' WHERE id = " + TargetClient.GetHabbo().Id + ";");

                    Session.SendWhisper("Você removeu o usuário " + TargetClient.GetHabbo().Username + " da prisão!");
                    TargetClient.GetHabbo().PrepareRoom(TargetClient.GetHabbo().HomeRoom, "");
                    string figure = TargetClient.GetHabbo().Look;
                    GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, "O Usuário " + Params[1] + " saiu da prisão!", ""));
                }
            }
            else
            {
                Session.SendWhisper("Você não pode usar este comando!");
            }
        }
    }
}