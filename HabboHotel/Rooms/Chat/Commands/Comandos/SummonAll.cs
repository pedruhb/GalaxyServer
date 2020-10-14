using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.HabboHotel.GameClients;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SummonAll : IChatCommand
    {
        public string PermissionRequired => "command_summonall";
        public string Parameters => "";
        public string Description => "Traz todos os usuários do Hotel.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            foreach (GameClient Client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
            {
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().Username == Session.GetHabbo().Username)
                    continue;

                Client.SendNotification("Você foi atraido por " + Session.GetHabbo().Username + "!");
                if (!Client.GetHabbo().InRoom)
                    Client.SendMessage(new RoomForwardComposer(Session.GetHabbo().CurrentRoomId));
                else if (Client.GetHabbo().InRoom)
                    Client.SendMessage(new RoomForwardComposer(Session.GetHabbo().CurrentRoomId));
            }

            Session.SendWhisper("Você acaba de atrair todos os usuários do Hotel!");

            }
        }
    }
