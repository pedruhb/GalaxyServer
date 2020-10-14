using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class roomSelllCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return "[VALOR]"; }
        }

        public string Description
        {
            get { return "Permite você vender seu quarto por um valor pré-determinado [Ex. 1000c OR 200d]"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room CurrentRoom, string[] Params)
        {
            RoomUser User = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

            if (User == null)
                return;



            if (Params.Length == 1 || Params[1].Length < 2)
            {
                Session.SendWhisper("Por favor escreva o valor da venda [Ex. 145000c OR 75000d]", 3);
                return;
            }

            string ActualInput = Params[1];

            if (CurrentRoom.RoomData.OwnerId != User.HabboId)
            {
                Session.SendWhisper("Você precisa ser o dono do quarto!", 3);
                return;
            }
            string roomCostType = ActualInput.Substring(ActualInput.Length - 1);
            int roomCost;
            try
            {
                roomCost = int.Parse(ActualInput.Substring(0, ActualInput.Length - 1));
            }
            catch
            {
                User.GetClient().SendWhisper("Você precisa escrever um preço válido para o quarto", 3);
                return;
            }

            if (roomCost == 0)
            {
                CurrentRoom.RoomData.roomForSale = false;
                CurrentRoom.RoomData.roomSaleCost = 0;
                CurrentRoom.RoomData.roomSaleType = "";
                Session.SendWhisper("Se o quarto for vendido, ficará indiponível", 3);
                User.RoomOfferPending = false;
                User.RoomOffer = "";
                return;
            }

            if (roomCost < 1 || roomCost > 10000000)
            {
                User.GetClient().SendWhisper("Preço do quarto inválido, Ele é muito baixo ou muito alto", 3);
                return;
            }

            if (ActualInput.EndsWith("c") || ActualInput.EndsWith("d"))
            {
                CurrentRoom.RoomData.roomForSale = true;
                CurrentRoom.RoomData.roomSaleCost = roomCost;
                CurrentRoom.RoomData.roomSaleType = roomCostType;
            }
            else
            {
                Session.SendWhisper("Preço inválido [Ex. 600c OR 400d]", 3);
                return;
            }

            foreach (RoomUser UserInRoom in CurrentRoom.GetRoomUserManager().GetRoomUsers().ToList())
            {
                if (UserInRoom != null)
                {
                    UserInRoom.GetClient().SendWhisper("Este quarto está a venda por " + roomCost + roomCostType + " diga :comprarquarto " + roomCost + roomCostType + " para aquiri-lo.", 5);
                }
            }


        }
    }
}