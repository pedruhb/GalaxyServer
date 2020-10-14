using Galaxy.Communication.Packets.Outgoing.Rooms.Avatar;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Pathfinding;
using System;


namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class KissCommand : IChatCommand
    {
        public string PermissionRequired => "command_kiss";
        public string Parameters => "[USUARIO]";
        public string Description => "Beijar um usuário";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, insira o usuário!");
                return;
            }
            GameClient Target = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Sentimos muito, não encontramos este usuário!");
                return;
            }
            else
            {
                RoomUser TargetID = Room.GetRoomUserManager().GetRoomUserByHabbo(Target.GetHabbo().Id);
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (TargetID == null)
                {
                    Session.LogsNotif("Sentimos muito o usuário não esta na sala!", "command_notification");
                    return;
                }
                else if (Target.GetHabbo().Username == Session.GetHabbo().Username)
                {
                    Session.SendWhisper("Está carente? Não pode se beijar.");
                    return;
                }
                else if (TargetID.TeleportEnabled)
                {
                    Session.LogsNotif("Sentimos muito o usuário está com o teletransporte ativado!", "command_notification");
                    return;
                }
                else
                {
                    if (User != null)
                    {
                        if ((Math.Abs((int)(TargetID.X - User.X)) < 2) && (Math.Abs((int)(TargetID.Y - User.Y)) < 2))
                        {

                            int Rot = Rotation.Calculate(User.X, User.Y, TargetID.X, TargetID.Y); /// vira o usuário
                            User.SetRot(Rot, false);
                            User.UpdateNeeded = true;

                            int Rot2 = Rotation.Calculate(TargetID.X, TargetID.Y, User.X, User.Y); /// vira o usuário
                            TargetID.SetRot(Rot2, false);
                            TargetID.UpdateNeeded = true;

                            System.Threading.Thread.Sleep(600); /// delay

                            Room.SendMessage(new ActionComposer(User.VirtualId, 2));
                            Room.SendMessage(new ActionComposer(TargetID.VirtualId, 2));

                            Room.SendMessage(new ChatComposer(User.VirtualId, "*Beijando "+TargetID.GetUsername()+"*", 0, 16));
                        }
                        else
                        {
                            Session.SendWhisper("Ops! O usuário não está perto de você!");
                            return;
                        }
                    }
                }
            }
        }

    }
}