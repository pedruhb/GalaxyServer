using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Items;
using System.Collections.Generic;
using System.Linq;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class roomAccepttOffer : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Aceita a oferta vigente pelo quarto"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room CurrentRoom, string[] Params)
        {
            RoomUser RoomOwner = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (RoomOwner.RoomOfferPending)
            {
                if (RoomOwner.GetClient().GetHabbo().CurrentRoom.RoomData.roomForSale)
                {
                    if (RoomOwner.GetClient().GetHabbo().CurrentRoom.RoomData.OwnerId == RoomOwner.GetClient().GetHabbo().Id)
                    {
                        RoomUser OfferingUser = CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(RoomOwner.RoomOfferUser);
                        OfferingUser.GetClient().SendWhisper("O dono do quarto aceitou sua oferta, a compra está sendo feita agora.");
                        NewRoomOwner(RoomOwner.GetClient().GetHabbo().CurrentRoom, OfferingUser, RoomOwner);
                        RoomOwner.RoomOfferPending = false;
                        RoomOwner.RoomOfferUser = 0;
                        RoomOwner.RoomOffer = "";
                    }
                }
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
                User.GetClient().SendNotification("<b> Alerta do quarto </b>\r\rO quarto foi comprado por\r\r<b>" + BoughtRoomUser.GetClient().GetHabbo().Username + "</b>!");

            }
        }


    }
}