namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class Freestyle : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_freestyle"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ativa e desativa o modo freestyle"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            ThisUser.Freestyle = !ThisUser.Freestyle;

            if (ThisUser.Freestyle)
            {
                Session.SendWhisper("Freestyle ativado!");
            }
            else
            {
                Session.SendWhisper("Freestyle desativado!");
            }
        }
    }
}