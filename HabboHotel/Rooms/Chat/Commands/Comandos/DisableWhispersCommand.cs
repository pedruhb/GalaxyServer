namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DisableWhispersCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_disable_whispers"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Permite voce ativar ou desativar a capacidade de sussurros."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            
            Session.GetHabbo().ReceiveWhispers = !Session.GetHabbo().ReceiveWhispers;
            Session.SendWhisper("Você agora " + (Session.GetHabbo().ReceiveWhispers ? "está" : "não está") + " recebendo sussurro!");
        }
    }
}
