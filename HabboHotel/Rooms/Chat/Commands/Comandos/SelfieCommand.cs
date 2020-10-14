using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SelfieCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_selfie"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Tira uma selfie"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;
            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Pega o celular*", 0, ThisUser.LastBubble));
            Session.GetHabbo().Effects().ApplyEffect(65);
            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Tira uma selfie*", 0, ThisUser.LastBubble));
        }
    }
}
