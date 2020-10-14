using Galaxy.Communication.Packets.Outgoing.Handshake;
using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.LandingView;
using Galaxy.Communication.Packets.Outgoing.Navigator;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Achievements;
using Galaxy.HabboHotel.Catalog;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Groups;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Rooms.Chat.Commands;
using Galaxy.HabboHotel.Subscriptions;
using Galaxy.HabboHotel.Users.Badges;
using Galaxy.HabboHotel.Users.Clothing;
using Galaxy.HabboHotel.Users.Effects;
using Galaxy.HabboHotel.Users.Ignores;
using Galaxy.HabboHotel.Users.Inventory;
using Galaxy.HabboHotel.Users.Messenger;
using Galaxy.HabboHotel.Users.Messenger.FriendBar;
using Galaxy.HabboHotel.Users.Navigator.SavedSearches;
using Galaxy.HabboHotel.Users.Permissions;
using Galaxy.HabboHotel.Users.Polls;
using Galaxy.HabboHotel.Users.Process;
using Galaxy.HabboHotel.Users.Relationships;
using log4net;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Galaxy.HabboHotel.Users
{
    public class Habbo
    {
        private static readonly ILog log = LogManager.GetLogger("Galaxy.HabboHotel.Users");

        //Generic player values.
        public double StackHeight;
		public object SessionSocket;
        private int _id;
        private string _username;
        private int _rank;
        private string _motto;
        private string _look;
        private string _gender;
        private string _footballLook;
        private string _footballGender;
        private string _backupLook;
        private bool _lastMovFGate;
        private string _backupGender;
        private int _credits;
        private int _duckets;
        private int _diamonds;
        private int _gotwPoints;
        private int _homeRoom;
        private double _lastOnline;
        private double _accountCreated;
        private List<int> _clientVolume;
        private double _lastNameChange;
        private string _machineId;
        private bool _chatPreference;
        private bool _focusPreference;
        private bool _isExpert;
        private int _vipRank;
        private int _userpoints;
        public string chatColour = null;
        public int NuxClosedCount = 0;
        public string chatHTMLColour;
        public string _NamePrefix;
        public string _NamePrefixColor;
        public bool CHtmlColour;
        public string ssoTicket;
        private int _CurrentTalentLevel;
        private int _BonusPoints;
        public int UltimaVezWiredAfk;
		public int UltimoPremioAtividade = Convert.ToInt32(GalaxyServer.GetUnixTimestamp());
		public int LastEffect = 0;

		public int UltimoAbraco = 0;
		public int UltimoBeijo = 0;
		public int UltimaTroca = 0;
		public int UltimoComando = 0;
		public int UltimoExplodir = 0;
		public int UltimoMatar = 0;


		public int WiredCongelar = 0;

		public string RespostaMencao = "";

		//Abilitys triggered by generic events.
		private bool _allowSex;
        private bool _ViewInventory;
        public bool isNoob;
        public bool isMedal;
        public bool isEntra;
        public bool isOfficer = false;
		public bool isLoggedIn = false;
		public bool craftThiago = false;
		public bool StatusMencao = true;
        private bool _appearOffline;
        private bool _allowTradingRequests;
        private bool _allowUserFollowing;
        private bool _allowFriendRequests;
        private bool _allowMessengerInvites;
        private bool _allowPetSpeech;
        private bool _allowBotSpeech;
        private bool _allowPublicRoomStatus;
        private bool _allowConsoleMessages;
        private bool _allowGifts;
        private bool _allowMimic;
        private bool _allowEvents;
        private bool _receiveWhispers;
        private bool _ignorePublicWhispers;
        private bool _playingFastFood;
        private FriendBarState _friendbarState;
        private int _christmasDay;
        private int _wantsToRideHorse;
        private int _timeAFK;
        private bool _disableForcedEffects;
        public long _lastTimeUsedHelpCommand;
        private ClubManager ClubManager;
        public List<string> Tags;
        //Player saving.
        private bool _disconnected;
        private bool _habboSaved;
        private bool _changingName;

        //Counters
        private double _floodTime;
        private int _friendCount;
        private double _timeMuted;
        private double _tradingLockExpiry;
        private int _bannedPhraseCount;
        private double _sessionStart;
        private int _messengerSpamCount;
        private double _messengerSpamTime;
        private int _creditsTickUpdate;
        private int _bonusTickUpdate;
        public bool _NUX;
        internal bool Nuevo;
        public bool PassedNuxNavigator = false, PassedNuxDuckets = false, PassedNuxItems = false, PassedNuxChat = false, PassedNuxCatalog = false, PassedNuxMMenu = false, PassedNuxCredits = false;
        public bool BotFrank;
        public bool Chat1Passed = false, Chat2Passed = false, Chat3Passed = false, Chat4Passed = false, Chat5Passed = false, Chat6Passed = false;
        public byte _guidelevel;
        public byte _publicistalevel;
        public byte _builder;
        public byte _croupier;
        public byte _TargetedBuy;

        //Room related
        private int _tentId;
        private int _hopperId;
        private bool _isHopping;
        private int _teleportId;
        private bool _isTeleporting;
        private int _teleportingRoomId;
        private bool _roomAuthOk;
        private int _currentRoomId;

        //Advertising reporting system.
        private bool _hasSpoken;
        private bool _advertisingReported;
        private double _lastAdvertiseReport;
        private bool _advertisingReportBlocked;

        //Values generated within the game.
        private bool _wiredInteraction;
        private int _questLastCompleted;
        private bool _inventoryAlert;
        private bool _ignoreBobbaFilter;
        private bool _wiredTeleporting;
        private int _customBubbleId;
        private int _tempInt;
        private bool _onHelperDuty;
        internal string lastLayout;
        public bool isDeveloping = false;
        public int lastX;
        public int lastY;
        //Fastfood
        private int _fastfoodScore;

        //Just random fun stuff.
        private int _petId;

        //Anti-script placeholders.
        private DateTime _lastGiftPurchaseTime;
		private DateTime _lastPurchaseTime;
		private DateTime _lastRoomCreate;
		private DateTime _lastMottoUpdateTime;
        private DateTime _lastClothingUpdateTime;
        private DateTime _lastForumMessageUpdateTime;
        internal int lastsex;
        internal bool bolhastaff;
        private int _giftPurchasingWarnings;
        private int _mottoUpdateWarnings;
        private int _clothingUpdateWarnings;
        public int _bet;
        public bool FollowStatus;
        private bool _sessionGiftBlocked;
        private bool _sessionMottoBlocked;
        private bool _sessionClothingBlocked;

        public List<int> RatedRooms;
        public List<int> MutedUsers;
        public List<RoomData> UsersRooms;

        private GameClient _client;
        private HabboStats _habboStats;
        private HabboMessenger Messenger;
        public Dictionary<int, CatalogItem> _lastitems;
        private ProcessComponent _process;
        public ArrayList FavoriteRooms;
        public Dictionary<int, int> quests;
        private BadgeComponent BadgeComponent;
        private InventoryComponent InventoryComponent;
        public Dictionary<int, Relationship> Relationships;
        public ConcurrentDictionary<string, UserAchievement> Achievements;

        private DateTime _timeCached;

        private SearchesComponent _navigatorSearches;
        private EffectsComponent _fx;
        private ClothingComponent _clothing;
        private PermissionComponent _permissions;
        private IgnoresComponent _ignores;
        private PollsComponent _polls;
        public bool copy = false;
        private IChatCommand _iChatCommand;
        public bool IsCitizen => CurrentTalentLevel > 4;
        public string TalentStatus;

        public Habbo(int Id, string Username, int Rank, string Motto, string Look, string Gender, int Credits, int ActivityPoints, int HomeRoom,
            bool HasFriendRequestsDisabled, int LastOnline, bool AppearOffline, bool HideInRoom, double CreateDate, int Diamonds,
            string machineID, bool nux, bool nuevo, string clientVolume, bool ChatPreference, bool FocusPreference, bool PetsMuted, bool BotsMuted, bool AdvertisingReportBlocked, double LastNameChange,
            int GOTWPoints, int UserPoints, bool IgnoreInvites, double TimeMuted, double TradingLock, bool AllowGifts, int FriendBarState, bool DisableForcedEffects, bool AllowMimic, bool AllowEvents, bool AllowSex, int VIPRank, string chatHTMLcolour, string Chatcolour, string NamePrefix, string NamePrefixColor, int BubbleId, byte publicistalevel, byte guidelevel, byte builder, byte croupier, string citizenShip, byte TargetedBuy, int BonusPoints)
        {
            _id = Id;
            _username = Username;
            _rank = Rank;
            _motto = Motto;
            _look = Look;
            _gender = Gender.ToLower();
            _footballLook = GalaxyServer.FilterFigure(Look.ToLower());
            _footballGender = Gender.ToLower();
            _credits = Credits;
            _duckets = ActivityPoints;
            _diamonds = Diamonds;
            _gotwPoints = GOTWPoints;
            _userpoints = UserPoints;
            _homeRoom = HomeRoom;
            _allowSex = AllowSex;
            _lastOnline = LastOnline;
            _guidelevel = guidelevel;
            _publicistalevel = publicistalevel;
            _builder = builder;
            _croupier = croupier;
            _accountCreated = CreateDate;
            _clientVolume = new List<int>();
            _BonusPoints = BonusPoints;
            foreach (string Str in clientVolume.Split(','))
            {
                int Val = 0;
                if (int.TryParse(Str, out Val))
                    _clientVolume.Add(int.Parse(Str));
                else
                    _clientVolume.Add(100);
            }
            _ViewInventory = false;
            Tags = new List<string>();
            _lastNameChange = LastNameChange;
            _machineId = machineID;
            _NUX = nux;
            Nuevo = nuevo;
            _bet = 0;
            _chatPreference = ChatPreference;
            _focusPreference = FocusPreference;
            _isExpert = IsExpert == true;

            _appearOffline = AppearOffline;
            _allowTradingRequests = true;//TODO
            _allowUserFollowing = true;//TODO
            _allowFriendRequests = HasFriendRequestsDisabled;//TODO
            _allowMessengerInvites = IgnoreInvites;
            _allowPetSpeech = PetsMuted;
            _allowBotSpeech = BotsMuted;
            _allowPublicRoomStatus = HideInRoom;
            _allowConsoleMessages = true;
            _allowGifts = AllowGifts;
            _allowMimic = AllowMimic;
            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT allow_events FROM users WHERE id = '" + Id + "' LIMIT 1");

                if (Convert.ToBoolean(dbClient.getString()) == null)
                    _allowEvents = false;
                else
                _allowEvents = Convert.ToBoolean(dbClient.getString());
            }
            // _allowEvents = AllowEvents;
            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT status_mencao FROM users WHERE id = '" + Id + "' LIMIT 1");

                if (Convert.ToBoolean(dbClient.getString()) == null)
                    StatusMencao = false;
                else
                    StatusMencao = Convert.ToBoolean(dbClient.getString());
            }

            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT allow_follow FROM users WHERE id = '" + Id + "' LIMIT 1");

                if (Convert.ToBoolean(dbClient.getString()) == null)
                    FollowStatus = false;
                else
                    FollowStatus = Convert.ToBoolean(dbClient.getString());
            }
            _receiveWhispers = true;
            _ignorePublicWhispers = false;
            _playingFastFood = false;
            _friendbarState = FriendBarStateUtility.GetEnum(FriendBarState);
            _christmasDay = ChristmasDay;
            _wantsToRideHorse = 0;
            _timeAFK = 0;
            _disableForcedEffects = DisableForcedEffects;
            _vipRank = VIPRank;
            TalentStatus = citizenShip;
            _disconnected = false;
            _habboSaved = false;
            _changingName = false;

            _floodTime = 0;
            _friendCount = 0;
            _timeMuted = TimeMuted;
            _timeCached = DateTime.Now;

            _tradingLockExpiry = TradingLock;
            if (_tradingLockExpiry > 0 && GalaxyServer.GetUnixTimestamp() > TradingLockExpiry)
            {
                _tradingLockExpiry = 0;
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `user_info` SET `trading_locked` = '0' WHERE `user_id` = '" + Id + "' LIMIT 1");
                }
            }

            chatHTMLColour = chatHTMLcolour;
            chatColour = Chatcolour;
            _NamePrefix = NamePrefix;
            _NamePrefixColor = NamePrefixColor;

            _bannedPhraseCount = 0;
            _sessionStart = GalaxyServer.GetUnixTimestamp();
            _messengerSpamCount = 0;
            _messengerSpamTime = 0;
            _creditsTickUpdate = ExtraSettings.Intervalo;

            int isMedal;
            using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT isMedal FROM users WHERE id = '" + Id + "' LIMIT 1");
                isMedal = dbClient.getInteger();
            }
            this.isMedal = isMedal == 1;

            _tentId = 0;
            _hopperId = 0;
            _isHopping = false;
            _teleportId = 0;
            _isTeleporting = false;
            _teleportingRoomId = 0;
            _roomAuthOk = false;
            _currentRoomId = 0;
            _TargetedBuy = TargetedBuy;
            _hasSpoken = false;
            _lastAdvertiseReport = 0;
            _advertisingReported = false;
            _advertisingReportBlocked = AdvertisingReportBlocked;

            _wiredInteraction = false;
            _questLastCompleted = 0;
            _inventoryAlert = false;
            _ignoreBobbaFilter = false;
            _wiredTeleporting = false;
            bolhastaff = true;
            _customBubbleId = BubbleId;
            _onHelperDuty = false;
            _fastfoodScore = 0;
            _petId = 0;
            _tempInt = 0;

            _lastGiftPurchaseTime = DateTime.Now;
            _lastMottoUpdateTime = DateTime.Now;
            _lastClothingUpdateTime = DateTime.Now;
            _lastForumMessageUpdateTime = DateTime.Now;

            _giftPurchasingWarnings = 0;
            _mottoUpdateWarnings = 0;
            _clothingUpdateWarnings = 0;

			if (ExtraSettings.LoginStaff == false)
				isLoggedIn = true;

			_sessionGiftBlocked = false;
            _sessionMottoBlocked = false;
            _sessionClothingBlocked = false;

            FavoriteRooms = new ArrayList();
            MutedUsers = new List<int>();
            Achievements = new ConcurrentDictionary<string, UserAchievement>();
            Relationships = new Dictionary<int, Relationship>();
            RatedRooms = new List<int>();
            UsersRooms = new List<RoomData>();

            InitPermissions();

            #region Stats
            DataRow StatRow = null;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `id`,`roomvisits`,`onlinetime`,`respect`,`respectgiven`,`giftsgiven`,`giftsreceived`,`dailyrespectpoints`,`dailypetrespectpoints`,`achievementscore`,`quest_id`,`quest_progress`,`groupid`,`tickets_answered`,`respectstimestamp`,`forum_posts`, `PurchaseUsersConcurrent`  FROM `user_stats` WHERE `id` = @user_id LIMIT 1");
                dbClient.AddParameter("user_id", Id);
                StatRow = dbClient.getRow();

                if (StatRow == null)//No row, add it yo
                {
                    dbClient.runFastQuery("INSERT INTO `user_stats` (`id`) VALUES ('" + Id + "')");
                    dbClient.SetQuery("SELECT `id`,`roomvisits`,`onlinetime`,`respect`,`respectgiven`,`giftsgiven`,`giftsreceived`,`dailyrespectpoints`,`dailypetrespectpoints`,`achievementscore`,`quest_id`,`quest_progress`,`groupid`,`tickets_answered`,`respectstimestamp`,`forum_posts`, `PurchaseUsersConcurrent`  FROM `user_stats` WHERE `id` = @user_id LIMIT 1");
                    dbClient.AddParameter("user_id", Id);
                    StatRow = dbClient.getRow();
                }

                try
                {
                    _habboStats = new HabboStats(Convert.ToInt32(StatRow["id"]), Convert.ToInt32(StatRow["roomvisits"]), Convert.ToDouble(StatRow["onlineTime"]), Convert.ToInt32(StatRow["respect"]), Convert.ToInt32(StatRow["respectGiven"]), Convert.ToInt32(StatRow["giftsGiven"]),
                        Convert.ToInt32(StatRow["giftsReceived"]), Convert.ToInt32(StatRow["dailyRespectPoints"]), Convert.ToInt32(StatRow["dailyPetRespectPoints"]), Convert.ToInt32(StatRow["AchievementScore"]),
                        Convert.ToInt32(StatRow["quest_id"]), Convert.ToInt32(StatRow["quest_progress"]), Convert.ToInt32(StatRow["groupid"]), Convert.ToString(StatRow["respectsTimestamp"]), Convert.ToInt32(StatRow["forum_posts"]), Convert.ToBoolean(StatRow["PurchaseUsersConcurrent"]));

                    if (Convert.ToString(StatRow["respectsTimestamp"]) != DateTime.Today.ToString("MM/dd"))
                    {
                        _habboStats.RespectsTimestamp = DateTime.Today.ToString("MM/dd");
                        SubscriptionData SubData = null;

                        int DailyRespects = ExtraSettings.RespeitosUsers;

                        if (_permissions.HasRight("mod_tool"))
                            DailyRespects = ExtraSettings.RespeitosStaffs;

						if (Rank >= 16)
							DailyRespects = 9999999;

						if (Username.ToLower() == ExtraSettings.ReiniciarPermissao)
							DailyRespects = 9999999;

						_habboStats.DailyRespectPoints = DailyRespects;
                        _habboStats.DailyPetRespectPoints = DailyRespects;

                        dbClient.runFastQuery("UPDATE `user_stats` SET `dailyRespectPoints` = '" + DailyRespects + "', `dailyPetRespectPoints` = '" + DailyRespects + "', `respectsTimestamp` = '" + DateTime.Today.ToString("MM/dd") + "' WHERE `id` = '" + Id + "' LIMIT 1");
                    }
                }
                catch (Exception e)
                {
                    ExceptionLogger.LogException(e);
                }
            }

            Group G = null;
            if (!GalaxyServer.GetGame().GetGroupManager().TryGetGroup(_habboStats.FavouriteGroupId, out G))
                _habboStats.FavouriteGroupId = 0;
            #endregion
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        public string Motto
        {
            get { return _motto; }
            set { _motto = value; }
        }

        public string Look
        {
            get { return _look; }
            set { _look = value; }
        }

        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public string FootballLook
        {
            get { return _footballLook; }
            set { _footballLook = value; }
        }

        public string FootballGender
        {
            get { return _footballGender; }
            set { _footballGender = value; }
        }

        public bool LastMovFGate
        {
            get { return _lastMovFGate; }
            set { _lastMovFGate = value; }
        }

        public string BackupLook
        {
            get { return _backupLook; }
            set { _backupLook = value; }
        }

        public string BackupGender
        {
            get { return _backupGender; }
            set { _backupGender = value; }
        }

        public int Credits
        {
            get { return _credits; }
            set { _credits = value; }
        }

        public int Duckets
        {
            get { return _duckets; }
            set { _duckets = value; }
        }

        public int Diamonds
        {
            get { return _diamonds; }
            set { _diamonds = value; }
        }

        public int GOTWPoints
        {
            get { return _gotwPoints; }
            set { _gotwPoints = value; }
        }

        public int BonusPoints
        {
            get { return _BonusPoints; }
            set { _BonusPoints = value; }
        }

        public int UserPoints
        {
            get { return _userpoints; }
            set { _userpoints = value; }
        }

        public int HomeRoom
        {
            get { return _homeRoom; }
            set { _homeRoom = value; }
        }

        public double LastOnline
        {
            get { return _lastOnline; }
            set { _lastOnline = value; }
        }

        public double AccountCreated
        {
            get { return _accountCreated; }
            set { _accountCreated = value; }
        }

        public List<int> ClientVolume
        {
            get { return _clientVolume; }
            set { _clientVolume = value; }
        }

        public double LastNameChange
        {
            get { return _lastNameChange; }
            set { _lastNameChange = value; }
        }

        public string MachineId
        {
            get { return _machineId; }
            set { _machineId = value; }
        }

        public bool ChatPreference
        {
            get { return _chatPreference; }
            set { _chatPreference = value; }
        }
        public bool FocusPreference
        {
            get { return _focusPreference; }
            set { _focusPreference = value; }
        }

        public bool IsExpert
        {
            get { return _isExpert; }
            set { _isExpert = value; }
        }

        public bool AppearOffline
        {
            get { return _appearOffline; }
            set { _appearOffline = value; }
        }

        public int VIPRank
        {
            get { return _vipRank; }
            set { _vipRank = value; }
        }

        public int TempInt
        {
            get { return _tempInt; }
            set { _tempInt = value; }
        }

        public bool AllowTradingRequests
        {
            get { return _allowTradingRequests; }
            set { _allowTradingRequests = value; }
        }

        public bool AllowUserFollowing
        {
            get { return _allowUserFollowing; }
            set { _allowUserFollowing = value; }
        }

        public bool AllowFriendRequests
        {
            get { return _allowFriendRequests; }
            set { _allowFriendRequests = value; }
        }

        public bool AllowMessengerInvites
        {
            get { return _allowMessengerInvites; }
            set { _allowMessengerInvites = value; }
        }

        public bool AllowPetSpeech
        {
            get { return _allowPetSpeech; }
            set { _allowPetSpeech = value; }
        }

        public bool AllowBotSpeech
        {
            get { return _allowBotSpeech; }
            set { _allowBotSpeech = value; }
        }

        public bool AllowPublicRoomStatus
        {
            get { return _allowPublicRoomStatus; }
            set { _allowPublicRoomStatus = value; }
        }

        internal ClubManager GetClubManager()
        {
            return ClubManager;
        }

        public bool AllowConsoleMessages
        {
            get { return _allowConsoleMessages; }
            set { _allowConsoleMessages = value; }
        }

        public bool AllowGifts
        {
            get { return _allowGifts; }
            set { _allowGifts = value; }
        }

        public bool AllowMimic
        {
            get { return _allowMimic; }
            set { _allowMimic = value; }
        }
        public bool AllowEvents
        {
            get { return _allowEvents; }
            set { _allowEvents = value; }
        }

        public bool AllowSex
        {
            get { return _allowSex; }
            set { _allowSex = value; }
        }

        public bool ReceiveWhispers
        {
            get { return _receiveWhispers; }
            set { _receiveWhispers = value; }
        }

        public bool IgnorePublicWhispers
        {
            get { return _ignorePublicWhispers; }
            set { _ignorePublicWhispers = value; }
        }

        public bool PlayingFastFood
        {
            get { return _playingFastFood; }
            set { _playingFastFood = value; }
        }

        public FriendBarState FriendbarState
        {
            get { return _friendbarState; }
            set { _friendbarState = value; }
        }

        public int ChristmasDay
        {
            get { return _christmasDay; }
            set { _christmasDay = value; }
        }

        public int WantsToRideHorse
        {
            get { return _wantsToRideHorse; }
            set { _wantsToRideHorse = value; }
        }

        public int TimeAFK
        {
            get { return _timeAFK; }
            set { _timeAFK = value; }
        }

        public bool DisableForcedEffects
        {
            get { return _disableForcedEffects; }
            set { _disableForcedEffects = value; }
        }

        public bool ChangingName
        {
            get { return _changingName; }
            set { _changingName = value; }
        }

        public int FriendCount
        {
            get { return _friendCount; }
            set { _friendCount = value; }
        }

        public double FloodTime
        {
            get { return _floodTime; }
            set { _floodTime = value; }
        }

        public int BannedPhraseCount
        {
            get { return _bannedPhraseCount; }
            set { _bannedPhraseCount = value; }
        }

        public bool RoomAuthOk
        {
            get { return _roomAuthOk; }
            set { _roomAuthOk = value; }
        }

        public int CurrentRoomId
        {
            get { return _currentRoomId; }
            set { _currentRoomId = value; }
        }

        public int QuestLastCompleted
        {
            get { return _questLastCompleted; }
            set { _questLastCompleted = value; }
        }

        public int MessengerSpamCount
        {
            get { return _messengerSpamCount; }
            set { _messengerSpamCount = value; }
        }

        public double MessengerSpamTime
        {
            get { return _messengerSpamTime; }
            set { _messengerSpamTime = value; }
        }

        public double TimeMuted
        {
            get { return _timeMuted; }
            set { _timeMuted = value; }
        }

        public double TradingLockExpiry
        {
            get { return _tradingLockExpiry; }
            set { _tradingLockExpiry = value; }
        }

        public double SessionStart
        {
            get { return _sessionStart; }
            set { _sessionStart = value; }
        }

        public int TentId
        {
            get { return _tentId; }
            set { _tentId = value; }
        }

        public int HopperId
        {
            get { return _hopperId; }
            set { _hopperId = value; }
        }

        public bool IsHopping
        {
            get { return _isHopping; }
            set { _isHopping = value; }
        }

        public int TeleporterId
        {
            get { return _teleportId; }
            set { _teleportId = value; }
        }

        public bool IsTeleporting
        {
            get { return _isTeleporting; }
            set { _isTeleporting = value; }
        }

        public int TeleportingRoomID
        {
            get { return _teleportingRoomId; }
            set { _teleportingRoomId = value; }
        }

        public bool HasSpoken
        {
            get { return _hasSpoken; }
            set { _hasSpoken = value; }
        }

        public double LastAdvertiseReport
        {
            get { return _lastAdvertiseReport; }
            set { _lastAdvertiseReport = value; }
        }

        public bool AdvertisingReported
        {
            get { return _advertisingReported; }
            set { _advertisingReported = value; }
        }

        public bool AdvertisingReportedBlocked
        {
            get { return _advertisingReportBlocked; }
            set { _advertisingReportBlocked = value; }
        }

        public bool WiredInteraction
        {
            get { return _wiredInteraction; }
            set { _wiredInteraction = value; }
        }

        public bool InventoryAlert
        {
            get { return _inventoryAlert; }
            set { _inventoryAlert = value; }
        }

        public bool IgnoreBobbaFilter
        {
            get { return _ignoreBobbaFilter; }
            set { _ignoreBobbaFilter = value; }
        }

        public bool WiredTeleporting
        {
            get { return _wiredTeleporting; }
            set { _wiredTeleporting = value; }
        }

        public int CustomBubbleId
        {
            get { return _customBubbleId; }
            set { _customBubbleId = value; }
        }

        public bool OnHelperDuty
        {
            get { return _onHelperDuty; }
            set { _onHelperDuty = value; }
        }

        internal void UnNuevo()
        {
            if (!Nuevo)
                Nuevo = false;
        }

        public int FastfoodScore
        {
            get { return _fastfoodScore; }
            set { _fastfoodScore = value; }
        }

        public int PetId
        {
            get { return _petId; }
            set { _petId = value; }
        }

        public int CreditsUpdateTick
        {
            get { return _creditsTickUpdate; }
            set { _creditsTickUpdate = value; }
        }

        public int BonusUpdateTick
        {
            get { return _bonusTickUpdate; }
            set { _bonusTickUpdate = value; }
        }

        public IChatCommand IChatCommand
        {
            get { return _iChatCommand; }
            set { _iChatCommand = value; }
        }

		public DateTime LastGiftPurchaseTime
		{
			get { return _lastGiftPurchaseTime; }
			set { _lastGiftPurchaseTime = value; }
		}

		public DateTime LastPurchaseTime
		{
			get { return _lastPurchaseTime; }
			set { _lastPurchaseTime = value; }
		}

		public DateTime LastRoomCreate
		{
			get { return _lastRoomCreate; }
			set { _lastRoomCreate = value; }
		}

		public DateTime LastMottoUpdateTime
        {
            get { return _lastMottoUpdateTime; }
            set { _lastMottoUpdateTime = value; }
        }

        public DateTime LastClothingUpdateTime
        {
            get { return _lastClothingUpdateTime; }
            set { _lastClothingUpdateTime = value; }
        }

        public DateTime LastForumMessageUpdateTime
        {
            get { return _lastForumMessageUpdateTime; }
            set { _lastForumMessageUpdateTime = value; }
        }

        public int GiftPurchasingWarnings
        {
            get { return _giftPurchasingWarnings; }
            set { _giftPurchasingWarnings = value; }
        }

        public int MottoUpdateWarnings
        {
            get { return _mottoUpdateWarnings; }
            set { _mottoUpdateWarnings = value; }
        }

        public int ClothingUpdateWarnings
        {
            get { return _clothingUpdateWarnings; }
            set { _clothingUpdateWarnings = value; }
        }

        public int CurrentTalentLevel
        {
            get { return _CurrentTalentLevel; }
            set { _CurrentTalentLevel = value; }
        }

        public bool SessionGiftBlocked
        {
            get { return _sessionGiftBlocked; }
            set { _sessionGiftBlocked = value; }
        }

        public bool SessionMottoBlocked
        {
            get { return _sessionMottoBlocked; }
            set { _sessionMottoBlocked = value; }
        }

        public bool SessionClothingBlocked
        {
            get { return _sessionClothingBlocked; }
            set { _sessionClothingBlocked = value; }
        }

        public HabboStats GetStats()
        {
            return _habboStats;
        }

        public bool InRoom
        {
            get
            {
                return CurrentRoomId >= 1 && CurrentRoom != null;
            }
        }

        public Room CurrentRoom
        {
            get
            {
                if (CurrentRoomId <= 0)
                    return null;

				if (GalaxyServer.GetGame().GetRoomManager().TryGetRoom(CurrentRoomId, out Room _room))
					return _room;

				return null;
            }
        }

        public bool IsHelper
        {
            get { return TalentStatus == "helper" || Rank >= 4; }
        }


        public bool CacheExpired()
        {
            TimeSpan Span = DateTime.Now - _timeCached;
            return (Span.TotalMinutes >= 30);
        }

        public string GetQueryString
        {
            get
            {
                _habboSaved = true;
                return "UPDATE `users` SET `online` = '0', `last_online` = '" + GalaxyServer.GetUnixTimestamp() + "', `activity_points` = '" + Duckets + "', `credits` = '" + Credits + "', `vip_points` = '" + Diamonds + "', `bonus_points` = '" + BonusPoints + "', `home_room` = '" + HomeRoom + "', `gotw_points` = '" + GOTWPoints + "', `user_points` = '" + UserPoints + "', `publi` = '" + _publicistalevel + "', `guia` = '" + _guidelevel + "', `builder` = '" + _builder + "', `croupier` = '" + _croupier + "', `time_muted` = '" + TimeMuted + "',`friend_bar_state` = '" + FriendBarStateUtility.GetInt(_friendbarState) + "' WHERE id = '" + Id + "' LIMIT 1;UPDATE `user_stats` SET `roomvisits` = '" + _habboStats.RoomVisits + "', `onlineTime` = '" + (GalaxyServer.GetUnixTimestamp() - SessionStart + _habboStats.OnlineTime) + "', `respect` = '" + _habboStats.Respect + "', `respectGiven` = '" + _habboStats.RespectGiven + "', `giftsGiven` = '" + _habboStats.GiftsGiven + "', `giftsReceived` = '" + _habboStats.GiftsReceived + "', `dailyRespectPoints` = '" + _habboStats.DailyRespectPoints + "', `dailyPetRespectPoints` = '" + _habboStats.DailyPetRespectPoints + "', `AchievementScore` = '" + _habboStats.AchievementPoints + "', `quest_id` = '" + _habboStats.QuestID + "', `quest_progress` = '" + _habboStats.QuestProgress + "', `groupid` = '" + _habboStats.FavouriteGroupId + "',`forum_posts` = '" + _habboStats.ForumPosts + "', `PurchaseUsersConcurrent` = '" + _habboStats.PurchaseUsersConcurrent + "'  WHERE `id` = '" + Id + "' LIMIT 1;";
            }
        }

        public bool InitProcess()
        {
            _process = new ProcessComponent();
            if (_process.Init(this))
                return true;
            return false;
        }

        public bool InitSearches()
        {
            _navigatorSearches = new SearchesComponent();
            if (_navigatorSearches.Init(this))
                return true;
            return false;
        }

        public bool InitFX()
        {
            _fx = new EffectsComponent();
            if (_fx.Init(this))
                return true;
            return false;
        }

        public bool InitClothing()
        {
            _clothing = new ClothingComponent();
            if (_clothing.Init(this))
                return true;
            return false;
        }

        public bool InitIgnores()
        {
            _ignores = new IgnoresComponent();

            return _ignores.Init(this);
        }

        private bool InitPolls()
        {
            _polls = new PollsComponent();

            return _polls.Init(this);
        }

        private bool InitPermissions()
        {
            _permissions = new PermissionComponent();
            if (_permissions.Init(this))
                return true;
            return false;
        }



        public IgnoresComponent GetIgnores()
        {
            return _ignores;
        }

        public void InitInformation(UserData.UserData data)
        {
            BadgeComponent = new BadgeComponent(this, data);
            Relationships = data.Relations;
        }

       public void Init(GameClient client, UserData.UserData data)
        {
            Achievements = data.achievements;

            FavoriteRooms = new ArrayList();
            foreach (int id in data.favouritedRooms)
            {
                FavoriteRooms.Add(id);
            }

            _client = client;
            BadgeComponent = new BadgeComponent(this, data);
            InventoryComponent = new InventoryComponent(Id, client);

            quests = data.quests;

            Messenger = new HabboMessenger(Id);
            Messenger.Init(data.friends, data.requests);
            _friendCount = Convert.ToInt32(data.friends.Count);
            _disconnected = false;
            UsersRooms = data.rooms;
            Relationships = data.Relations;

            InitSearches();
            InitFX();
            InitClothing();
            InitIgnores();
            InitPolls();
            LoadTags(data.Tags);
            ClubManager = new ClubManager(Id, data);
        }

        public void LoadTags(List<string> tags)
        {
            Tags = tags;
        }

        public PermissionComponent GetPermissions()
        {
            return _permissions;
        }

        public PollsComponent GetPolls()
        {
            return _polls;
        }

        public void OnDisconnect()
        {
            if (_disconnected)
                return;

            try
            {
                if (_process != null)
                    _process.Dispose();
            }
            catch { }

            _disconnected = true;

            if (ClubManager != null)
            {
                ClubManager.Clear();
                ClubManager = null;
            }

            GalaxyServer.GetGame().GetClientManager().UnregisterClient(Id, Username);

            if (!_habboSaved)
            {
                _habboSaved = true;
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `users` SET `online` = '0', `last_online` = '" + GalaxyServer.GetUnixTimestamp() + "', `activity_points` = '" + Duckets + "', `credits` = '" + Credits + "', `vip_points` = '" + Diamonds + "', `bonus_points` = '" + BonusPoints + "', `home_room` = '" + HomeRoom + "', `gotw_points` = '" + GOTWPoints + "', `user_points` = '" + UserPoints + "', `time_muted` = '" + TimeMuted + "',`friend_bar_state` = '" + FriendBarStateUtility.GetInt(_friendbarState) + "' WHERE id = '" + Id + "' LIMIT 1;UPDATE `user_stats` SET `roomvisits` = '" + _habboStats.RoomVisits + "', `onlineTime` = '" + (GalaxyServer.GetUnixTimestamp() - SessionStart + _habboStats.OnlineTime) + "', `respect` = '" + _habboStats.Respect + "', `respectGiven` = '" + _habboStats.RespectGiven + "', `giftsGiven` = '" + _habboStats.GiftsGiven + "', `giftsReceived` = '" + _habboStats.GiftsReceived + "', `dailyRespectPoints` = '" + _habboStats.DailyRespectPoints + "', `dailyPetRespectPoints` = '" + _habboStats.DailyPetRespectPoints + "', `AchievementScore` = '" + _habboStats.AchievementPoints + "', `quest_id` = '" + _habboStats.QuestID + "', `quest_progress` = '" + _habboStats.QuestProgress + "', `groupid` = '" + _habboStats.FavouriteGroupId + "',`forum_posts` = '" + _habboStats.ForumPosts + "', `PurchaseUsersConcurrent` = '" + _habboStats.PurchaseUsersConcurrent + "'  WHERE `id` = '" + Id + "' LIMIT 1;");
                    if(Rank > 1)
                    dbClient.runFastQuery("INSERT INTO `client_staff_quit` (`user`, `date`) VALUES ('" + Id + "', '" + GalaxyServer.GetUnixTimestamp() + "');");
                    if (GetPermissions().HasRight("mod_tickets"))
                        dbClient.runFastQuery("UPDATE `moderation_tickets` SET `status` = 'open', `moderator_id` = '0' WHERE `status` ='picked' AND `moderator_id` = '" + Id + "'");
                }
            }

            Dispose();

            _client = null;

        }

        public void Dispose()
        {
            if (InventoryComponent != null)
                InventoryComponent.SetIdleState();

            if (UsersRooms != null)
                UsersRooms.Clear();

            if (InRoom && CurrentRoom != null)
                CurrentRoom.GetRoomUserManager().RemoveUserFromRoom(_client, false, false);

            if (Messenger != null)
            {
                Messenger.AppearOffline = true;
                Messenger.Destroy();
            }

            if (_fx != null)
                _fx.Dispose();

            if (_clothing != null)
                _clothing.Dispose();

            if (_permissions != null)
                _permissions.Dispose();

            if (_ignores != null)
                _ignores.Dispose();
        }

		public void CheckCreditsTimer()
		{
			try
			{
				CreditsUpdateTick--;

				if (CreditsUpdateTick <= 0)
				{

					DataRow moedasRank = null;
					using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
					{
						dbClient.SetQuery("SELECT * FROM ranks WHERE id = '" + _client.GetHabbo().Rank + "'");
						moedasRank = dbClient.getRow();
					}

					int CreditUpdate = Convert.ToInt32(moedasRank["moedas_tempo"]);
					int DucketUpdate = Convert.ToInt32(moedasRank["duckets_tempo"]);
					int DiamondUpdate = Convert.ToInt32(moedasRank["diamantes_tempo"]);
					int GOTWUpdate = Convert.ToInt32(moedasRank["gotw_tempo"]);

					Credits += CreditUpdate;
					Duckets += DucketUpdate;
					Diamonds += DiamondUpdate;
					GOTWPoints += GOTWUpdate;

					_client.SendMessage(new CreditBalanceComposer(Credits));
					_client.SendMessage(new HabboActivityPointNotificationComposer(Duckets, DucketUpdate));
					_client.SendMessage(new HabboActivityPointNotificationComposer(_client.GetHabbo().Diamonds, 0, 5));
					_client.SendMessage(new HabboActivityPointNotificationComposer(_client.GetHabbo().GOTWPoints, GOTWUpdate, 103));

					CreditsUpdateTick = ExtraSettings.Intervalo;
				}
			}
			catch
			{
			}
		}

		/*public void CheckCreditsTimer()
		{
			try
			{
				this._creditsTickUpdate--;

				if (this._creditsTickUpdate <= 0)
				{

					if (ExtraSettings.Intervalo == 0)
						return;

					if (ExtraSettings.PremioPorAtividade == false)
						return;


					if (_client.GetHabbo().UltimoPremioAtividade > 0)
					{
						int IntervaloSegundo = (ExtraSettings.Intervalo * 60);
						int TimeStamp = Convert.ToInt32(GalaxyServer.GetUnixTimestamp());
						int UltimoPremio = _client.GetHabbo().UltimoPremioAtividade;

						if ((UltimoPremio + IntervaloSegundo) == TimeStamp)
						{
							/// ok
							/// phb <3
							/// 
						}
						else
						{
							return;
						}


					}


					_client.GetHabbo().UltimoPremioAtividade = Convert.ToInt32(GalaxyServer.GetUnixTimestamp());

					DataRow moedasRank = null;
					using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
					{
						dbClient.SetQuery("SELECT * FROM ranks WHERE id = '" + _client.GetHabbo().Rank + "'");
						moedasRank = dbClient.getRow();
					}

					int CreditUpdate = Convert.ToInt32(moedasRank["moedas_tempo"]);
					int DucketUpdate = Convert.ToInt32(moedasRank["duckets_tempo"]);
					int DiamondUpdate = Convert.ToInt32(moedasRank["diamantes_tempo"]);
					int GOTWUpdate = Convert.ToInt32(moedasRank["gotw_tempo"]);


					bool flag2 = ExtraSettings.PremioPorAtividade;

					if (flag2)
					{
						if (GalaxyServer.GetGame().GetSubscriptionManager().TryGetSubscriptionData(_vipRank, out SubscriptionData SubData))
						{
							CreditUpdate += SubData.Credits;
							DucketUpdate += SubData.Duckets;
							DiamondUpdate += SubData.Diamonds;
						}

						_credits += CreditUpdate;
						_duckets += DucketUpdate;
						_diamonds += DiamondUpdate;


						_client.SendMessage(new CreditBalanceComposer(_credits));
						_client.SendMessage(new ActivityPointsComposer(_duckets, _diamonds, _gotwPoints));

						GetClient().SendMessage(RoomNotificationComposer.SendBubble("command_notification_credits", "Você acaba de receber " + CreditUpdate + " " + ExtraSettings.NomeMoedas + ", " + DiamondUpdate + " " + ExtraSettings.NomeDiamantes + " e " + DucketUpdate + " " + ExtraSettings.NomeDuckets + " do bônus horário!", ""));

						CreditsUpdateTick = ExtraSettings.Intervalo;


						if (GOTWUpdate > 0)
						{
							_client.GetHabbo().GOTWPoints = _client.GetHabbo().GOTWPoints + GOTWUpdate;
							_client.SendMessage(new HabboActivityPointNotificationComposer(_client.GetHabbo().GOTWPoints, GOTWUpdate, 103));
							_client.SendMessage(RoomNotificationComposer.SendBubble("moedas", "Você recebeu " + GOTWUpdate + " " + Core.ExtraSettings.NomeGotw.ToLower() + ".\n", "catalog/open/gotws"));
						}
					}
				}
			}
			catch { }
        }
		*/
        public GameClient GetClient()
        {    
            if (_client != null)
                return _client;

            return GalaxyServer.GetGame().GetClientManager().GetClientByUserID(Id);
        }

        public HabboMessenger GetMessenger()
        {
            return Messenger;
        }

        public BadgeComponent GetBadgeComponent()
        {
            return BadgeComponent;
        }

        public InventoryComponent GetInventoryComponent()
        {
            return InventoryComponent;
        }

        public SearchesComponent GetNavigatorSearches()
        {
            return _navigatorSearches;
        }

        public EffectsComponent Effects()
        {
            return _fx;
        }

        public ClothingComponent GetClothing()
        {
            return _clothing;
        }

        public int GetQuestProgress(int p)
        {
			quests.TryGetValue(p, out int progress);
			return progress;
        }

        public UserAchievement GetAchievementData(string p)
        {
            UserAchievement achievement = null;
            Achievements.TryGetValue(p, out achievement);
            return achievement;
        }

        public void ChangeName(string Username)
        {
            this.LastNameChange = GalaxyServer.GetUnixTimestamp();
            this.Username = Username;

            SaveKey("username", Username);
            SaveKey("last_change", LastNameChange.ToString());
        }

        public void SaveKey(string Key, string Value)
        {
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET " + Key + " = @value WHERE `id` = '" + this.Id + "' LIMIT 1;");
                dbClient.AddParameter("value", Value);
                dbClient.RunQuery();
            }
        }

        public void PrepareRoom(int Id, string Password)
        {
            if (GetClient() == null || GetClient().GetHabbo() == null)
                return;

			if (GetClient().GetHabbo().InRoom)
            {
				if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(GetClient().GetHabbo().CurrentRoomId, out Room OldRoom))
					return;

				if (OldRoom.GetRoomUserManager() != null)
                    OldRoom.GetRoomUserManager().RemoveUserFromRoom(GetClient(), false, false);
            }

            if (GetClient().GetHabbo().IsTeleporting && GetClient().GetHabbo().TeleportingRoomID != Id)
            {
                GetClient().SendMessage(new CloseConnectionComposer());
                return;
            }

			Room Room = GalaxyServer.GetGame().GetRoomManager().LoadRoom(Id);
            if (Room == null)
            {
                GetClient().SendMessage(new CloseConnectionComposer());
                return;
            }

            if (Room.isCrashed)
            {
                GetClient().SendNotification("Quarto indisponível! procure um administrador.");
                GetClient().SendMessage(new CloseConnectionComposer());
                return;
            }

            GetClient().GetHabbo().CurrentRoomId = Room.RoomId;

			if (Room.GetRoomUserManager().userCount >= Room.UsersMax && !GetClient().GetHabbo().GetPermissions().HasRight("room_enter_full") && GetClient().GetHabbo().Id != Room.OwnerId)
            {
                GetClient().SendMessage(new CantConnectComposer(1));
                GetClient().SendMessage(new CloseConnectionComposer());
                return;
            }

            if (!GetPermissions().HasRight("room_ban_override") && Room.GetBans().IsBanned(this.Id))
            {
                RoomAuthOk = false;
                GetClient().GetHabbo().RoomAuthOk = false;
                GetClient().SendMessage(new CantConnectComposer(4));
                GetClient().SendMessage(new CloseConnectionComposer());
                return;
            }

            GetClient().SendMessage(new OpenConnectionComposer());


			if (!Room.CheckRights(this.GetClient(), true, true) && !this.GetClient().GetHabbo().IsTeleporting && !this.GetClient().GetHabbo().IsHopping)
            {
                if (Room.Access == RoomAccess.DOORBELL && !this.GetClient().GetHabbo().GetPermissions().HasRight("room_enter_locked"))
                {
                    if (Room.UserCount > 0)
                    {
                        this.GetClient().SendMessage(new DoorbellComposer(""));
                        Room.SendMessage(new DoorbellComposer(this.GetClient().GetHabbo().Username), true);
                        return;
                    }
                    else
                    {
                        this.GetClient().SendMessage(new FlatAccessDeniedComposer(""));
                        this.GetClient().SendMessage(new CloseConnectionComposer(this.GetClient()));
                        return;
                    }
                }
                else if (Room.Access == RoomAccess.PASSWORD && !this.GetClient().GetHabbo().GetPermissions().HasRight("room_enter_locked"))
                {
                    if (Password.ToLower() != Room.Password.ToLower() || String.IsNullOrWhiteSpace(Password))
                    {
                        this.GetClient().SendMessage(new GenericErrorComposer(-100002));
                        this.GetClient().SendMessage(new CloseConnectionComposer(this.GetClient()));
                        return;
                    }
                }
            }


			if (!EnterRoom(Room))
                this.GetClient().SendMessage(new CloseConnectionComposer(this.GetClient()));

		}

		public bool ViewInventory
        {
            get { return _ViewInventory; }
            set { _ViewInventory = value; }
        }

        public bool EnterRoom(Room Room)
        {
            if (Room == null)
                GetClient().SendMessage(new CloseConnectionComposer());

            GetClient().SendMessage(new RoomReadyComposer(Room.RoomId, Room.ModelName));
            if (Room.Wallpaper != "0.0")
                GetClient().SendMessage(new RoomPropertyComposer("wallpaper", Room.Wallpaper));
            if (Room.Floor != "0.0")
                GetClient().SendMessage(new RoomPropertyComposer("floor", Room.Floor));

            GetClient().SendMessage(new RoomPropertyComposer("landscape", Room.Landscape));
            GetClient().SendMessage(new RoomRatingComposer(Room.Score, !(GetClient().GetHabbo().RatedRooms.Contains(Room.RoomId) || Room.OwnerId == GetClient().GetHabbo().Id)));

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("INSERT INTO user_roomvisits (user_id,room_id,entry_timestamp,exit_timestamp,hour,minute) VALUES ('" + GetClient().GetHabbo().Id + "','" + GetClient().GetHabbo().CurrentRoomId + "','" + GalaxyServer.GetUnixTimestamp() + "','0','" + DateTime.Now.Hour + "','" + DateTime.Now.Minute + "');");// +
            }

			/// Atualiza informações de moedas do banco de dados
			using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
			{
				dbClient.runFastQuery("UPDATE `users` SET `activity_points` = '" + GetClient().GetHabbo().Duckets + "', `credits` = '" + GetClient().GetHabbo().Credits + "', `vip_points` = '" + GetClient().GetHabbo().Diamonds + "', `gotw_points` = '" + GetClient().GetHabbo().GOTWPoints + "' WHERE id = '"+ GetClient().GetHabbo().Id + "' LIMIT 1");
			}


			if (Room.OwnerId != Id)
            {
                GetClient().GetHabbo().GetStats().RoomVisits += 1;
                GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(GetClient(), "ACH_RoomEntry", 1);
            }
            return true;
        }
    }
    enum TypeOfHelper
    {
        None,
        Guide,
        Alpha,
        Guardian
    }
}