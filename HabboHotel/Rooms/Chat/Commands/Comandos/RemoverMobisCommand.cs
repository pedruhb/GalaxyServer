using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Database.Interfaces;
using System.Data;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RemoverMobisCommand : IChatCommand
    {
        public string PermissionRequired => "command_removemobi";
        public string Parameters => "[ID]";
        public string Description => "Remove todos os mobis igual ao id indicado.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            string ApenasNumeros(string str)
            {
                var apenasDigitos = new System.Text.RegularExpressions.Regex(@"[^\d]");
                return apenasDigitos.Replace(str, "");
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Você deve inserir o id de um mobi!");
                Session.SendWhisper("Para achar o id basta clicar nele e ver perto do botão de mover/girar");
                Session.SendWhisper("Todos os mobis iguais ao mobi do id inserido vão ser removidos do quarto");
                return;
            }

            if (!Room.CheckRights(Session, true))
            return;

            DataRow BaseID = null;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `base_item` FROM items WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("id", ApenasNumeros(Params[1]));
                BaseID = dbClient.getRow();
            }

            int ItemBaseId = System.Convert.ToInt32(ApenasNumeros(System.Convert.ToString(BaseID["base_item"])));

            Room.GetRoomItemHandler().RemoveItemsCommand(Session, ItemBaseId);
            Room.GetGameMap().GenerateMaps();

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `items` SET `room_id` = '0' WHERE `room_id` = @RoomId AND `user_id` = @UserId and `base_item` = @BaseItem");
                dbClient.AddParameter("RoomId", Room.Id);
                dbClient.AddParameter("UserId", Session.GetHabbo().Id);
                dbClient.AddParameter("BaseItem", ItemBaseId);
                dbClient.RunQuery();
                Session.SendWhisper("Os mobis foram recolhidos.");
            }

            Session.SendMessage(new FurniListUpdateComposer());
        }
    }
}