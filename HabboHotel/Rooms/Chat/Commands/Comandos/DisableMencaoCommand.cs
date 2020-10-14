using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class DisableMencaoCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_eventosoff"; }
		}

		public string Parameters
		{
			get { return ""; }
		}

		public string Description
		{
			get { return "Ativar ou desativar menções na client."; }
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{
			if (Session.GetHabbo().StatusMencao == true)
			{
				using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.SetQuery("UPDATE `users` SET `status_mencao` = 'false' WHERE `id` = '" + Session.GetHabbo().Id + "'");
					dbClient.RunQuery();
				}
				Session.GetHabbo().StatusMencao = false;
				Session.SendWhisper("Você agora não permite receber alertas de menções.");
			}
			else

			if (Session.GetHabbo().StatusMencao == false)
			{
				using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.SetQuery("UPDATE `users` SET `status_mencao` = 'true' WHERE `id` = '" + Session.GetHabbo().Id + "'");
					dbClient.RunQuery();
				}
				Session.GetHabbo().StatusMencao = true;
				Session.SendWhisper("Você agora permite receber alertas de menções.");
			}

		}
	}
}