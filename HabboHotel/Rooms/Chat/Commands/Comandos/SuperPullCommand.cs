using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SuperPullCommand : IChatCommand
    {
        public string PermissionRequired => "command_super_pull";
        public string Parameters => "[USUARIO]";
        public string Description => "Puxe outro usuário para sua frente";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {


            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, introduza o nome do usuário que deseja puxar...");
                return;
            }

            if (!Room.SPullEnabled && !Room.CheckRights(Session, true) && !Session.GetHabbo().GetPermissions().HasRight("room_override_custom_config"))
            {
                Session.SendWhisper("O dono do quarto desativou o comando aqui...");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Usuário não encontrado, talvez ele não esteja online.");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Usuário não encontrado, talvez ele não esteja online.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode se empurrar!");
                return;
            }

            if (TargetUser.TeleportEnabled)
            {
                Session.SendWhisper("O usuário está com o teletransporte ativado...");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (ThisUser.SetX - 1 == Room.GetGameMap().Model.DoorX)
            {
                Session.SendWhisper("Você não pode remover o usuário da sala :(!");
                return;
            }

            if (ThisUser.RotBody % 2 != 0)
                ThisUser.RotBody--;
            if (ThisUser.RotBody == 0)
                TargetUser.MoveTo(ThisUser.X, ThisUser.Y - 1);
            else if (ThisUser.RotBody == 2)
                TargetUser.MoveTo(ThisUser.X + 1, ThisUser.Y);
            else if (ThisUser.RotBody == 4)
                TargetUser.MoveTo(ThisUser.X, ThisUser.Y + 1);
            else if (ThisUser.RotBody == 6)
                TargetUser.MoveTo(ThisUser.X - 1, ThisUser.Y);

            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*puxou " + Params[1] + " para ele*", 0, ThisUser.LastBubble));
            return;
        }
    }
}