namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RegenMaps : IChatCommand
    {
        public string PermissionRequired => "command_regen_maps";
        public string Parameters => "";
        public string Description => "Regenerar o mapa da sala!";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {


            if (!Room.CheckRights(Session, true) && Session.GetHabbo().Rank < 8)
            {
                Session.SendWhisper("Bem, somente o proprietário da sala pode executar este comando!");
                return;
            }

            Room.GetGameMap().GenerateMaps();
            Session.SendWhisper("Mapa do jogo regenerado!");
        }
    }
}
