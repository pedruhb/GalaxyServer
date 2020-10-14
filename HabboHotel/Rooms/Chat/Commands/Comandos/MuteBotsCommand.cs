using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MuteBotsCommand : IChatCommand
    {
        public string PermissionRequired => "command_mute_bots";
        public string Parameters => "";
        public string Description => "Ignorar ou permitir bate papo de bots";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowBotSpeech = !Session.GetHabbo().AllowBotSpeech;

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `users` SET `bots_muted` = '" + ((Session.GetHabbo().AllowBotSpeech) ? 1 : 0) + "' WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            if (Session.GetHabbo().AllowBotSpeech)
                Session.SendWhisper("Mudança bem sucedida, e você não pode ver o discurso de bots.");
            else
                Session.SendWhisper("Mudança bem sucedida, agora você pode ver o discurso de bots");
        }
    }
}
