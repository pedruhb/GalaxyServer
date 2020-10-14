using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Users;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MuteCommand : IChatCommand
    {
        public string PermissionRequired => "command_mute";
        public string Parameters => "[USUÁRIO] [TEMPO]";
        public string Description => "Silenciar o usuário por um tempo.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite um nome de usuário e um tempo válido em segundos (máximo 600, nada mais é reiniciado para 600).");
                return;
            }

            Habbo Habbo = GalaxyServer.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário no banco de dados.");
                return;
            }

            if (Habbo.Username == "PHB" || Habbo.Rank > Session.GetHabbo().Rank || Habbo.GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_mute_any"))
            {
                Session.SendWhisper("Uau, você não pode silenciar esse usuário.");
                return;
            }

			if (double.TryParse(Params[2], out double Time))
			{
               
				if (Time > 600 && !Session.GetHabbo().GetPermissions().HasRight("mod_mute_limit_override"))
					Time = 600;

                int tempo = GalaxyServer.GetIUnixTimestamp() + Convert.ToInt32(Time);


				using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.runFastQuery("UPDATE `users` SET `time_muted` = '" + tempo + "' WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
				}

				if (Habbo.GetClient() != null)
				{
					Habbo.TimeMuted = tempo;
					Habbo.GetClient().SendNotification("Você foi silenciado por " + Time + " segundos!");
				}

				Session.SendWhisper("Você mutou o usuário " + Habbo.Username + " por " + Time + " segundos.");
			}
			else
				Session.SendWhisper("Insira um número inteiro válido.");
		}
    }
}