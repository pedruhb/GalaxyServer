using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MassBadgeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_mass_badge"; }
        }

        public string Parameters
        {
            get { return "[CODIGO]"; }
        }

        public string Description
        {
            get { return "Dâ emblema a todos os usuários online."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o código do emblema que deseja dar ao hotel inteiro.");
                return;
            }

            foreach (GameClient Client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
            {
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().Username == Session.GetHabbo().Username)
                    continue;

                if (!Client.GetHabbo().GetBadgeComponent().HasBadge(Params[1]))
                {
                    Client.GetHabbo().GetBadgeComponent().GiveBadge(Params[1], true, Client);
                    Client.SendWhisper("Você recebeu um emblema!");
                }
               
            }

            Session.SendWhisper("Você deu com êxito a cada usuário neste hotel o emblema " + Params[1] + "!");
        }
    }
}
