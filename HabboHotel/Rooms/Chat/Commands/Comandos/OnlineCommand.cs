namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class OnlineCommand : IChatCommand
    {
        public string PermissionRequired => "command_onlines";
        public string Parameters => "";
        public string Description => "Exibe a quantidade de usuários onlines no hotel.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.SendWhisper("Temos agora "+ GalaxyServer.GetGame().GetClientManager().Count+" usuários onlines em "+GalaxyServer.GetGame().GetRoomManager().Count+" quartos.");
        }
    }
}