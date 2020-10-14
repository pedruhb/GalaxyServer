using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class GiveBadgeCommand : IChatCommand
    {
        public string PermissionRequired => "command_give_badge";
        public string Parameters => "[USUÁRIO] [CODIGO]";
        public string Description => "Dê um emblema a um usuário.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length != 3)
            {
                Session.SendWhisper("Digite um nome de usuário e um código de emblema que você gostaria de dar!");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                if (!TargetClient.GetHabbo().GetBadgeComponent().HasBadge(Params[2].ToUpper()))
                {
                    TargetClient.GetHabbo().GetBadgeComponent().GiveBadge(Params[2].ToUpper(), true, TargetClient);
                    if (TargetClient.GetHabbo().Id != Session.GetHabbo().Id)
                        TargetClient.SendMessage(RoomNotificationComposer.SendBubble("emblema/" + Params[2].ToUpper(), "Você acabou de receber um emblema!", "/inventory/open/badge"));
                    else
                        Session.SendMessage(RoomNotificationComposer.SendBubble("emblema/" + Params[2].ToUpper(), "Você acabou de dar o emblema: " + Params[2].ToUpper(), " /inventory/open/badge"));
                }
                else
                    Session.SendWhisper("Esse usuário já possui este emblema (" + Params[2].ToUpper() + ") !");
                return;
            }
            else
            {
                Session.SendWhisper("Nossa, não conseguimos encontrar o usuário!");
                return;
            }
        }
    }
}
