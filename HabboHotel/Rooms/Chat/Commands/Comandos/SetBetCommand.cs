using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SetBetCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_bet"; }
        }

        public string Parameters
        {
            get { return "[Quantidade]"; }
        }

        public string Description
        {
            get { return "Use para apostar nas máquinas de cassino."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (Params.Length == 1)
            {
                Session.SendWhisper("Você deve inserir um valor até 50!", 34);
                return;
            }

            if (Params[1].ToLower() == "info")
            {
                System.Text.StringBuilder List = new System.Text.StringBuilder("");
                List.AppendLine("<b>Como usar a máquina de apostas</b>");
                List.AppendLine("");
                List.AppendLine("Para você fazer uma aposta, primeiramente você deve usar o comando :apostar x para preparar os "+Core.ExtraSettings.NomeDiamantes.ToLower()+ " para a aposta, sendo x a quantidade de " + Core.ExtraSettings.NomeDiamantes.ToLower() + " a ser apostado.");
                List.AppendLine("A máquina funciona assim: irá ser gerado 3 emojis (ª, | e ¥), caso os 3 emojis forem iguais, você irá ganhar, veja abaixo os prêmios.");
                List.AppendLine("");
                List.AppendLine("ª - ª - ª = Você ganha o dobro do que foi apostado.");
                List.AppendLine("| - | - | = Você ganha o triplo do que foi apostado.");
                List.AppendLine("¥ - ¥ - ¥ = Você ganha o quádruplo do que foi apostado.");
                Session.SendNotification(List.ToString());
                return;
            }

            int Bet = 0;
            if (!int.TryParse(Params[1].ToString(), out Bet))
            {
                Session.SendWhisper("Por favor, digite um valor válido!", 34);
                return;
            }

			if(Bet < 1)
			{
				Session.SendWhisper("Por favor, digite um valor válido!", 34);
				return;
			}

			Session.GetHabbo()._bet = Bet;
            Session.SendWhisper("Agora você está apostando " + Bet + " " + Core.ExtraSettings.NomeDiamantes.ToLower() + "!", 34);
        }
    }
}
