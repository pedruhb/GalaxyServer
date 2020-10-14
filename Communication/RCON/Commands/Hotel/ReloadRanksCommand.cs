using System.Linq;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadRanksCommand : IRCONCommand
    {
        public string Description => "Atualizar Ranks";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            GalaxyServer.GetGame().GetPermissionManager().Init();

            foreach (GameClient client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null || client.GetHabbo().GetPermissions() == null)
                    continue;

                client.GetHabbo().GetPermissions().Init(client.GetHabbo());
            }
            
            return true;
        }
    }
}