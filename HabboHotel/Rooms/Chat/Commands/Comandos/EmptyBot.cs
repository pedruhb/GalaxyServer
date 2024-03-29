﻿using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Items;
using System;
using System.Data;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class EmptyBot : IChatCommand
	{
		public string PermissionRequired => "command_emptyitems";
		public string Parameters => "";
		public string Description => "Apaga todos os bots do seu inventário.";

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{

			if (Params.Length == 1)
			{
				Session.SendWhisper("O comando apagará todos os bots do seu inventário, se você concorda com isso digite \":emptybots sim\".");
				return;
			}

			if (Params[1] != "sim")
			{	
				Session.SendWhisper("O comando apagará todos os bots do seu inventário, se você concorda com isso digite \":emptybots sim\".");
				return;
			}

			Session.GetHabbo().GetInventoryComponent().ClearBots();
			Session.SendMessage(new FurniListUpdateComposer());
			Session.SendWhisper("Os bots foram apagados!.");

		}
	}
}

