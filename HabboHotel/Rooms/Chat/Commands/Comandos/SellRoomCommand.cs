using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SellRoomCommand : IChatCommand
    {
        public string Description => "Coloque à venda o quarto em que você está.";
        public string Parameters => "[PREÇO]";
        public string PermissionRequired => "";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Coloque um preço xD");
                return;
            }

            if (!Room.CheckRights(Session, true))
                return;

            if (Room == null)

                if (Params.Length == 1)
                {
                    Session.SendWhisper("Oops, esqueceu de escolher um preço para vender este quarto.");
                    return;
                }
                else if (Room.Group != null)
                {
                    Session.SendWhisper("Ops, aparentemente este quarto tem um grupo, então você não pode vender, primeiro você deve excluir o grupo.");
                    return;
                }

            int Value = 0;
            if (!int.TryParse(Params[1], out Value))
            {
                Session.SendWhisper("Ops, você está inserindo um valor que não está correto");
                return;
            }

            if (Value < 0)
            {
                Session.SendWhisper("Você não pode vender um quarto com um valor numérico negativo.");
                return;
            }

            if (Room.ForSale)
            {
                Room.SalePrice = Value;
            }
            else
            {
                Room.ForSale = true;
                Room.SalePrice = Value;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers())
            {
                if (User == null || User.GetClient() == null)
                    continue;

                Session.SendWhisper("Este quarto está à venda, o preço atual é:  " + Value + " Duckets! Compre-o digitando :buyroom.");
            }

            Session.SendNotification("Se você quiser vender seu quarto, você deve incluir um valor numérico. \n\nPOR FAVOR NOTA:\nSe você vende um quarto, não pode recuperá-lo novamente!\n\n" +
        "Você pode cancelar a venda de um quarto digitando ':unload' (sem as '')");

            return;
        }

    }
}