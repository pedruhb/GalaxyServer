using Galaxy.HabboHotel.Rooms.Games.Teams;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class EnableCommand : IChatCommand
    {
        public string PermissionRequired => "command_enable";
        public string Parameters => "[ID]";
        public string Description => "Habilita um efeito!";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Você deve escrever um ID de efeito");
                return;
            }

            if (!Room.EnablesEnabled && !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                Session.SendWhisper("Parece que o proprietário do quarto desabilitou esse comando!");
                return;
            }

            RoomUser ThisUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
            if (ThisUser == null)
                return;

            if (ThisUser.RidingHorse)
            {
                Session.SendWhisper("Não pode ativar um efeito em cima do cavalo!");
                return;
            }
            else if (ThisUser.Team != TEAM.NONE)
                return;
            else if (ThisUser.isLying)
                return;

            int EffectId = 0;
            if (!int.TryParse(Params[1], out EffectId))
                return;

            if (EffectId > int.MaxValue || EffectId < int.MinValue)
                return;

            if(Session.GetHabbo().Rank < 6)
            if (EffectId == 178 || EffectId == 23 || EffectId == 24 || EffectId == 25 || EffectId == 26 || EffectId == 918 || EffectId == 548 || EffectId == 531 || EffectId == 102 || EffectId == 187 || EffectId == 593 || EffectId == 596 || EffectId == 598 || EffectId == 39 || EffectId == 38 || EffectId == 55 || EffectId == 56 || EffectId == 97)
            {
                Session.SendWhisper("Efeito desativado para seu rank!");
                return;
            }

			if (Session.GetHabbo().Rank != 17 && EffectId == 593)
			{
				Session.SendWhisper("Sentimos muito, você não pode utilizar o comando!");
				return;
			}

			if (GalaxyServer.Tipo == 0)
			{
				if(EffectId == 535 || EffectId == 492 || EffectId == 493 || EffectId == 491 || EffectId == 523 || EffectId == 596)
				{
					Session.SendWhisper("Sentimos muito, você não pode utilizar o comando!");
					return;
				}

                if (Session.GetHabbo().Rank < 10 && EffectId == 102)
                {
                    Session.SendWhisper("Sentimos muito, você não pode utilizar o comando!");
                    return;
                }

            }

			if (Session.GetHabbo().Rank < 4 && EffectId == 491)
			{
				Session.SendWhisper("Sentimos muito, você não pode utilizar o comando!");
				return;
			}

            if (Session.GetHabbo().Rank != 2 && EffectId == 529)
            {
                Session.SendWhisper("Sentimos muito, você não pode utilizar o comando!");
                return;

            }

            if (Session.GetHabbo().Rank < 16 && EffectId == 533 || Session.GetHabbo().Rank == 16 && EffectId == 918)
			{
				Session.SendWhisper("Sentimos muito, você não pode utilizar o comando!");
				return;
			}

			if (EffectId == 592 || EffectId == 595 || EffectId == 597 && Session.GetHabbo()._guidelevel < 1)
            {
                Session.SendWhisper("Sentimos muito, somente membros da equipe guia podem usar esse comando!");
                return;
            }

            if (EffectId == 594 && Session.GetHabbo()._croupier < 1)
            {
                Session.SendWhisper("Sentimos muito, somente membros da equipe Croupier podem usar esse comando!");
                return;
            }

            if (EffectId == 599 && Session.GetHabbo()._builder < 1)
            {
                Session.SendWhisper("Sentimos muito, somente membros da equipe arquiteto podem usar esse comando!");
                return;
            }

            if (EffectId == 44 && (Session.GetHabbo().Rank < 2))
            {
                Session.SendWhisper("Sentimos muito, somente membros VIP's podem usar esse comando!");
                return;
            }

            if (EffectId == 494 && (Session.GetHabbo().Rank < 7))
            {
                Session.SendWhisper("Sentimos muito, somente membros VIP's podem usar esse comando!");
                return;
            }



            Session.GetHabbo().Effects().ApplyEffect(EffectId);
        }
    }
}
