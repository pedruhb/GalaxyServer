/*using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Items.Utilities;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Rooms.AI;
using Galaxy.HabboHotel.Rooms.AI.Responses;
using Galaxy.HabboHotel.Rooms.AI.Speech;
using System;
using System.Collections.Generic;

namespace Galaxy.Communication.Packets.Incoming.Catalog
{
    class CheckGnomeNameEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            int ItemId = Packet.PopInt();
            Item Item = Room.GetRoomItemHandler().GetItem(ItemId);
            if (Item == null || Item.Data == null || Item.UserID != Session.GetHabbo().Id || Item.Data.InteractionType != InteractionType.GNOME_BOX)
                return;

            string PetName = Packet.PopString();
            if (!GalaxyServer.IsValidAlphaNumeric(PetName))
            {
                Session.SendMessage(new CheckGnomeNameComposer(PetName, 1));
                return;
            }

            int X = Item.GetX;
            int Y = Item.GetY;

            //Quickly delete it from the database.
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("DELETE FROM `items` WHERE `id` = @ItemId LIMIT 1");
                dbClient.AddParameter("ItemId", Item.Id);
                dbClient.RunQuery();
            }

            //Remove the item.
            Room.GetRoomItemHandler().RemoveFurniture(Session, Item.Id);

            //Apparently we need this for success.
            Session.SendMessage(new CheckGnomeNameComposer(PetName, 0));

            //Create the pet here.
            Pet Pet = PetUtility.CreatePet(Session.GetHabbo().Id, PetName, 26, "30", "ffffff");
            if (Pet == null)
            {
                Session.SendNotification("Ops! Ocorreu um erro, porfavor informe isso!");
                return;
            }

            List<RandomSpeech> RndSpeechList = new List<RandomSpeech>();
            List<BotResponse> BotResponse = new List<BotResponse>();

            Pet.RoomId = Session.GetHabbo().CurrentRoomId;
            Pet.GnomeClothing = RandomClothing();

            //Update the pets gnome clothing.
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `bots_petdata` SET `gnome_clothing` = @GnomeClothing WHERE `id` = @PetId LIMIT 1");
                dbClient.AddParameter("GnomeClothing", Pet.GnomeClothing);
                dbClient.AddParameter("PetId", Pet.PetId);
                dbClient.RunQuery();
            }

            //Make a RoomUser of the pet.
            RoomUser PetUser = Room.GetRoomUserManager().DeployBot(new RoomBot(Pet.PetId, Pet.RoomId, "pet", "freeroam", Pet.Name, "", Pet.Look, X, Y, 0, 0, 0, 0, 0, 0, ref RndSpeechList, "", 0, Pet.OwnerId, false, 0, false, 0), Pet);

            //Give the food.
            ItemData PetFood = null;
            if (GalaxyServer.GetGame().GetItemManager().GetItem(320, out PetFood))
            {
                Item Food = ItemFactory.CreateSingleItemNullable(PetFood, Session.GetHabbo(), "", "");
                if (Food != null)
                {
                    Session.GetHabbo().GetInventoryComponent().TryAddItem(Food);
                    Session.SendMessage(new FurniListNotificationComposer(Food.Id, 1));
                }
            }
        }

        private static string RandomClothing()
        {
            Random Random = new Random();

            int RandomNumber = Random.Next(1, 6);
            switch (RandomNumber)
            {
                default:
                case 1:
                    return "5 0 -1 0 4 402 5 3 301 4 1 101 2 2 201 3";
                case 2:
                    return "5 0 -1 0 1 102 13 3 301 4 4 401 5 2 201 3";
                case 3:
                    return "5 1 102 8 2 201 16 4 401 9 3 303 4 0 -1 6";
                case 4:
                    return "5 0 -1 0 3 303 4 4 401 5 1 101 2 2 201 3";
                case 5:
                    return "5 3 302 4 2 201 11 1 102 12 0 -1 28 4 401 24";
                case 6:
                    return "5 4 402 5 3 302 21 0 -1 7 1 101 12 2 201 17";
            }
        }
    }
}*/

//using System;
//using System.Linq;
//using System.Text;
//using System.Collections.Generic;

//using Raven.HabboHotel.Rooms;
//using Raven.HabboHotel.Items;
//using Raven.HabboHotel.Rooms.AI;
//using Raven.HabboHotel.Rooms.AI.Speech;
//using Raven.HabboHotel.Items.Utilities;



