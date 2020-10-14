using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms.Chat.Styles;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class BubbleCommand : IChatCommand
    {
        public string PermissionRequired => "command_bubble";
        public string Parameters => "[BUBBLEID]";
        public string Description => "Mude sua bolha de chat.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (Params.Length == 1)
            {
                Session.SendWhisper("Oh, esqueceu-se de introduzir um ID!");
                return;
            }

            int Bubble = 0;
            if (!int.TryParse(Params[1].ToString(), out Bubble))
            {
                Session.SendWhisper("Por favor ultilize um número valido.");
                return;
            }

            if ((Bubble == 33) && !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                Session.LogsNotif("Desculpe, apenas os membros da equipe podem usar essas falas", "command_notification");
                return;
            }

            ChatStyle Style = null;
            if (!GalaxyServer.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Bubble, out Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().GetPermissions().HasRight(Style.RequiredRight)))
            {
                Session.SendWhisper("Bem, você não pode usar esta fala por causa do seu cargo, sorry!");
                return;
            }

            User.LastBubble = Bubble;
            Session.GetHabbo().CustomBubbleId = Bubble;
            Session.SendWhisper("Você alterou sua bolha para a " + Bubble);
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `users` SET `bubble_id` = '" + Bubble + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }
        }
    }
}