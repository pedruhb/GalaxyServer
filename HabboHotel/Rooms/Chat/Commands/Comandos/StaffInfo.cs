using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class StaffInfo : IChatCommand
    {
        public string PermissionRequired => "command_staffinfo";
        public string Parameters => "";
        public string Description => "Ver lista de staffs online";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {

            Dictionary<Habbo, UInt32> clients = new Dictionary<Habbo, UInt32>();

            StringBuilder content = new StringBuilder();
            content.Append("Status da Equipe " + GalaxyServer.HotelName + ":\r\n");

            foreach (var client in GalaxyServer.GetGame().GetClientManager()._clients.Values)
            {
                if (client != null && client.GetHabbo() != null && client.GetHabbo().Rank > 3)
                    clients.Add(client.GetHabbo(), (Convert.ToUInt16(client.GetHabbo().Rank)));
            }

            foreach (KeyValuePair<Habbo, UInt32> client in clients.OrderBy(key => key.Value))
            {
                if (client.Key == null)
                    continue;

                content.Append("¥ " + client.Key.Username + " [Rank: " + client.Key.Rank + "] » Está na sala: " + ((client.Key.CurrentRoom == null) ? "em nenhuma sala." : client.Key.CurrentRoom.RoomData.Name) + "\r\n");
            }

            Session.SendMessage(new MOTDNotificationComposer(content.ToString()));
            return;
        }
    }
}