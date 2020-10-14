
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DesmutarUsuario : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_unmute"; }
        }

        public string Parameters
        {
            get { return "[USER]"; }
        }

        public string Description
        {
            get { return "Desative um usuário atualmente silenciado."; }
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
                Session.SendWhisper("Digite o nome de usuário do usuário que deseja desmutar.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null || TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Ocorreu um erro ao encontrar esse usuário, talvez ele não esteja online.");
                return;
            }
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `users` SET `time_muted` = '0' WHERE `id` = '" + TargetClient.GetHabbo().Id + "' LIMIT 1");
            }
                 TargetClient.GetHabbo().TimeMuted = 0;
                 Room.MutedUsers.Remove(TargetClient.GetHabbo().Id);
            if (TargetClient.GetHabbo().TimeMuted == 0)
            {
                TargetClient.GetHabbo().TimeMuted = 0;
                TargetClient.SendNotification("Você foi desmutado por " + Session.GetHabbo().Username + "!");
                Session.SendWhisper("Você demutou o usuário " + TargetClient.GetHabbo().Username + "!");
            }


        }
    }
}