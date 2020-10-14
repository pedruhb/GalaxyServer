using Galaxy.Communication;
using Galaxy.Communication.ConnectionManager;
using Galaxy.Communication.Encryption.Crypto.Prng;
using Galaxy.Communication.Interfaces;
using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Communication.Packets.Outgoing.BuildersClub;
using Galaxy.Communication.Packets.Outgoing.Handshake;
using Galaxy.Communication.Packets.Outgoing.Inventory.Achievements;
using Galaxy.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Communication.Packets.Outgoing.Navigator;
using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Rooms.Furni;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing.Sound;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Catalog;
using Galaxy.HabboHotel.Moderation;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.Users.Messenger;
using Galaxy.HabboHotel.Users.Messenger.FriendBar;
using Galaxy.HabboHotel.Users.UserData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Galaxy.HabboHotel.GameClients
{
    public class GameClient
    {
        private readonly int _id;
        private Habbo _habbo;
		public string ssoTicket;
		public string MachineId;
        private bool _disconnected;
        public ARC4 RC4Client = null;
        private GamePacketParser _packetParser;
        private ConnectionInformation _connection;
        public int PingCount { get; set; }
        internal int CurrentRoomUserId;
        internal DateTime TimePingedReceived;

		public GameClient(int ClientId, ConnectionInformation pConnection)
        {
            _id = ClientId;
            _connection = pConnection;
            _packetParser = new GamePacketParser(this);

            PingCount = 0;
        }

        private void SwitchParserRequest()
        {
            _packetParser.SetConnection(_connection);
            _packetParser.OnNewPacket += Parser_OnNewPacket;
            byte[] data = (_connection.parser as InitialPacketParser).currentData;
            _connection.parser.Dispose();
            _connection.parser = _packetParser;
            _connection.parser.handlePacketData(data);
        }

        private void Parser_OnNewPacket(ClientPacket Message)
        {
            try
            {
                GalaxyServer.GetGame().GetPacketManager().TryExecutePacket(this, Message);
            }
            catch (Exception e)
            {

				ExceptionLogger.LogException(e);
			}
        }

        private void PolicyRequest()
        {
            _connection.SendData(GalaxyServer.GetDefaultEncoding().GetBytes("<?xml version=\"1.0\"?>\r\n" +
                   "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                   "<cross-domain-policy>\r\n" +
                   "<allow-access-from domain=\"*\" to-ports=\"1-31111\" />\r\n" +
                   "</cross-domain-policy>\x0"));
        }


        public void StartConnection()
        {
            if (_connection == null)
                return;

            PingCount = 0;

            (_connection.parser as InitialPacketParser).PolicyRequest += PolicyRequest;
            (_connection.parser as InitialPacketParser).SwitchParserRequest += SwitchParserRequest;
            _connection.startPacketProcessing();
        }

        public bool TryAuthenticate(string AuthTicket)
        {
            try
            {
                UserData userData = UserDataFactory.GetUserData(AuthTicket, out byte errorCode);
                if (errorCode == 1 || errorCode == 2)
                {
                    //.WriteLine("Erro ao obter o AuthTicket do usuário " + AuthTicket + ".");
                    Disconnect();
                    return false;
                }

                #region Ban Checking
                //Let's have a quick search for a ban before we successfully authenticate..
                ModerationBan BanRecord = null;
                if (!string.IsNullOrEmpty(MachineId))
                {
                    if (GalaxyServer.GetGame().GetModerationManager().IsBanned(MachineId, out BanRecord))
                    {
                        if (GalaxyServer.GetGame().GetModerationManager().MachineBanCheck(MachineId))
                        {
                            Disconnect();
                            return false;
                        }
                    }
                }

                if (userData.user != null)
                {
                    //Now let us check for a username ban record..
                    BanRecord = null;
                    if (GalaxyServer.GetGame().GetModerationManager().IsBanned(userData.user.Username, out BanRecord))
                    {
                        if (GalaxyServer.GetGame().GetModerationManager().UsernameBanCheck(userData.user.Username))
                        {
                            Disconnect();
                            return false;
                        }
                    }
                }
                #endregion

                GalaxyServer.GetGame().GetClientManager().RegisterClient(this, userData.userID, userData.user.Username);
                _habbo = userData.user;
                _habbo.ssoTicket = AuthTicket;
                if (_habbo != null)
                {
					this.ssoTicket = AuthTicket;
					userData.user.Init(this, userData);

                    SendMessage(new AuthenticationOKComposer());
                    SendMessage(new AvatarEffectsComposer(_habbo.Effects().GetAllEffects));
                    /// Fix de quarto by PHB
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT quartoid FROM `users` WHERE id = '" + GetHabbo().Id + "'");
                        DataRow Table = dbClient.getRow();
                        if (Convert.ToInt32(Table["quartoid"]) > 0)
                        {
                            dbClient.SetQuery("SELECT caption FROM `rooms` WHERE id = '" + Convert.ToInt32(Table["quartoid"]) + "'");
                            DataRow Table2 = dbClient.getRow();
                            SendNotification("Você será redirecionado para o quarto '" + Table2["caption"] + "'");
                            SendMessage(new NavigatorSettingsComposer(Convert.ToInt32(Table["quartoid"])));
                            dbClient.runFastQuery("UPDATE users SET quartoid = '0' WHERE id = '" + GetHabbo().Id + "'");
                        }
                        else
                        {
                            //// Bloqueio de preso
                            DataRow preso = null;
                            using (var dbClient2 = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient2.SetQuery("SELECT Presidio FROM users WHERE id = '" + GetHabbo().Id + "'");
                                preso = dbClient2.getRow();
                            }

                            if (System.Convert.ToBoolean(preso["Presidio"]))
                            {
                                this.SendMessage(new RoomNotificationComposer("police_announcement", "message", "Você está preso e foi teletransportado automaticamente para a cadeia."));
                                SendMessage(new NavigatorSettingsComposer(ExtraSettings.Prisao));
                            }
                            else
                            {
                                SendMessage(new NavigatorSettingsComposer(_habbo.HomeRoom));
                            }
                            /////
                        }
                    }
                    ///// End fix de quarto
                    SendMessage(new FavouritesComposer(userData.user.FavoriteRooms));
                    SendMessage(new FigureSetIdsComposer(_habbo.GetClothing().GetClothingParts));
                    SendMessage(new UserRightsComposer(_habbo));
                    SendMessage(new AvailabilityStatusComposer());
                    SendMessage(new AchievementScoreComposer(_habbo.GetStats().AchievementPoints));
                    SendMessage(new BuildersClubMembershipComposer());
                    SendMessage(new CfhTopicsInitComposer(GalaxyServer.GetGame().GetModerationManager().UserActionPresets));
                    SendMessage(new BadgeDefinitionsComposer(GalaxyServer.GetGame().GetAchievementManager()._achievements));
                    SendMessage(new SoundSettingsComposer(_habbo.ClientVolume, _habbo.ChatPreference, _habbo.AllowMessengerInvites, _habbo.FocusPreference, FriendBarStateUtility.GetInt(_habbo.FriendbarState)));
                    SendMessage(new FigureSetIdsComposer(_habbo.GetClothing().GetClothingParts));
                    GetHabbo().Look = GetHabbo().Look;
                    if (GetHabbo().GetMessenger() != null)
                        GetHabbo().GetMessenger().OnStatusChanged(true);

                    if (!string.IsNullOrEmpty(MachineId))
                    {
                        if (_habbo.MachineId != MachineId)
                        {
                            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("UPDATE `users` SET `machine_id` = @MachineId WHERE `id` = @id LIMIT 1");
                                dbClient.AddParameter("MachineId", MachineId);
                                dbClient.AddParameter("id", _habbo.Id);
                                dbClient.RunQuery();
                            }
                        }

                        _habbo.MachineId = MachineId;
                    }

					

					if (!GalaxyServer.GetGame().GetCacheManager().ContainsUser(_habbo.Id))
                        GalaxyServer.GetGame().GetCacheManager().GenerateUser(_habbo.Id);

                    _habbo.InitProcess();

                    this.GetHabbo()._lastitems = new Dictionary<int, CatalogItem>();

					if (userData.user.GetPermissions().HasRight("mod_tickets"))
                    {
                        SendMessage(new ModeratorInitComposer(
                         GalaxyServer.GetGame().GetModerationManager().UserMessagePresets,
                         GalaxyServer.GetGame().GetModerationManager().RoomMessagePresets,
                         GalaxyServer.GetGame().GetModerationManager().GetTickets));
                    } 

                    if (GalaxyServer.GetGame().GetSettingsManager().TryGetValue("user.login.message.enabled") == "1")
                        SendMessage(new MOTDNotificationComposer(GalaxyServer.GetGame().GetLanguageManager().TryGetValue("user.login.message")));

                    if (GalaxyServer.GetGame().GetLanguageManager().TryGetValue("user.login.message") != "off")
                    {
                        string mensagem = GalaxyServer.GetGame().GetLanguageManager().TryGetValue("user.login.message");

                        mensagem = mensagem.Replace("%username%", GetHabbo().Username);
                        mensagem = mensagem.Replace("%hotelname%", GalaxyServer.HotelName);
                        mensagem = mensagem.Replace("%userson%", GalaxyServer.GetGame().GetClientManager().Count.ToString());

                        SendMessage(new MOTDNotificationComposer(mensagem));
                    }

                    if (GetHabbo().Rank > 1)
                    {
                        using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            dbClient.runFastQuery("INSERT INTO `client_staff_enter` (`user`, `date`) VALUES ('" + GetHabbo().Id + "', '" + GalaxyServer.GetUnixTimestamp() +"');");
                    }

                    if (ExtraSettings.TARGETED_OFFERS_ENABLED)
                    {
                        if (GalaxyServer.GetGame().GetTargetedOffersManager().TargetedOffer != null)
                        {
                            GalaxyServer.GetGame().GetTargetedOffersManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                            TargetedOffers TargetedOffer = GalaxyServer.GetGame().GetTargetedOffersManager().TargetedOffer;

                            if (TargetedOffer.Expire > GalaxyServer.GetIUnixTimestamp())
                            {

                                if (TargetedOffer.Limit != GetHabbo()._TargetedBuy)
                                {

                                    SendMessage(GalaxyServer.GetGame().GetTargetedOffersManager().TargetedOffer.Serialize());
                                }
                            }
                            else
                            {
                                using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                                    dbClient.runFastQuery("UPDATE targeted_offers SET active = 'false'");
                                using (var dbClient2 = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                                    dbClient2.runFastQuery("UPDATE users SET targeted_buy = '0' WHERE targeted_buy > 0");
                            }
                        }
                    }

					/// Mensagem de bem vindo nux
					if (ExtraSettings.ImagemBemVindo != "") {
						ServerPacket notif = new ServerPacket(ServerPacketHeader.NuxAlertMessageComposer);
						notif.WriteString("habbopages/bemvindo.php?hn="+GalaxyServer.HotelName+ "&img="+ExtraSettings.ImagemBemVindo);
						userData.user.GetClient().SendMessage(notif);
					}


					userData.user.GetClient().GetHabbo().GetClubManager().AddOrExtendSubscription("habbo_vip", 863913600, userData.user.GetClient());

					GalaxyServer.SendUserJson(userData.user.GetClient(), "{ \"tipo\": \"welcome\" }");

                    string dFrank = null;
                    using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT Datahoje FROM users WHERE id = '" + userData.user.GetClient().GetHabbo().Id + "' LIMIT 1");
                        dFrank = dbClient.getString();
                    }
                    int dFrankInt = Int32.Parse(dFrank);
                    DateTime dateGregorian = new DateTime();
                    dateGregorian = DateTime.Today;
                    int day = (dateGregorian.Day);
                    if (dFrankInt != day)
                    {
                        using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE users SET Datahoje = '" + day + "' WHERE id = " + GetHabbo().Id + ";");
                        }
                        #region Premiação diária by PHB
                        if (ExtraSettings.PremiacaoDiaria)
                        {
							// Premiação diária by PHB

							DataRow moedasRank = null;
							using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
							{
								dbClient.SetQuery("SELECT * FROM ranks WHERE id = '" + GetHabbo().Rank + "'");
								moedasRank = dbClient.getRow();
							}
							int creditspre = Convert.ToInt32(moedasRank["moedas_diario"]);
                            int ducketspre = Convert.ToInt32(moedasRank["duckets_diario"]);
							int diamondspre = Convert.ToInt32(moedasRank["diamantes_diario"]);
							int gotwpre = Convert.ToInt32(moedasRank["gotw_diario"]);

							if ((creditspre + ducketspre + diamondspre + gotwpre) > 0)
							{
								GetHabbo().Credits += creditspre;
								SendMessage(new CreditBalanceComposer(GetHabbo().Credits));
								GetHabbo().Duckets += ducketspre;
								SendMessage(new HabboActivityPointNotificationComposer(GetHabbo().Duckets, ducketspre));
								GetHabbo().Diamonds += diamondspre;
								SendMessage(new HabboActivityPointNotificationComposer(GetHabbo().Diamonds, diamondspre, 5));
								GetHabbo().GOTWPoints += gotwpre;
								SendMessage(new HabboActivityPointNotificationComposer(GetHabbo().GOTWPoints, 0, 103));

								string msggotw = "";
								string msgdiamonds = "";
								string msgduckets = "";
								string msgcreditos = "";

								if (gotwpre > 0)
								{
									msggotw = gotwpre + " " + ExtraSettings.NomeGotw + ", ";
								}
								if (diamondspre > 0)
								{
									msgdiamonds = diamondspre + " " + ExtraSettings.NomeDiamantes + ", ";
								}
								if (ducketspre > 0)
								{
									msgduckets = ducketspre + " " + ExtraSettings.NomeDuckets + ", ";
								}
								if (creditspre > 0)
								{
									msgcreditos = creditspre + " " + ExtraSettings.NomeMoedas + ", ";
								}

								string mensagemdiaria = (msgcreditos + msgduckets + msgdiamonds + msggotw);
								SendMessage(new RoomNotificationComposer("moedas", "message", "Você ganhou " + mensagemdiaria + " de bônus do prêmio diário!"));
							}
                        }
                        #endregion
                        GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(userData.user.GetClient(), "ACH_Login", 1);
                    }

                    ///Alert do loginstaff se tiver pin 0000

                    DataRow password = null;

                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT `pin` FROM users WHERE `id` = @Id LIMIT 1");
                        dbClient.AddParameter("Id", GetHabbo().Id);
                        password = dbClient.getRow();
                    }


                    if (ExtraSettings.STAFF_MENSG_ENTER)
                    {
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT name FROM `ranks` WHERE id = '" + GetHabbo().Rank + "'");
                            DataRow Table = dbClient.getRow();

                            if (GetHabbo().Rank == 1)
                            {

                            }
                            else
                            {
                                string figure = this.GetHabbo().Look;

                                GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, "O " + Convert.ToString(Table["name"]) + " " + userData.user.GetClient().GetHabbo().Username + " entrou no hotel!", ""));

                            }
                        }


                      
                            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("SELECT * FROM `users` WHERE id = '" + GetHabbo().Id + "'");
                                DataRow Table = dbClient.getRow();

                                if (Convert.ToString(Table["LalaConf"]) == "0")
                                {

                                }
                                else
                                {
                                    GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(userData.user.GetClient(), "ACH_TraderPass", 1);
                                    GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(userData.user.GetClient(), "ACH_AvatarTags", 1);
                                    GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(userData.user.GetClient(), "ACH_EmailVerification", 1);
                                }
                            }


                    

                        if (GetHabbo().isMedal)
                        {
                         
                            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE users SET isMedal = '0' WHERE id = " + GetHabbo().Id + ";");
                            }
                            GetHabbo().isMedal = false;

                        }

                        if (this.GetHabbo().VIPRank > 0)
                        {
                            SendMessage(new RoomNotificationComposer("fig/" + GetHabbo().Look, 3, "O usuário VIP '" + this.GetHabbo().Username + "' acabou de entrar!", ""));
                        }

                        GalaxyServer.GetGame().GetRewardManager().CheckRewards(this);
                        GalaxyServer.GetGame().GetAchievementManager().TryProgressHabboClubAchievements(this);
                        GalaxyServer.GetGame().GetAchievementManager().TryProgressRegistrationAchievements(this);
                        GalaxyServer.GetGame().GetAchievementManager().TryProgressLoginAchievements(this);
                        ICollection<MessengerBuddy> Friends = new List<MessengerBuddy>();
                        foreach (MessengerBuddy Buddy in GetHabbo().GetMessenger().GetFriends().ToList())
                        {
                            if (Buddy == null)
                                continue;

                            GameClient Friend = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(Buddy.Id);
                            if (Friend == null)
                                continue;

                            Friend.SendMessage(new RoomNotificationComposer("fig/" + GetHabbo().Look, 3, "Seu amigo '" + this.GetHabbo().Username + "' acabou de entrar!", ""));

                        }
                        return true;
                    }
                }
            }


            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
            }
			return false;
		}

        private object GetGame()
        {
            throw new NotImplementedException();
        }

        public void SendWhisper(string Message, int Colour = 0)
        {
            if (GetHabbo() == null || GetHabbo().CurrentRoom == null)
                return;

            RoomUser User = GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(GetHabbo().Username);
            if (User == null)
                return;

            SendMessage(new WhisperComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }

        public void SendNotification(string Message)
        {
            SendMessage(new BroadcastMessageAlertComposer(Message));
        }

        public void LogsNotif(string Message, string Key)
        {
            SendMessage(new RoomNotificationComposer(Message, Key));
        }

        public void SendMessage(IServerPacket Message)
        {
            byte[] bytes = Message.GetBytes();

            if (Message == null)
                return;

            if (GetConnection() == null)
                return;

            GetConnection().SendData(bytes);
        }

        public void SendShout(string Message, int Colour = 0)
        {
            if (this == null || GetHabbo() == null || GetHabbo().CurrentRoom == null)
                return;

            RoomUser User = GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(GetHabbo().Username);
            if (User == null)
                return;

            SendMessage(new ShoutComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }


        public int ConnectionID
        {
            get { return _id; }
        }

        public ConnectionInformation GetConnection()
        {
            return _connection;
        }

        public Habbo GetHabbo()
        {
            return _habbo;
        }

        public void Disconnect()
        {
            try
            {
                if (GetHabbo() != null)
                {
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery(GetHabbo().GetQueryString);
                    }


                    GetHabbo().OnDisconnect();


                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
            }


            if (!_disconnected)
            {
                if (_connection != null)
                    _connection.Dispose();
                _disconnected = true;
            }
        }

        public void Dispose()
        {
            CurrentRoomUserId = -1;

            if (GetHabbo() != null)
                GetHabbo().OnDisconnect();

            MachineId = string.Empty;
            _disconnected = true;
            _habbo = null;
            _connection = null;
            RC4Client = null;
            _packetParser = null;
        }


    }
}