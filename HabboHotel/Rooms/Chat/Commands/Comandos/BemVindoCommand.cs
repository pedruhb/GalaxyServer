using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class BemVindoCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_bemvindo"; }
        }

        public string Parameters
        {
            get { return "[USUARIO]"; }
        }

        public string Description
        {
            get { return "Dê Boas Vindas a alguém."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Coloque o nome do Usuário!");
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if(ThisUser.GetClient().GetHabbo().Gender.ToLower() == "f")
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "Seja Bem-Vinda " + Params[1] + ", espero que se divirta e curta bastante o " + GalaxyServer.HotelName +" Hotel ƒ :)", 0, ThisUser.LastBubble));
            else
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "Seja Bem-Vindo " + Params[1] + ", espero que se divirta e curta bastante o " + GalaxyServer.HotelName + " Hotel ƒ :)", 0, ThisUser.LastBubble));

        }
    }
}


