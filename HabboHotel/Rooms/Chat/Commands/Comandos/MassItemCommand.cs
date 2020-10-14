using System.Collections.Generic;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MassItemCommand : IChatCommand
    {
        public string PermissionRequired => "command_massitem";
        public string Parameters => "[ID]";
        public string Description => "Faz todos os usuários segurarem um item.";

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
                int Item = System.Convert.ToInt32(Params[1]);
                if (Item != null)
                    foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
                        RoomUser.CarryItem(Item);
                else
                        Session.SendWhisper("Item inválido.");
            }
        }
    }
}