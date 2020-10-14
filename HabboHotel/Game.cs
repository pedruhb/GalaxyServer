using Galaxy.Communication.Packets;
using Galaxy.Communication.Packets.Incoming.LandingView;
using Galaxy.Core;
using Galaxy.Core.FigureData;
using Galaxy.Core.Language;
using Galaxy.Core.Settings;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Achievements;
using Galaxy.HabboHotel.Badges;
using Galaxy.HabboHotel.Bots;
using Galaxy.HabboHotel.Cache;
using Galaxy.HabboHotel.Catalog;
using Galaxy.HabboHotel.Catalog.FurniMatic;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Games;
using Galaxy.HabboHotel.Groups;
using Galaxy.HabboHotel.Groups.Forums;
using Galaxy.HabboHotel.Helpers;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Items.Crafting;
using Galaxy.HabboHotel.Items.RentableSpaces;
using Galaxy.HabboHotel.Items.Televisions;
using Galaxy.HabboHotel.LandingView;
using Galaxy.HabboHotel.Moderation;
using Galaxy.HabboHotel.Navigator;
using Galaxy.HabboHotel.Permissions;
using Galaxy.HabboHotel.Quests;
using Galaxy.HabboHotel.Rewards;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Rooms.Camera;
using Galaxy.HabboHotel.Rooms.Chat;
using Galaxy.HabboHotel.Rooms.Polls;
using Galaxy.HabboHotel.Rooms.TraxMachine;
using Galaxy.HabboHotel.Subscriptions;
using log4net;
using System;
using System.Threading;

namespace Galaxy.HabboHotel
{
    public class Game
    {
        private static readonly ILog log = LogManager.GetLogger("Galaxy.HabboHotel.Game");
        internal bool ClientManagerCycleEnded, RoomManagerCycleEnded;
        private readonly PacketManager _packetManager;
        private readonly GameClientManager _clientManager;
        private readonly ModerationManager _moderationManager;
        private readonly ItemDataManager _itemDataManager;
        private readonly CatalogManager _catalogManager;
        private readonly TelevisionManager _televisionManager;
        private readonly NavigatorManager _navigatorManager;
        private readonly RoomManager _roomManager;
        private readonly ChatManager _chatManager;
        private readonly GroupManager _groupManager;
        private readonly GroupForumManager _groupForumManager;
        private readonly QuestManager _questManager;
        private readonly AchievementManager _achievementManager;
        private readonly LandingViewManager _landingViewManager;
        private readonly GameDataManager _gameDataManager;
        private readonly BotManager _botManager;
        private readonly CacheManager _cacheManager;
        private readonly RewardManager _rewardManager;
        private readonly BadgeManager _badgeManager;
        private readonly PermissionManager _permissionManager;
        private readonly SubscriptionManager _subscriptionManager;
        private readonly TargetedOffersManager _targetedoffersManager;
        private readonly CrackableManager _crackableManager;
        private readonly FurniMaticRewardsManager _furniMaticRewardsManager;
        private readonly FigureDataManager _figureManager;
        private readonly LanguageManager _languageManager;
        private readonly SettingsManager _settingsManager;
        private readonly CraftingManager _craftingManager;
		private readonly PollManager _pollManager;
        private RentableSpaceManager _rentableSpaceManager;

        private Thread _gameLoop;
        internal static bool GameLoopEnabled = true;
        public static int SessionUserRecord;
        internal bool GameLoopActiveExt { get; private set; }

        public Game()
        {
            log.Info("» Iniciando o " + GalaxyServer.VersionGalaxy + " para o " + GalaxyServer.HotelName + " Hotel!");

            SessionUserRecord = 0;
            // Run Extra Settings
           // BotFrankConfig.RunBotFrank();
            ExtraSettings.RunExtraSettings();

            // Run Catalog Settings
            CatalogSettings.RunCatalogSettings();

            // Run Notification Settings
            NotificationSettings.RunNotiSettings();

            _languageManager = new LanguageManager();
            _languageManager.Init();

            _rentableSpaceManager = new RentableSpaceManager();

            _settingsManager = new SettingsManager();
            _settingsManager.Init();

            _packetManager = new PacketManager();
            _clientManager = new GameClientManager();

            _moderationManager = new ModerationManager();
            _moderationManager.Init();

            _itemDataManager = new ItemDataManager();
            _itemDataManager.Init();

            _catalogManager = new CatalogManager();
            _catalogManager.Init(_itemDataManager);

            _craftingManager = new CraftingManager();
            _craftingManager.Init();

            _televisionManager = new TelevisionManager();

            _navigatorManager = new NavigatorManager();
            _roomManager = new RoomManager();
            _chatManager = new ChatManager();
            _groupManager = new GroupManager();
            _groupManager.Init();
            _groupForumManager = new GroupForumManager();
            _questManager = new QuestManager();
            _achievementManager = new AchievementManager();
            _landingViewManager = new LandingViewManager();
            _gameDataManager = new GameDataManager();

            _botManager = new BotManager();
           
            _cacheManager = new CacheManager();
            _rewardManager = new RewardManager();

            _badgeManager = new BadgeManager();
            _badgeManager.Init();

            _permissionManager = new PermissionManager();
            _permissionManager.Init();

            _subscriptionManager = new SubscriptionManager();
            _subscriptionManager.Init();

            TraxSoundManager.Init();
            HabboCameraManager.Init();
            HelperToolsManager.Init();

            _figureManager = new FigureDataManager(GalaxyServer.GetConfig().data["game.legacy.figure_mutant"].ToString() == "0");
            _figureManager.Init();

            _crackableManager = new CrackableManager();
            _crackableManager.Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());

