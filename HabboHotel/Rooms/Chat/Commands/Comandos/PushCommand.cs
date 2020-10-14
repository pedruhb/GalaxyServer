using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class PushCommand : IChatCommand
    {
        public string PermissionRequired => "command_push";
        public string Parameters => "[USUARIO]";
        public string Description => "Empurar um usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nick de quem você quer puxar!");
                return;
            }

            if (!Room.PushEnabled && !Session.GetHabbo().GetPermissions().HasRight("room_override_custom_config"))
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
                Session.SendWhisper("Você não quer fazer isso, né?");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                Session.SendWhisper("Não pode empurrar esse usuário.");
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

            if (!((Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2)))
            {
                if (TargetUser.SetX - 1 == Room.GetGameMap().Model.DoorX)
                {
                    Session.SendWhisper("Não faça esse usuário sair do quarto :(!");
                    return;
                }

                if (TargetUser.RotBody == 4)
                {
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y + 1);
                }

                if (ThisUser.RotBody == 0)
                {
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y - 1);
                }

                if (ThisUser.RotBody == 6)
                {
                    TargetUser.MoveTo(TargetUser.X - 1, TargetUser.Y);
                }

                if (ThisUser.RotBody == 2)
                {
                    TargetUser.MoveTo(TargetUser.X + 1, TargetUser.Y);
                }

                if (ThisUser.RotBody == 3)
                {
                    TargetUser.MoveTo(TargetUser.X + 1, TargetUser.Y);
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y + 1);
                }

                if (ThisUser.RotBody == 1)
                {
                    TargetUser.MoveTo(TargetUser.X + 1, TargetUser.Y);
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y - 1);
                }

                if (ThisUser.RotBody == 7)
                {
                    TargetUser.MoveTo(TargetUser.X - 1, TargetUser.Y);
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y - 1);
                }

                if (ThisUser.RotBody == 5)
                {
                    TargetUser.MoveTo(TargetUser.X - 1, TargetUser.Y);
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y + 1);
                }

                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*empurrou " + Params[1] + "*", 0, ThisUser.LastBubble));
            }
            else
            {
                Session.SendWhisper("Ops, " + Params[1] + " está longe demais!");
            }
        }
    }
}