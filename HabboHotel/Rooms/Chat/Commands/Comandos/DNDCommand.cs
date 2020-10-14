namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DNDCommand : IChatCommand
    {
        public string PermissionRequired => "command_dnd";
        public string Parameters => "";
        public string Description => "Ativar/Desativar Mensagens do Consele.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            
            Session.GetHabbo().AllowConsoleMessages = !Session.GetHabbo().AllowConsoleMessages;
            Session.SendWhisper("Você agora " + (Session.GetHabbo().AllowConsoleMessages == true ? "pode" : "não pode") + " receber mensagens no console.");
        }
    }
}