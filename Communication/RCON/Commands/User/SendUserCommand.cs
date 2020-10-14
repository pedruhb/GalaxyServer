
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms;
using System;
using System.Diagnostics.Eventing.Reader;

namespace Galaxy.Communication.RCON.Commands.User
{
    class SendUserCommand : IRCONCommand
    {
        public string Description
        {
            get { return "Este comando é usado para enviar um usuário para um quarto."; }
        }

        public string Parameters
        {
            get { return "%userId% %roomId%"; }
        }

        public bool TryExecute(string[] parameters)
        {
            
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            // Validate the message
            int RoomID = 0;
            if (!int.TryParse(parameters[1], out RoomID))
                return false;

            if (!GalaxyServer.GetGame().GetRoomManager().RoomExist(RoomID))
                return false;

            RoomData RoomData = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(RoomID);
            //TargetClient.SendNotification("Has sido enviado a la sala " + RoomData.Name + "!");
            client.SendMessage(RoomNotificationComposer.SendBubble("advice", "Você foi redirecionado para o quarto " + RoomData.Name + "!", ""));
            if (!client.GetHabbo().InRoom)
			{
                client.SendMessage(new RoomForwardComposer(RoomID));
                Console.WriteLine(client.GetHabbo().Username + " > " + RoomID);
                client.GetHabbo().PrepareRoom(RoomID, "");
            }
            else
                client.GetHabbo().PrepareRoom(RoomID, "");
            
            return true;
        }
    }
}