using Galaxy.Database.Interfaces;
using System.Data;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class NumeroSorteCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_numerosorte"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Veja seu número da sorte"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
                  DataRow LoteriaPHB = null;
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT loteria FROM users WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1;");
                    dbClient.RunQuery();
                    LoteriaPHB = dbClient.getRow();
               }

            if (System.Convert.ToInt32(LoteriaPHB["loteria"]) == 0)
                Session.SendWhisper("Você não tem número da sorte! adquira um no catálogo.");
            else
                Session.SendWhisper("O seu número da sorte é \""+ LoteriaPHB["loteria"] + "\".");
        }
    }
}