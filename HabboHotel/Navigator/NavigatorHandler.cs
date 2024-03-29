﻿using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Groups;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users.Messenger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Galaxy.HabboHotel.Navigator
{
    static class NavigatorHandler
    {
        public static void Search(ServerPacket Message, SearchResultList SearchResult, string SearchData, GameClient Session, int FetchLimit)
        {
            //Switching by categorys.
            switch (SearchResult.CategoryType)
            {
                default:
                    Message.WriteInteger(0);
                    break;
                case NavigatorCategoryType.QUERY:
                    {
                        #region Query
                        if (SearchData.ToLower().StartsWith("owner:"))
                        {
                            if (SearchData.Length > 0)
                            {
                                int UserId = 0;
                                DataTable GetRooms = null;
                                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                                {
                                    if (SearchData.ToLower().StartsWith("owner:"))
                                    {
                                        dbClient.SetQuery("SELECT `id` FROM `users` WHERE `username` = @username LIMIT 1");
                                        dbClient.AddParameter("username", SearchData.Remove(0, 6));
                                        UserId = dbClient.getInteger();

                                        if (Session.GetHabbo().Rank > 10)
                                        {
                                            dbClient.SetQuery("SELECT * FROM `rooms` WHERE `owner` = '" + UserId + "' ORDER BY `users_now` DESC");
                                            GetRooms = dbClient.getTable();
                                        } else
                                        {
                                         dbClient.SetQuery("SELECT * FROM `rooms` WHERE `owner` = '" + UserId + "' and `state` != 'invisible' ORDER BY `users_now` DESC");
                                        GetRooms = dbClient.getTable();
                                        }
                                    }
                                }
                                List<RoomData> Results = new List<RoomData>();
                                if (GetRooms != null)
                                {
                                    foreach (DataRow Row in GetRooms.Rows)
                                    {
                                        RoomData RoomData = GalaxyServer.GetGame().GetRoomManager().FetchRoomData(Convert.ToInt32(Row["id"]), Row);
                                        if (RoomData != null && !Results.Contains(RoomData))
                                            Results.Add(RoomData);
                                    }
                                }
                                Message.WriteInteger(Results.Count);
                                foreach (RoomData Data in Results.ToList())
                                {
                                    RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                                }
                            }
                        }
                        else if (SearchData.ToLower().StartsWith("tag:"))
                        {
                            SearchData = SearchData.Remove(0, 4);
                            ICollection<RoomData> TagMatches = GalaxyServer.GetGame().GetRoomManager().SearchTaggedRooms(SearchData);
                            Message.WriteInteger(TagMatches.Count);
                            foreach (RoomData Data in TagMatches.ToList())
                            {
                                RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                            }
                        }
                        else if (SearchData.ToLower().StartsWith("group:"))
                        {
                            SearchData = SearchData.Remove(0, 6);
                            ICollection<RoomData> GroupRooms = GalaxyServer.GetGame().GetRoomManager().SearchGroupRooms(SearchData);
                            Message.WriteInteger(GroupRooms.Count);
                            foreach (RoomData Data in GroupRooms.ToList())
                            {
                                RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                            }
                        }
                        else
                        {
                            if (SearchData.Length > 0)
                            {
                                DataTable Table = null;
                                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                                {
                                    dbClient.SetQuery("SELECT * FROM rooms WHERE `caption` LIKE @query ORDER BY `users_now` DESC LIMIT 50");
                                    dbClient.AddParameter("query", "%" + SearchData + "%");
                                    Table = dbClient.getTable();
                                }
                                List<RoomData> Results = new List<RoomData>();
                                if (Table != null)
                                {
                                    foreach (DataRow Row in Table.Rows)
                                    {
										if (Convert.ToString(Row["state"]) != "invisible")
										{
											RoomData RData = GalaxyServer.GetGame().GetRoomManager().FetchRoomData(Convert.ToInt32(Row["id"]), Row);
											if (RData != null && !Results.Contains(RData))
												Results.Add(RData);
										}

                                    }
                                }
                                Message.WriteInteger(Results.Count);
                                foreach (RoomData Data in Results.ToList())
                                {
                                    RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                                }
                            }
                        }
                        #endregion
                        break;
                    }
                case NavigatorCategoryType.FEATURED:
                    {
                        #region Featured
                        List<RoomData> Rooms = new List<RoomData>();
                        ICollection<FeaturedRoom> Featured = GalaxyServer.GetGame().GetNavigator().GetFeaturedRooms(SearchResult.Id);
                        foreach (FeaturedRoom FeaturedItem in Featured.ToList())
                        {
                            if (FeaturedItem == null)
                                continue;
                            RoomData Data = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(FeaturedItem.RoomId);
                            if (Data == null)
                                continue;
                            if (!Rooms.Contains(Data))
                                Rooms.Add(Data);
                        }
                        Message.WriteInteger(Rooms.Count);
                        foreach (RoomData Data in Rooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        #endregion
                        break;
                    }
                case NavigatorCategoryType.STAFF_PICKS:
                    {
                        #region Featured
                        List<RoomData> rooms = new List<RoomData>();
                        ICollection<StaffPick> picks = GalaxyServer.GetGame().GetNavigator().GetStaffPicks();
                        foreach (StaffPick pick in picks.ToList())
                        {
                            if (pick == null)
                                continue;
                            RoomData Data = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(pick.RoomId);
                            if (Data == null)
                                continue;
                            if (!rooms.Contains(Data))
                                rooms.Add(Data);
                        }
                        Message.WriteInteger(rooms.Count);
                        foreach (RoomData data in rooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, data, data.Promotion);
                        }
                        #endregion
                        break;
                    }
                case NavigatorCategoryType.POPULAR:
                    {
                        List<RoomData> PopularRooms = GalaxyServer.GetGame().GetRoomManager().GetPopularRooms(-1, FetchLimit);
                        Message.WriteInteger(PopularRooms.Count);
                        foreach (RoomData Data in PopularRooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }
                case NavigatorCategoryType.RECOMMENDED:
                    {
                        List<RoomData> RecommendedRooms = GalaxyServer.GetGame().GetRoomManager().GetRecommendedRooms(FetchLimit);
                        Message.WriteInteger(RecommendedRooms.Count);
                        foreach (RoomData Data in RecommendedRooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }
                case NavigatorCategoryType.CATEGORY:
                    {
                        List<RoomData> GetRoomsByCategory = GalaxyServer.GetGame().GetRoomManager().GetRoomsByCategory(SearchResult.Id, FetchLimit);
                        Message.WriteInteger(GetRoomsByCategory.Count);
                        foreach (RoomData Data in GetRoomsByCategory.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }
                case NavigatorCategoryType.MY_ROOMS:
                    Message.WriteInteger(Session.GetHabbo().UsersRooms.Count);
                    foreach (RoomData Data in Session.GetHabbo().UsersRooms.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;
                case NavigatorCategoryType.MY_FAVORITES:
                    List<RoomData> Favourites = new List<RoomData>();
                    foreach (int Id in Session.GetHabbo().FavoriteRooms.ToArray())
                    {
                        RoomData Room = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(Id);
                        if (Room == null)
                            continue;
                        if (!Favourites.Contains(Room))
                            Favourites.Add(Room);
                    }
                    Favourites = Favourites.Take(FetchLimit).ToList();
                    Message.WriteInteger(Favourites.Count);
                    foreach (RoomData Data in Favourites.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;
                case NavigatorCategoryType.MY_GROUPS:
                    List<RoomData> MyGroups = new List<RoomData>();
                    foreach (Group Group in GalaxyServer.GetGame().GetGroupManager().GetGroupsForUser(Session.GetHabbo().Id).ToList())
                    {
                        if (Group == null)
                            continue;
                        RoomData Data = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(Group.RoomId);
                        if (Data == null)
                            continue;
                        if (!MyGroups.Contains(Data))
                            MyGroups.Add(Data);
                    }
                    MyGroups = MyGroups.Take(FetchLimit).ToList();
                    Message.WriteInteger(MyGroups.Count);
                    foreach (RoomData Data in MyGroups.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;
                case NavigatorCategoryType.MY_FRIENDS_ROOMS:
                    List<RoomData> MyFriendsRooms = new List<RoomData>();
                    foreach (MessengerBuddy buddy in Session.GetHabbo().GetMessenger().GetFriends().Where(p => p.InRoom))
                    {
                        if (buddy == null || !buddy.InRoom || buddy.UserId == Session.GetHabbo().Id)
                            continue;
                        if (!MyFriendsRooms.Contains(buddy.CurrentRoom.RoomData))
                            MyFriendsRooms.Add(buddy.CurrentRoom.RoomData);
                    }
                    Message.WriteInteger(MyFriendsRooms.Count);
                    foreach (RoomData Data in MyFriendsRooms.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;
                case NavigatorCategoryType.MY_RIGHTS:
                    List<RoomData> MyRights = new List<RoomData>();
                    DataTable GetRights = null;
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT `room_id` FROM `room_rights` WHERE `user_id` = @UserId LIMIT @FetchLimit");
                        dbClient.AddParameter("UserId", Session.GetHabbo().Id);
                        dbClient.AddParameter("FetchLimit", FetchLimit);
                        GetRights = dbClient.getTable();
                        foreach (DataRow Row in GetRights.Rows)
                        {
                            RoomData Data = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(Convert.ToInt32(Row["room_id"]));
                            if (Data == null)
                                continue;
                            if (!MyRights.Contains(Data))
                                MyRights.Add(Data);
                        }
                    }
                    Message.WriteInteger(MyRights.Count);
                    foreach (RoomData Data in MyRights.ToList())
                    {
                        RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                    }
                    break;
                case NavigatorCategoryType.TOP_PROMOTIONS:
                    {
                        List<RoomData> GetPopularPromotions = GalaxyServer.GetGame().GetRoomManager().GetOnGoingRoomPromotions(16, FetchLimit);
                        Message.WriteInteger(GetPopularPromotions.Count);
                        foreach (RoomData Data in GetPopularPromotions.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }
                case NavigatorCategoryType.PROMOTION_CATEGORY:
                    {
                        List<RoomData> GetPromotedRooms = GalaxyServer.GetGame().GetRoomManager().GetPromotedRooms(SearchResult.Id, FetchLimit);
                        Message.WriteInteger(GetPromotedRooms.Count);
                        foreach (RoomData Data in GetPromotedRooms.ToList())
                        {
                            RoomAppender.WriteRoom(Message, Data, Data.Promotion);
                        }
                        break;
                    }
            }
        }
    }
}