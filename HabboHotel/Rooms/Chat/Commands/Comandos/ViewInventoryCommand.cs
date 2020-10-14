using System.Linq;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ViewInventoryCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_inv"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Visualiza inventário de outros usuários."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Session.GetHabbo().ViewInventory)
            {
                Session.SendMessage(new FurniListComposer(Session.GetHabbo().GetInventoryComponent().GetFloorItems().ToList(), Session.GetHabbo().GetInventoryComponent().GetWallItems()));
                Session.GetHabbo().ViewInventory = false;
                Session.SendWhisper("Inventário invertido.");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome de usuário que deseja ver o inventário.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Erro ao encontrar usuário, talvez ele não esteja online.");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Erro ao encontrar usuário, talvez ele não esteja online.");
                return;
            }

            Session.SendMessage(new FurniListComposer(TargetClient.GetHabbo().GetInventoryComponent().GetFloorItems().ToList(), TargetClient.GetHabbo().GetInventoryComponent().GetWallItems()));
            Session.GetHabbo().ViewInventory = true;

            Session.SendWhisper("Na próxima vez que você abrir o inventário, verá os items de " + TargetClient.GetHabbo().Username + "!");
        }
    }
}