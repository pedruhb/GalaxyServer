using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SendUserCommand : IChatCommand
    {
        public string PermissionRequired => "command_senduser";
        public string Parameters => "[USUARIO] [SALAID]";
        public string Description => "Manda um usuário para uma sala";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 2)
            {
                Session.SendWhisper("Por favor, Introduza o nome do usuário e o ID do quarto.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Não encontramos o usuário online.");
                return;
            }
            
            
            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("O usuário não foi encontrado, talvez ele esteja offline.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode fazer isso em você mesmo.");
                return;
            }

            int RoomID;
            if (!int.TryParse(Params[2], out RoomID))
            {
                Session.SendWhisper("Ocorreu um erro enquanto a pesquisa do usuário não pode existir ... Lembre-se de usar apenas números para a sala.");
                return;
            }

            if (!GalaxyServer.GetGame().GetRoomManager().RoomExist(RoomID))
            {
                Session.SendWhisper("Essa sala não existe.");
                return;

            }
            RoomData RoomData = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(RoomID);
            //TargetClient.SendNotification("Has sido enviado a la sala " + RoomData.Name + "!");
            TargetClient.SendMessage(RoomNotificationComposer.SendBubble("advice", "Você foi enviado para a sala " + RoomData.Name + "!", ""));
            if (!TargetClient.GetHabbo().InRoom)
                TargetClient.SendMessage(new RoomForwardComposer(RoomID));
            else
                TargetClient.GetHabbo().PrepareRoom(RoomID, "");
        }
    }
}