using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class EmptyUser : IChatCommand
    {
        public string PermissionRequired => "command_emptyuser";
        public string Parameters => "[USUARIO]";
        public string Description => "Limpar o inventario de um usúario";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Escreva o nome do usúario que você deseja limpar.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("¡Oops! Provavelmente o usuário não está online.");
                return;
            }

            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendWhisper("Você não pode limpar o inventário desse usuário.");
                return;
            }

            TargetClient.GetHabbo().GetInventoryComponent().ClearItems();
            Session.SendWhisper("Inventário do usuário limpo com sucesso!");
        }
    }
}