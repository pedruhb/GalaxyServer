using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Items;
using System;
using System.Data;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class EmptyItems : IChatCommand
    {
        public string PermissionRequired => "command_emptyitems";
        public string Parameters => "";
        public string Description => "Apaga todos os mobis do seu inventário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Params.Length == 1)
            {
                Session.SendWhisper("O comando apagará todos os mobis do seu inventário, incluindo raros e moedas, se você concorda com isso digite \":empty sim\".");
                return;
            }

            if (Params[1] != "sim")
            {
                return;
            }

    
            Session.GetHabbo().GetInventoryComponent().ClearItems();
            Session.SendMessage(new FurniListUpdateComposer());
            Session.SendNotification("Inventário vazio! Porém seus raros LTD não foram apagados, reentre no hotel ou compre algo para que eles apareçam novamente no inventário!");
        }
    }
}

