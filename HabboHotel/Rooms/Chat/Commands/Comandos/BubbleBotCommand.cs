using Galaxy.Core;
using Galaxy.Database.Interfaces;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class BubbleBotCommand : IChatCommand
    {
        public string PermissionRequired => "command_bubble_bot";
        public string Parameters => "[NOMEBOT] [BUBBLEID]";
        public string Description => "Mude o balão de fala de um BOT.";

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
                Session.SendWhisper("Oh, não se esqueça de inserir o nome do bot!");
                return;
            }

            if (Params.Length == 2)
            {
                Session.SendWhisper("Oh, esqueceu-se de introduzir um ID!");
                return;
            }
            string BotName = CommandManager.MergeParams(Params, 1);
            string Bubble = CommandManager.MergeParams(Params, 2);
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `bots` SET `chat_bubble` =  '" + Params[2] + "' WHERE `name` =  '" + Params[1] + "' AND  `room_id` =  '" + Session.GetHabbo().CurrentRoomId + "'");
                Session.LogsNotif("Você mudou a fala do bot: " + Params[1] + "!", "command_notification");
            }
        }
    }
}