            _furniMaticRewardsManager = new FurniMaticRewardsManager();
            _furniMaticRewardsManager.Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());

            _targetedoffersManager = new TargetedOffersManager();
            _targetedoffersManager.Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());

		}

        public void ContinueLoading()
        {
            ServerStatusUpdater.Init();
			StartGameLoop();
        }

        public void StartGameLoop()
        {
            GameLoopActiveExt = true;
			_gameLoop = new Thread(GameCycle)
			{
				Name = "Game Loop"
			};
			_gameLoop.Start();

        }

        public PollManager GetPollManager()
        {
            return _pollManager;
        }

        private void GameCycle()
        {
            ServerStatusUpdater.StartProcessing();

            while (GameLoopActiveExt)
            {
                if (GameLoopEnabled)
                    try
                    {
                        RoomManagerCycleEnded = false;
                        ClientManagerCycleEnded = false;
                        _roomManager.OnCycle();
                        _clientManager.OnCycle();
                    }
                    catch (Exception ex)
                    {
                        ExceptionLogger.LogCriticalException(ex);
                    }
                Thread.Sleep(25);
            }
        }

        public void StopGameLoop()
        {
            GameLoopActiveExt = false;
            while (!RoomManagerCycleEnded || !ClientManagerCycleEnded) Thread.Sleep(25);
        }

        public static void DatabaseCleanup(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("UPDATE `users` SET `online` = '0' WHERE `online` = '1'");
            dbClient.runFastQuery("UPDATE `rooms` SET `users_now` = 0 WHERE `users_now` > '0'");
            dbClient.runFastQuery("UPDATE `server_status` SET `status` = '1', `users_online` = '0', `loaded_rooms` = '0'");
        }

        public void Destroy()
        {
            using (var queryReactor = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                DatabaseCleanup(queryReactor);
            GetClientManager();
            log.WarnFormat("GalaxyServer GameManager encerrado.", "Galaxy.HabboHotel.Game", ConsoleColor.DarkYellow);
        }


        public PacketManager GetPacketManager()
        {
            return _packetManager;
        }

        public GameClientManager GetClientManager()
        {
            return _clientManager;
        }

        public CatalogManager GetCatalog()
        {
            return _catalogManager;
        }

        public NavigatorManager GetNavigator()
        {
            return _navigatorManager;
        }

        public ItemDataManager GetItemManager()
        {
            return _itemDataManager;
        }

        public RoomManager GetRoomManager()
        {
            return _roomManager;
        }

        public RentableSpaceManager GetRentableSpaceManager()
        {
            return _rentableSpaceManager;
        }

        internal TargetedOffersManager GetTargetedOffersManager()
        {
            return _targetedoffersManager;
        }

        public AchievementManager GetAchievementManager()
        {
            return _achievementManager;
        }

        public ModerationManager GetModerationManager()
        {
            return _moderationManager;
        }

        public PermissionManager GetPermissionManager()
        {
            return _permissionManager;
        }

        public SubscriptionManager GetSubscriptionManager()
        {
            return _subscriptionManager;
        }

        public QuestManager GetQuestManager()
        {
            return _questManager;
        }

        public GroupManager GetGroupManager()
        {
            return _groupManager;
        }

        public GroupForumManager GetGroupForumManager()
        {
            return _groupForumManager;
        }

        public LandingViewManager GetLandingManager()
        {
            return _landingViewManager;
        }
        public TelevisionManager GetTelevisionManager()
        {
            return _televisionManager;
        }

        public ChatManager GetChatManager()
        {
            return _chatManager;
        }

        public FurniMaticRewardsManager GetFurniMaticRewardsMnager()
        {
            return _furniMaticRewardsManager;
        }

        internal CrackableManager GetPinataManager()
        {
            return _crackableManager;
        }

        public GameDataManager GetGameDataManager()
        {
            return _gameDataManager;
        }

        public BotManager GetBotManager()
        {
            return _botManager;
        }

        public CacheManager GetCacheManager()
        {
            return _cacheManager;
        }

        public LanguageManager GetLanguageManager()
        {
            return _languageManager;
        }

        public SettingsManager GetSettingsManager()
        {
            return _settingsManager;
        }

        public CraftingManager GetCraftingManager()
        {
            return _craftingManager;
        }

        public FigureDataManager GetFigureManager()
        {
            return _figureManager;
        }

        public RewardManager GetRewardManager()
        {
            return _rewardManager;
        }

        public BadgeManager GetBadgeManager()
        {
            return _badgeManager;
        }
    }
}