using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    internal class PollCommand : IChatCommand
    {
        public void Execute(GameClient Session, Room Room, string[] Params)
        {

            if (Params.Length == 0)
            {
                Session.SendWhisper("Por favor, apresente a pergunta");
            }
            else
            {

                string quest = CommandManager.MergeParams(Params, 1);
                if (quest == "end")
                {
                    Room.EndQuestion();
                }
                else
                {
                    Room.StartQuestion(quest);
                }

            }
        }

        public string Description =>
            "Faça uma busca imediata.";

        public string Parameters =>
            "[QUESTÃO]";

        public string PermissionRequired =>
            "command_poll";
    }
}