using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class GiveRoom : IChatCommand
    {
        public string PermissionRequired => "command_give_room";
        public string Parameters => "[QUANTIDADE]";
        public string Description => "Dar " + ExtraSettings.NomeMoedas + " a todos.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite a quantidade que você gostaria de dar à sala.");
                return;
            }
			if (int.TryParse(Params[1], out int Amount))

				foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
				{
					if (RoomUser == null || RoomUser.GetClient() == null || Session.GetHabbo().Id == RoomUser.UserId)
						continue;
					RoomUser.GetClient().GetHabbo().Credits += Amount;
					RoomUser.GetClient().SendMessage(new CreditBalanceComposer(RoomUser.GetClient().GetHabbo().Credits));
				}
         
                GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Amount + " " + ExtraSettings.NomeMoedas + " para um quarto!"));
            
        }
}
}
  