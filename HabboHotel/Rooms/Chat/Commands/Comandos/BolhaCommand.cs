namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class BolhaCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_bolha_staff"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ativa/desativa efeito de bolha"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Session.GetHabbo().bolhastaff == false)
            {
                Session.GetHabbo().bolhastaff = true;
                Session.SendWhisper("Bolha ativada!");

				if (Session.GetHabbo().Rank == 13)
					Session.GetHabbo().Effects().ApplyEffect(535);

				if (Session.GetHabbo().Rank == 14)
					Session.GetHabbo().Effects().ApplyEffect(492);

				if (Session.GetHabbo().Rank == 15)
					Session.GetHabbo().Effects().ApplyEffect(493);

				if (Session.GetHabbo().Rank == 4)
					Session.GetHabbo().Effects().ApplyEffect(491);

				if (Session.GetHabbo().Id == 5387)
					Session.GetHabbo().Effects().ApplyEffect(523);

				if (Session.GetHabbo().Id == 15)
					Session.GetHabbo().Effects().ApplyEffect(596);
			}
            else
            {
                Session.GetHabbo().bolhastaff = false;
                Session.SendWhisper("Bolha desativada até reentrar!");
                Session.GetHabbo().Effects().ApplyEffect(0);
            }

        }
    }
}
