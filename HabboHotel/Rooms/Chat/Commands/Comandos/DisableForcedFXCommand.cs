using Galaxy.Core;
using Galaxy.Database.Interfaces;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class DisableForcedFXCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_forced_effects"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Capacidade de ignorar ou permitir efeitos forçados."; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            Session.GetHabbo().DisableForcedEffects = !Session.GetHabbo().DisableForcedEffects;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `disable_forced_effects` = @DisableForcedEffects WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("DisableForcedEffects", (Session.GetHabbo().DisableForcedEffects == true ? 1 : 0).ToString());
                dbClient.RunQuery();
            }

            Session.SendWhisper("O modo de efeito foraçado agora é " + (Session.GetHabbo().DisableForcedEffects == true ? "desabilitado!" : "ativado!"));
        }
    }
}
