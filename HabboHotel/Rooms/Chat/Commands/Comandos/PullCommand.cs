using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class PullCommand : IChatCommand
    {
        public string PermissionRequired => "command_pull";
        public string Parameters => "[USUARIO]";
        public string Description => "Puxar um usuário até você.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nick de quem você quer puxar!");
                return;
            }

            if (!Room.PullEnabled && !Session.GetHabbo().GetPermissions().HasRight("room_override_custom_config"))
            {
                Session.SendWhisper("Ops! O proprietario do quarto desativou o comando.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocorreu um erro o usuário não se encontra online!");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Ocorreu um erro, parece que o usuário não está na sala e nem no quarto!");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Não se puxe, tá doidão?");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                Session.SendWhisper("Não pode puxar esse usuário.");
                return;
            }

            if (TargetUser.TeleportEnabled)
            {
                Session.SendWhisper("Esse usuário está com o modo teletransporte ativo!");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (ThisUser.SetX - 1 == Room.GetGameMap().Model.DoorX)
            {
                Session.SendWhisper("Por favor, não puxe esse usuário pra fora do quarto! :(");
                return;
            }


            string PushDirection = "down";
            if (TargetClient.GetHabbo().CurrentRoomId == Session.GetHabbo().CurrentRoomId && (Math.Abs(ThisUser.X - TargetUser.X) < 3 && Math.Abs(ThisUser.Y - TargetUser.Y) < 3))
            {
                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Puxei " + Params[1] + "*", 0, ThisUser.LastBubble));

                if (ThisUser.RotBody == 0)
                    PushDirection = "up";
                if (ThisUser.RotBody == 2)
                    PushDirection = "right";
                if (ThisUser.RotBody == 4)
                    PushDirection = "down";
                if (ThisUser.RotBody == 6)
                    PushDirection = "left";

                if (PushDirection == "up")
                    TargetUser.MoveTo(ThisUser.X, ThisUser.Y - 1);

                if (PushDirection == "right")
                    TargetUser.MoveTo(ThisUser.X + 1, ThisUser.Y);

                if (PushDirection == "down")
                    TargetUser.MoveTo(ThisUser.X, ThisUser.Y + 1);

                if (PushDirection == "left")
                    TargetUser.MoveTo(ThisUser.X - 1, ThisUser.Y);
                return;
            }
            else
            {
                Session.SendWhisper("Esse usuário não está perto o bastante para você puxar!");
                return;
            }
        }
    }
}