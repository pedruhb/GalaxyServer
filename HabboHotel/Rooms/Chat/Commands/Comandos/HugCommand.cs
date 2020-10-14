using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class HugCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_abracar"; }
        }

        public string Parameters
        {
            get { return "[USUARIO]"; }
        }

        public string Description
        {
            get { return "Abraça outro usuário"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Escolha o usuário");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("O usuário não existe!");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Ocorreu um erro ao encontrar esse usuário, talvez eles não estejam on-line ou nesta sala.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Não pode ser você!");
                return;
            }

            if (TargetUser.TeleportEnabled)
            {
                Session.SendWhisper("Oops, esta pessoa desativou abraçar!");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (!((Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2)))
            {
                ThisUser.ApplyEffect(168);
                TargetUser.ApplyEffect(168);
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Abraçando " + TargetUser.GetUsername() + "*", 0, 16));
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Recebendo um abraço de " + Session.GetHabbo().Username + "*", 0, 16));
            }
            else
            {
                Session.SendWhisper("Oops, " + TargetUser.GetUsername() + " está muito longe!");
            }
        }
    }
}