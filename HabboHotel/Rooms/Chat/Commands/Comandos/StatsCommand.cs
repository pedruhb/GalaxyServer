using Galaxy.Core;
using System;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class StatsCommand : IChatCommand
    {
        public string PermissionRequired => "command_stats";
        public string Parameters => "";
        public string Description => "Ver as estatísticas atuais.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            double Minutes = Session.GetHabbo().GetStats().OnlineTime / 60;
            double Hours = Minutes / 60;
            int OnlineTime = Convert.ToInt32(Hours);
            string s = OnlineTime == 1 ? "" : "s";

            StringBuilder HabboInfo = new StringBuilder();
            HabboInfo.Append("Estatísticas da sua conta são:\r\r");
            HabboInfo.Append("Informações de moedas:\r");
            HabboInfo.Append(ExtraSettings.NomeMoedas+": " + Session.GetHabbo().Credits + "\r");
            HabboInfo.Append(ExtraSettings.NomeDuckets+": " + Session.GetHabbo().Duckets + "\r");
            HabboInfo.Append(ExtraSettings.NomeDiamantes + ": " + Session.GetHabbo().Diamonds + "\r");
            HabboInfo.Append("Tempo online: " + OnlineTime + " Hora" + s + "\r");
            HabboInfo.Append("Respeitos: " + Session.GetHabbo().GetStats().Respect + "\r");
            HabboInfo.Append(ExtraSettings.NomeGotw+": " + Session.GetHabbo().GOTWPoints + "\r\r");
            Session.SendNotification(HabboInfo.ToString());
        }
    }
}
