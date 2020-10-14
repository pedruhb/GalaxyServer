using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RemoveWiredCommand : IChatCommand
    {
        public string PermissionRequired => "command_removewired";
        public string Parameters => "";
        public string Description => "Remove todos os wireds do quarto.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (!Room.CheckRights(Session, true))
                return;

            Room.GetRoomItemHandler().RemoveAllWireds(Session);
            Room.GetGameMap().GenerateMaps();
            Session.SendMessage(new FurniListUpdateComposer());
            Session.SendWhisper("Os wireds foram removidos.");
        }
    }
}