//using Raven.Communication.Packets.Outgoing.Catalog;
//using Raven.Communication.Packets.Outgoing.Inventory.Furni;

//using Raven.Database.Interfaces;
//using Raven.HabboHotel.Rooms.AI.Responses;

//namespace Raven.Communication.Packets.Incoming.Catalog
//{
//    class CheckGnomeNameEvent : IPacketEvent
//    {
//        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
//        {
//            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
//                return;

//            Room Room = Session.GetHabbo().CurrentRoom;
//            if (Room == null)
//                return;

//            int ItemId = Packet.PopInt();
//            Item Item = Room.GetRoomItemHandler().GetItem(ItemId);
//            if (Item == null || Item.Data == null || Item.UserID != Session.GetHabbo().Id || Item.Data.InteractionType != InteractionType.GNOME_BOX)
//                return;

//            string PetName = Packet.PopString();
//            if (string.IsNullOrEmpty(PetName))
//            {
//                Session.SendMessage(new CheckGnomeNameComposer(PetName, 1));
//                return;
//            }

//            int X = Item.GetX;
//            int Y = Item.GetY;

//            //Quickly delete it from the database.
//            using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
//            {
//                dbClient.SetQuery("DELETE FROM `items` WHERE `id` = @ItemId LIMIT 1");
//                dbClient.AddParameter("ItemId", Item.Id);
//                dbClient.RunQuery();
//            }

//            //Remove the item.
//            Room.GetRoomItemHandler().RemoveFurniture(Session, Item.Id);

//            //Apparently we need this for success.
//            Session.SendMessage(new CheckGnomeNameComposer(PetName, 0));

//            int PetRace = Item.GetBaseItem().ClothingId;

//            string Race = RandonColor();
//            //Create the pet here.
//            Pet Pet = PetUtility.CreatePet(Session.GetHabbo().Id, PetName, PetRace, Race, "ffffff");
//            if (Pet == null)
//            {
//                Session.SendNotification("Oops, ocurrio un error, reporte esto en Atencion al Cliente");
//                return;
//            }

//            List<RandomSpeech> RndSpeechList = new List<RandomSpeech>();
//            List<BotResponse> BotResponse = new List<BotResponse>();

//            Pet.RoomId = Session.GetHabbo().CurrentRoomId;
//            Pet.GnomeClothing = RandomClothing();
//            Pet.Race = RandonColor();
//            if (PetRace == 26)
//            {
//                //Update the pets gnome clothing.
//                using (IQueryAdapter dbClient = RavenEnvironment.GetDatabaseManager().GetQueryReactor())
//                {
//                    dbClient.SetQuery("UPDATE `bots_petdata` SET `gnome_clothing` = @GnomeClothing WHERE `id` = @PetId LIMIT 1");
//                    dbClient.AddParameter("GnomeClothing", Pet.GnomeClothing);
//                    dbClient.AddParameter("PetId", Pet.PetId);
//                    dbClient.RunQuery();
//                }
//            }

//            Pet.DBState = DatabaseUpdateState.NeedsUpdate;
//            Room.GetRoomUserManager().UpdatePets();

//            //Make a RoomUser of the pet.
//            //RoomUser PetUser = Room.GetRoomUserManager().DeployBot(new RoomBot(Pet.PetId, Pet.RoomId, "pet", "freeroam", Pet.Name, "", Pet.Look, X, Y, 0, 0, 0, 0, 0, 0, ref RndSpeechList, "", 0, Pet.OwnerId, false, 0, false, 0), Pet);
//            RoomBot RoomBot = new RoomBot(Pet.PetId, Pet.RoomId, "pet", "freeroam", Pet.Name, "", Pet.Look, X, Y, 0, 0, 0, 0, 0, 0, ref RndSpeechList, "", 0, Pet.OwnerId, false, 0, false, 0);
//            if (RoomBot == null)
//                return;

//            Room.GetRoomUserManager().DeployBot(RoomBot, Pet);

//            Pet.DBState = DatabaseUpdateState.NeedsUpdate;
//            Room.GetRoomUserManager().UpdatePets();

