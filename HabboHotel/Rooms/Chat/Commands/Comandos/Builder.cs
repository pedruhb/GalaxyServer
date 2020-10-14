using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class Builder : IChatCommand
    {
        public string PermissionRequired => "command_builder";
        public string Parameters => "";
        public string Description => "Teletransporte permite que o espaço para construir mais facilmente.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Você só pode usar o teletransporte em seu quarto.");
                return;

            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;
            
            User.TeleportEnabled = !User.TeleportEnabled;
            Room.GetGameMap().GenerateMaps();

           // Session.SendMessage(RoomNotificationComposer.SendBubble("builders_club_room_locked_small", "Acaba de ativar o modo construtor.", ""));
        }
    }
}
