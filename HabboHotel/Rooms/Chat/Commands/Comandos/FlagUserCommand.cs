using Galaxy.Communication.Packets.Outgoing.Handshake;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class FlagUserCommand : IChatCommand
    {
        public string PermissionRequired => "command_flaguser";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Renomeie um usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome de usuário que deseja alterar.");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez eles não estejam online.");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                Session.SendWhisper("O usuário não tem permissão para marcá-lo.");
                return;
            }
            else
            {
                TargetClient.GetHabbo().LastNameChange = 0;
                TargetClient.GetHabbo().ChangingName = true;
                TargetClient.SendWhisper("Você acaba de levar um flag! clique em você e altere seu nome.");
                TargetClient.SendMessage(new UserObjectComposer(TargetClient.GetHabbo()));
            }

        }
    }
}
