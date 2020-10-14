using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ViewOnlineCommand : IChatCommand
    {
        public string PermissionRequired => "command_view_online";
        public string Parameters => "";
        public string Description => "Veja usuários online.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            Dictionary<Habbo, UInt32> clients = new Dictionary<Habbo, UInt32>();

            StringBuilder content = new StringBuilder();
            content.Append("- LISTA DE USUÁRIOS ONLINE -\r\n");

            foreach (var client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
            {
                if (client == null)
                    continue;

                content.Append("¥ " + client.GetHabbo().Username + " » Está no quarto: " + ((client.GetHabbo().CurrentRoom == null) ? "Em qualquer sala." : client.GetHabbo().CurrentRoom.RoomData.Name) + "\r\n");
            }

            Session.SendMessage(new MOTDNotificationComposer(content.ToString()));
            return;
        }
    }
}
