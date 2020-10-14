using System.Collections.Generic;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MassSignCommand : IChatCommand
    {
        public string PermissionRequired => "command_masssign";
        public string Parameters => "[ID]";
        public string Description => "Faz todos os usuários levantarem uma placa.";

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
                string SinalID = System.Convert.ToString(System.Convert.ToInt32(Params[1]));
                if (SinalID != null)
                    foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
                    {
                        RoomUser.SetStatus("sign", SinalID);
                        RoomUser.UpdateNeeded = true;
                        RoomUser.SignTime = GalaxyServer.GetUnixTimestamp() + 5;
                    }
                else Session.SendWhisper("ID inválido!");   
            }
        }
    }
}