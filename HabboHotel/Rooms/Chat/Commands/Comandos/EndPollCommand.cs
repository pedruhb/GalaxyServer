using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class EndPollCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_endpoll"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Termine a pesquisa na sala atual."; }
        }

        public void Execute(GameClients.GameClient Session, Galaxy.HabboHotel.Rooms.Room Room, string[] Params)
        {

            Room.EndQuestion();
            Session.SendMessage(new RoomNotificationComposer("game", 3, "A pesquisa foi terminada!", ""));
            return;
        }
    }
}
