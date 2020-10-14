using Galaxy.Communication.Packets.Outgoing.Handshake;
using Galaxy.HabboHotel.Users;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class FlagMeCommand : IChatCommand
    {
        public string PermissionRequired => "command_flagme";
        public string Parameters => "";
        public string Description => "Mudar seu nome de usuario.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            
          /*  if (!this.CanChangeName(Session.GetHabbo()))
            {
                Session.SendWhisper("Parece que atualmente não tem a opçao para mudar seu nome de usuario!");
                return;
            } */

            Session.GetHabbo().ChangingName = true;
            Session.SendMessage(new UserObjectComposer(Session.GetHabbo()));
        }

        private bool CanChangeName(Habbo Habbo)
        {
            if (Habbo.Rank == 1 && Habbo.VIPRank == 0 && Habbo.LastNameChange == 0)
                return true;
            else if (Habbo.Rank == 1 && Habbo.VIPRank == 1 && (Habbo.LastNameChange == 0 || (GalaxyServer.GetUnixTimestamp() + 604800) > Habbo.LastNameChange))
                return true;
            else if (Habbo.Rank == 1 && Habbo.VIPRank == 2 && (Habbo.LastNameChange == 0 || (GalaxyServer.GetUnixTimestamp() + 86400) > Habbo.LastNameChange))
                return true;
            else if (Habbo.Rank == 1 && Habbo.VIPRank == 3)
                return true;
            else if (Habbo.GetPermissions().HasRight("mod_tool"))
                return true;

            return true;
        }
    }
}
