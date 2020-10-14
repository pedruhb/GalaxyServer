using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SuperPushCommand : IChatCommand
    {
        public string PermissionRequired => "command_super_push";
        public string Parameters => "[USUARIO]"; 
        public string Description => "Empurre um usuário 3 casas a frente"; 

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, diga o nome do usuário que deseja empurrar..");
                return;
            }

            if (!Room.SPushEnabled && !Room.CheckRights(Session, true) && !Session.GetHabbo().GetPermissions().HasRight("room_override_custom_config"))
            {
                Session.SendWhisper("O dono do quarto desativou a possibilidade de usar esse comando aqui...");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Usuário não encontrado, o nick está correto?");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Talvez o usuário não esteja online...");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode fazer isso em sí mesmo...");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                Session.SendWhisper("Você não pode empurrar esse usuário...");
                return;
            }

            if (TargetUser.TeleportEnabled)
            {
                Session.SendWhisper("Esse usuário tem teletransporte ativado.");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (!((Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2)))
            {
                if (TargetUser.SetX - 1 == Room.GetGameMap().Model.DoorX || TargetUser.SetY - 1 == Room.GetGameMap().Model.DoorY)
                {
                    Session.SendWhisper("Não empurre esse usuário para fora do quarto  :(!");
                    return;
                }

                if (TargetUser.SetX - 2 == Room.GetGameMap().Model.DoorX || TargetUser.SetY - 2 == Room.GetGameMap().Model.DoorY)
                {
                    Session.SendWhisper("Não empurre esse usuário para fora do quarto :(!");
                    return;
                }

                if (TargetUser.SetX - 3 == Room.GetGameMap().Model.DoorX || TargetUser.SetY - 3 == Room.GetGameMap().Model.DoorY)
                {
                    Session.SendWhisper("Não empurre esse usuário para fora do quarto :(!");
                    return;
                }

                if (TargetUser.RotBody == 4)
                {
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y + 3);
                }

                if (ThisUser.RotBody == 0)
                {
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y - 3);
                }

                if (ThisUser.RotBody == 6)
                {
                    TargetUser.MoveTo(TargetUser.X - 3, TargetUser.Y);
                }

                if (ThisUser.RotBody == 2)
                {
                    TargetUser.MoveTo(TargetUser.X + 3, TargetUser.Y);
                }

                if (ThisUser.RotBody == 3)
                {
                    TargetUser.MoveTo(TargetUser.X + 3, TargetUser.Y);
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y + 3);
                }

                if (ThisUser.RotBody == 1)
                {
                    TargetUser.MoveTo(TargetUser.X + 3, TargetUser.Y);
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y - 3);
                }

                if (ThisUser.RotBody == 7)
                {
                    TargetUser.MoveTo(TargetUser.X - 3, TargetUser.Y);
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y - 3);
                }

                if (ThisUser.RotBody == 5)
                {
                    TargetUser.MoveTo(TargetUser.X - 3, TargetUser.Y);
                    TargetUser.MoveTo(TargetUser.X, TargetUser.Y + 3);
                }

                Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*empurrou com força o " + Params[1] + "*", 0, ThisUser.LastBubble));
            }
            else
            {
                Session.SendWhisper("Oops, " + Params[1] + " está longe.!");
            }
        }
    }
}