//            Pet ToRemove = null;
//            //Give the food.
//            ItemData PetFood = null;
//            if (RavenEnvironment.GetGame().GetItemManager().GetItem(320, out PetFood))
//            {
//                Item Food = ItemFactory.CreateSingleItemNullable(PetFood, Session.GetHabbo(), "", "");
//                if (Food != null)
//                {
//                    Session.GetHabbo().GetInventoryComponent().TryAddItem(Food);
//                    Session.SendMessage(new FurniListNotificationComposer(Food.Id, 1));
//                }
//            }
//        }

//        private static string RandomClothing()
//        {
//            Random Random = new Random();

//            int RandomNumber = Random.Next(1, 6);
//            switch (RandomNumber)
//            {
//                default:
//                case 1:
//                    return "5 0 -1 0 4 402 5 3 301 4 1 101 2 2 201 3";
//                case 2:
//                    return "5 0 -1 0 1 102 13 3 301 4 4 401 5 2 201 3";
//                case 3:
//                    return "5 1 102 8 2 201 16 4 401 9 3 303 4 0 -1 6";
//                case 4:
//                    return "5 0 -1 0 3 303 4 4 401 5 1 101 2 2 201 3";
//                case 5:
//                    return "5 3 302 4 2 201 11 1 102 12 0 -1 28 4 401 24";
//                case 6:
//                    return "5 4 402 5 3 302 21 0 -1 7 1 101 12 2 201 17";
//            }
//        }
//        private static string RandonColor()
//        {
//            Random Random = new Random();

//            int RandomNumber = Random.Next(6, 12);
//            switch (RandomNumber)
//            {
//                default:
//                case 1:
//                    return "1";
//                case 2:
//                    return "2";
//                case 3:
//                    return "3";
//                case 4:
//                    return "4";
//                case 5:
//                    return "5";
//                case 6:
//                    return "6";
//                case 7:
//                    return "7";
//                case 8:
//                    return "8";
//                case 9:
//                    return "9";
//                case 10:
//                    return "10";
//                case 11:
//                    return "11";
//                case 12:
//                    return "12";
//            }
//        }
//    }
//}
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Rooms.AI;
using Galaxy.HabboHotel.Rooms.AI.Speech;
using Galaxy.HabboHotel.Items.Utilities;



using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;

using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms.AI.Responses;
using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using System.ServiceModel.Channels;
using System.Data;
using Galaxy.Communication.Packets.Outgoing.Rooms.Furni;

namespace Galaxy.Communication.Packets.Incoming.Catalog
{
	class CheckGnomeNameEvent : IPacketEvent
	{
		public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
		{
			if (Session == null || Session.GetHabbo() == null)
				return;

			int Id = Packet.PopInt();
			string Pin = Packet.PopString();

			DataRow password = null;

			using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
			{
				dbClient.SetQuery("SELECT `pin`,ip_last FROM users WHERE `id` = @Id LIMIT 1");
				dbClient.AddParameter("Id", Session.GetHabbo().Id);
				password = dbClient.getRow();
			}


			if (Pin == password["pin"].ToString())
			{
				Session.GetHabbo().isLoggedIn = true;
				GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " fez o login staff!", ""));
				GalaxyServer.discordWH(":lock: O usuário " + Session.GetHabbo().Username + " acaba de realizar o login staff! IP: " + password["ip_last"].ToString());
				Session.SendWhisper("Cotação do dólar é atualizada sempre que é realizado o loginstaff.");
				GalaxyServer.ValorDolar();
				Session.SendMessage(new CheckGnomeNameComposer(Pin, 0));
			}
			else
			{
				Session.SendMessage(new CheckGnomeNameComposer(Pin, 0));
				Session.SendMessage(new GnomeBoxComposer(0));
				GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " errou a senha no login staff!", ""));
				this.LogCommand(Session.GetHabbo().Id, "Loginstaff inválido", Session.GetHabbo().MachineId, Session.GetHabbo().Username);
			}


		}
		public void LogCommand(int UserId, string Data, string MachineId, string Username)
		{
			using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
			{
				dbClient.SetQuery("INSERT INTO `logs_client_staff` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
				dbClient.AddParameter("UserId", UserId);
				dbClient.AddParameter("Data", Data);
				dbClient.AddParameter("MachineId", MachineId);
				dbClient.AddParameter("Timestamp", GalaxyServer.GetUnixTimestamp());
				dbClient.RunQuery();
			}
		}
	}
}