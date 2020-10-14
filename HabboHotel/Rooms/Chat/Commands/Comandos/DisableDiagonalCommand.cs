namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DisableDiagonalCommand : IChatCommand
    {
        public string PermissionRequired => "command_disable_diagonal";
        public string Parameters => "";
        public string Description => " Desativar diagonal no seu quarto!";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {


			if (!Room.CheckRights(Session, true) && Session.GetHabbo().Rank < 8)
			{
				Session.SendWhisper("Bem, somente o proprietário da sala pode executar este comando!");
				return;
			}

			Room.GetGameMap().DiagonalEnabled = !Room.GetGameMap().DiagonalEnabled;
            Session.SendWhisper("Comando executado com Sucesso!.");
        }
    }
}
