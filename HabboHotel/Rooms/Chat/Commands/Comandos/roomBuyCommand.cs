using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Items;
using System.Collections.Generic;
using System.Linq;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class roomBuyyCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return "[OFERTA]"; }
        }

        public string Description
        {
            get { return "Possibilita comprar o quarto dizendo o preço [Ex. 1000c OR 200d]"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room CurrentRoom, string[] Params)
        {
            //Gets the current RoomUser
            RoomUser User = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            RoomUser RoomOwner = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(CurrentRoom.RoomData.OwnerId);
            //If it grabs an invalid user somehow, it returns
            if (User == null)
                return;

            if (RoomOwner == null)
            {
                User.GetClient().SendWhisper("Dono do quarto não encontrado", 3);
                return;
            }
            //Room is not for sale
            if (!CurrentRoom.RoomData.roomForSale)
            {
                User.GetClient().SendWhisper("Você não pôde comrpar este quarto, o dono não o colocou á venda", 3);
                return;
            }

            //If does not have any value or the length does not contain a number with letter
            if (Params.Length == 1 || Params[1].Length < 2)
            {
                Session.SendWhisper("Por favor insira uma oferta válida pelo quarto [Ex. 145000c OR 75000d]", 3);
                return;
            }

            //Assigns the input to a variable
            string ActualInput = Params[1];

            //If they are the current room owner
            if (CurrentRoom.RoomData.OwnerId == User.HabboId)
            {
                Session.SendWhisper("Você não pode comprar seu próprio quarto, diga :comprarquarto 0c", 3);
                return;
            }

            string roomCostType = ActualInput.Substring(ActualInput.Length - 1);

            //Declares the variable to be assigned in the try statement if they entered a valid offer
            int roomCost;
            try
            {
                //Great! It's valid if it passes this try
                roomCost = int.Parse(ActualInput.Substring(0, ActualInput.Length - 1));
            }
            catch
            {
                //Nope, Invalid integer
                User.GetClient().SendWhisper("Você precisa inserir um valor de quarto válido", 3);
                return;
            }

            //Is there offer out of bounds?
            if (roomCost < 1 || roomCost > 10000000)
            {
                User.GetClient().SendWhisper("Oferta inválida, Ela é muito alta ou muito baixa", 3);
                return;
            }

            //Start doing the checks
            if (roomCost == CurrentRoom.RoomData.roomSaleCost && roomCostType == CurrentRoom.RoomData.roomSaleType)
            {
                if (roomCostType == "c")
                {
                    if (User.GetClient().GetHabbo().Credits >= roomCost)
                    {
                        //Everything is valid now, do the magic.
                        NewRoomOwner(CurrentRoom, User, RoomOwner);
                    }
                    else
                    {
                        User.GetClient().SendWhisper("Você não tem créditos suficientes.", 3);
                        return;
                    }
                }
                else if (roomCostType == "d")
                {
                    if (User.GetClient().GetHabbo().Diamonds >= roomCost)
                    {
                        //Everything is valid now, do the magic.
                        NewRoomOwner(CurrentRoom, User, RoomOwner);
                    }
                    else
                    {
                        User.GetClient().SendWhisper("Você não tem diamantes suficientes", 3);
                        return;
                    }
                }
                else
                {
                    Session.SendWhisper("Você não deveria ver esta mensagem, por favor contate um Staff", 3);
                    return;
                }
            }
            else
            {
                MakeOffer(CurrentRoom, User, RoomOwner, roomCost, roomCostType);
            }

        }

        public void NewRoomOwner(Room RoomForSale, RoomUser BoughtRoomUser, RoomUser SoldRoomUser)
        {
            //Pre-Emptive Things
            using (IQueryAdapter Adapter = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                Adapter.SetQuery("UPDATE rooms SET owner = @newowner WHERE id = @roomid");
                Adapter.AddParameter("newowner", BoughtRoomUser.HabboId);
                Adapter.AddParameter("roomid", RoomForSale.RoomData.Id);
                Adapter.RunQuery();

                Adapter.SetQuery("UPDATE items SET user_id = @newowner WHERE room_id = @roomid");
                Adapter.AddParameter("newowner", BoughtRoomUser.HabboId);
                Adapter.AddParameter("roomid", RoomForSale.RoomData.Id);
                Adapter.RunQuery();

                Adapter.SetQuery("DELETE FROM room_rights WHERE room_id = @roomid");
                Adapter.AddParameter("roomid", RoomForSale.RoomData.Id);
                Adapter.RunQuery();

                if (RoomForSale.Group != null)
                {
                    Adapter.SetQuery("SELECT id FROM groups WHERE room_id = @roomid");
                    Adapter.AddParameter("roomid", RoomForSale.RoomData.Id);

                    int GroupId = Adapter.getInteger();

                    if (GroupId > 0)
                    {
                        RoomForSale.Group.ClearRequests();

                        foreach (int MemberId in RoomForSale.Group.GetAllMembers)
                        {
                            RoomForSale.Group.DeleteMember(MemberId);

                            GameClients.GameClient MemberClient = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(MemberId);

                            if (MemberClient == null)
                                continue;

                            if (MemberClient.GetHabbo().GetStats().FavouriteGroupId == GroupId)
                            {
                                MemberClient.GetHabbo().GetStats().FavouriteGroupId = 0;
                            }
                        }

                        Adapter.RunQuery("DELETE FROM `groups` WHERE `id` = '" + RoomForSale.Group.Id + "'");
                        Adapter.RunQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + RoomForSale.Group.Id + "'");
                        Adapter.RunQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + RoomForSale.Group.Id + "'");
                        Adapter.RunQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + RoomForSale.Group.Id + "' LIMIT 1");
                        Adapter.RunQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + RoomForSale.Group.Id + "' LIMIT 1");
                        Adapter.RunQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + RoomForSale.Group.Id + "'");
                    }
                    GalaxyServer.GetGame().GetGroupManager().DeleteGroup(RoomForSale.Group.Id);
                    RoomForSale.RoomData.Group = null;
                    RoomForSale.Group = null;
                }
            }
            //Change Room Owners
            RoomForSale.RoomData.OwnerId = BoughtRoomUser.HabboId;
            RoomForSale.RoomData.OwnerName = BoughtRoomUser.GetClient().GetHabbo().Username;

            //Change Item Owners
            foreach (Item CurrentItem in RoomForSale.GetRoomItemHandler().GetWallAndFloor)
            {
                CurrentItem.UserID = BoughtRoomUser.HabboId;
                CurrentItem.Username = BoughtRoomUser.GetClient().GetHabbo().Username;
            }

            //Take Credits or Diamonds from User
            if (RoomForSale.RoomData.roomSaleType == "c")
            {
                BoughtRoomUser.GetClient().GetHabbo().Credits -= RoomForSale.RoomData.roomSaleCost;
                BoughtRoomUser.GetClient().SendMessage(new CreditBalanceComposer(BoughtRoomUser.GetClient().GetHabbo().Credits));
            }
            else if (RoomForSale.RoomData.roomSaleType == "d")
            {
                BoughtRoomUser.GetClient().GetHabbo().Diamonds -= RoomForSale.RoomData.roomSaleCost;
                BoughtRoomUser.GetClient().SendMessage(new HabboActivityPointNotificationComposer(BoughtRoomUser.GetClient().GetHabbo().Diamonds, 0, 5));
            }


            //Give Credits or Diamonds to User
            if (RoomForSale.RoomData.roomSaleType == "c")
            {
                SoldRoomUser.GetClient().GetHabbo().Credits += RoomForSale.RoomData.roomSaleCost;
                SoldRoomUser.GetClient().SendMessage(new CreditBalanceComposer(SoldRoomUser.GetClient().GetHabbo().Credits));
            }
            else if (RoomForSale.RoomData.roomSaleType == "d")
            {
                SoldRoomUser.GetClient().GetHabbo().Diamonds += RoomForSale.RoomData.roomSaleCost;
                SoldRoomUser.GetClient().SendMessage(new HabboActivityPointNotificationComposer(SoldRoomUser.GetClient().GetHabbo().Diamonds, 0, 5));

            }

            //Unsign Room
            RoomForSale.RoomData.roomForSale = false;
            RoomForSale.RoomData.roomSaleCost = 0;
            RoomForSale.RoomData.roomSaleType = "";

            //Unload the Room
            //RoomBuyCommand Updated By Hamada Zipto
            Room R = null;
            if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(RoomForSale.Id, out R))
                return;
            List<RoomUser> UsersToReturn = RoomForSale.GetRoomUserManager().GetRoomUsers().ToList();
            GalaxyServer.GetGame().GetNavigator().Init();
            GalaxyServer.GetGame().GetRoomManager().UnloadRoom(R, true);
            foreach (RoomUser User in UsersToReturn)
            {
                if (User == null || User.GetClient() == null)
                    continue;

                User.GetClient().SendMessage(new RoomForwardComposer(RoomForSale.Id));
                User.GetClient().SendNotification("<b> Alerta no quarto </b>\r\rO quarto acaba de ser comprado por\r\r<b>" + BoughtRoomUser.GetClient().GetHabbo().Username + "</b>!");

            }
        }

        public void MakeOffer(Room RoomForSale, RoomUser OfferingUser, RoomUser RoomOwner, int OfferCost, string OfferType)
        {
            if (RoomOwner.RoomOfferPending)
            {
                OfferingUser.GetClient().SendWhisper("Este user tem uma oferta pendente, por favor aguarde", 3);
                return;
            }
            if (OfferType == "c")
            {
                if (OfferingUser.GetClient().GetHabbo().Credits < OfferCost)
                {
                    OfferingUser.GetClient().SendWhisper("Você não tem mais créditos", 3);
                    return;
                }
            }
            else if (OfferType == "d")
            {
                if (OfferingUser.GetClient().GetHabbo().Diamonds < OfferCost)
                {
                    OfferingUser.GetClient().SendWhisper("Você não tem mais diamantes", 3);
                    return;
                }
            }
            string TheOffer = OfferCost + "" + OfferType;

            if (RoomOwner.RoomId == RoomForSale.RoomData.Id)
            {
                RoomOwner.GetClient().SendWhisper("Nova oferta pelo quarto de " + OfferingUser.GetUsername() + " com a oferta de " + TheOffer + " escreva :aceitaroferta ou :negaroferta para responder.");
                RoomOwner.RoomOfferPending = true;
                RoomOwner.RoomOffer = TheOffer;
                RoomOwner.RoomOfferUser = OfferingUser.HabboId;
                OfferingUser.GetClient().SendWhisper("Oferta enviada!");
            }
            else
            {
                OfferingUser.GetClient().SendWhisper("Dono do quarto não encontrado", 3);
                return;
            }
        }

    }
}