using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SetStackHeightCommand : IChatCommand
    {
        public string PermissionRequired => "command_setsh";
        public string Parameters => "[ALTURA]";
        public string Description => "Faz a função da lajota.";

        public void Execute(GameClient session, Room room, string[] Params)
        {
            if (Params.Length == 1)
                return;

            if (!room.CheckRights(session, true)) { session.SendWhisper("Você não pode usar isso nesse quarto!"); return; }

                if (!double.TryParse(Params[1], out session.GetHabbo().StackHeight)) { 
                session.SendWhisper("Digite um valor inteiro ou duplo válido.");
				} 
				else 
   {
				if (Convert.ToInt32(Params[1]) > 100 || Convert.ToInt32(Params[1]) < 0)
				{
					session.SendWhisper("Digite algo entre 0 e 100.");
				}
				else
				{
					session.GetHabbo().StackHeight = Convert.ToInt32(Params[1]);
					session.SendWhisper("Altura alterada para " + Params[1] + "");
				}
            }
        }
    }
}