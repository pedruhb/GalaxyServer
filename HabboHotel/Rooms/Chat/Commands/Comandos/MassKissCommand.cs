using Galaxy.Communication.Packets.Outgoing.Rooms.Avatar;
using System.Collections.Generic;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MassKissCommand : IChatCommand
    {
        public string PermissionRequired => "command_masskiss";
        public string Parameters => "[ID]";
        public string Description => "Faz todos os usuários mandarem um beijo.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
            if (Users.Count > 0)
            {
                    foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
                        Room.SendMessage(new ActionComposer(RoomUser.VirtualId, 2));
            }
        }
    }
}