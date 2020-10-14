using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Items.Wired;
using Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands
{
	public class CommandManager
	{
		private string _prefix = ":";
		private readonly Dictionary<string, IChatCommand> _commands;
		public CommandManager(string Prefix)
		{
			this._prefix = Prefix;
			this._commands = new Dictionary<string, IChatCommand>();
			this.RegisterCommands();
		}

		public bool Parse(GameClient Session, string Message)
		{
			if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().CurrentRoom == null)
				return false;

			if (!Message.StartsWith(_prefix))
				return false;

			if (Message == _prefix + "comandos")
			{
				StringBuilder List = new StringBuilder();
				List.Append("- Comandos disponíveis no " + GalaxyServer.HotelName + " Hotel -\n\n");
				int contadorComandos = 0;
				foreach (var CmdList in _commands.ToList())
				{
					if (!string.IsNullOrEmpty(CmdList.Value.PermissionRequired))
					{
						if (!Session.GetHabbo().GetPermissions().HasCommand(CmdList.Value.PermissionRequired))
							continue;
					}

					List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " >> " + CmdList.Value.Description + "\n");
					contadorComandos++;
				}
				List.Append("\nTotal de comandos disponíveis: " + contadorComandos);

				Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
				return true;
			}

			Message = Message.Substring(1);
			string[] Split = Message.Split(' ');

			if (Split.Length == 0)
				return false;

			if (Session.GetHabbo().Rank == 1)
			{
				this.LogCommand(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId);
			}


			if (_commands.TryGetValue(Split[0].ToLower(), out IChatCommand Cmd))
			{

				if (Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
					this.LogCommandStaff(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId, Session.GetHabbo().Username);

				if (!string.IsNullOrEmpty(Cmd.PermissionRequired))
				{
					if (!Session.GetHabbo().GetPermissions().HasCommand(Cmd.PermissionRequired))
						return false;
				}

				/// Verifica se o usuário pode usar o comando no quarto
				if (Session.GetHabbo().Rank < 5)
				{
					DataRow BlockCMD = null;
					using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
					{
						dbClient.SetQuery("SELECT id FROM room_blockcmd WHERE room = @room AND command = @command LIMIT 1");
						dbClient.AddParameter("command", Split[0].ToLower());
						dbClient.AddParameter("room", Session.GetHabbo().CurrentRoomId);
						BlockCMD = dbClient.getRow();
						if (BlockCMD != null)
						{
							Session.SendWhisper("Você não pode usar esse comando nesse quarto.");
							return false;
						}
					}
				}

				/// Cooldown de comandos do hotel.
				if (Session.GetHabbo().UltimoComando > 0)
				{
					if ((System.Convert.ToInt32(GalaxyServer.GetIUnixTimestamp()) - Session.GetHabbo().UltimoComando) <= 5 && Session.GetHabbo().Rank < 3)
					{
						Session.SendWhisper("Espere alguns segundos para usar um comando novamente.");
						return true;
					}
				}

				Session.GetHabbo().IChatCommand = Cmd;
				Session.GetHabbo().CurrentRoom.GetWired().TriggerEvent(WiredBoxType.TriggerUserSaysCommand, Session.GetHabbo(), this);

				Cmd.Execute(Session, Session.GetHabbo().CurrentRoom, Split);
				Session.GetHabbo().UltimoComando = System.Convert.ToInt32(GalaxyServer.GetIUnixTimestamp());
				return true;
			}
			return false;
		}


		private void RegisterCommands()
		{
			this.Register("eha", new EventAlertCommand());
			this.Register("groupchat", new GroupChatCommand());
			this.Register("sobre", new InfoCommand());
			this.Register("builder", new Builder());
			this.Register("pickall", new PickAllCommand());
			this.Register("ejectall", new EjectAllCommand());
			this.Register("lay", new LayCommand());
			this.Register("sit", new SitCommand());
			this.Register("stand", new StandCommand());
			this.Register("mutepets", new MutePetsCommand());
			this.Register("mutebots", new MuteBotsCommand());
			this.Register("beijar", new KissCommand());
			this.Register("bater", new BaterCommand());
			this.Register("curar", new CurarCommand());
			this.Register("cor", new ColourCommand());
			this.Register("sexo", new SexCommand());
			this.Register("fumar", new WeedCommand());
			this.Register("abracar", new HugCommand());
			this.Register("carro", new CarCommand());
			this.Register("copiar", new MimicCommand());
			this.Register("dance", new DanceCommand());
			this.Register("push", new PushCommand());
			this.Register("pull", new PullCommand());
			this.Register("efeito", new EnableCommand());
			this.Register("enable", new EnableCommand());
			this.Register("follow", new FollowCommand());
			this.Register("disablefollow", new DisableFollowCommand());
			this.Register("faceless", new FacelessCommand());
			this.Register("moonwalk", new MoonwalkCommand());
			this.Register("unload", new UnloadCommand());
			this.Register("reload", new UnloadCommand(true));
			this.Register("fixroom", new RegenMaps());
			this.Register("emptyitems", new EmptyItems());
			this.Register("empty", new EmptyItems());
			this.Register("setmax", new SetMaxCommand());
			this.Register("setspeed", new SetSpeedCommand());
			this.Register("disablefriends", new DisableFriendsCommand());
			this.Register("enablefriends", new EnableFriendsCommand());
			this.Register("disablediagonal", new DisableDiagonalCommand());
			this.Register("flagme", new FlagMeCommand());
			this.Register("stats", new StatsCommand());
			this.Register("kickpets", new KickPetsCommand());
			this.Register("kickbots", new KickBotsCommand());
			this.Register("room", new RoomCommand());
			this.Register("dnd", new DNDCommand());
			this.Register("matar", new MatarCommand());
			this.Register("disablegifts", new DisableGiftsCommand());
			this.Register("convertcredits", new ConvertCreditsCommand());
			this.Register("disablewhispers", new DisableWhispersCommand());
			this.Register("disablemimic", new DisableMimicCommand()); ;
			this.Register("pet", new PetCommand());
			this.Register("spush", new SuperPushCommand());
			this.Register("eventosoff", new DisableEventsCommand());
			this.Register("mencaooff", new DisableMencaoCommand());
			this.Register("emoji", new EmojiCommand());
			this.Register("freestyle", new Freestyle());
			this.Register("setsh", new SetStackHeightCommand());
			this.Register("clearsh", new ClearStackHeightCommand());
			this.Register("hidewired", new HideWiredCommand());
			this.Register("disablesex", new DisableSexCommand());
			this.Register("numerosorte", new NumeroSorteCommand());
			this.Register("removemobi", new RemoverMobisCommand());
			this.Register("ban", new BanCommand());
			this.Register("unban", new UnBanCommand());
			this.Register("mip", new MIPCommand());
			this.Register("ipban", new IPBanCommand());
			this.Register("pcolor", new ColourPrefixCommand());
			this.Register("ncolor", new ColourNameCommand());
			this.Register("ui", new UserInfoCommand());
			this.Register("roomcredits", new GiveRoom());
			this.Register("sa", new StaffAlertCommand());
			this.Register("va", new VIPAlertCommand());
			this.Register("ga", new GuideAlertCommand());
			this.Register("roomunmute", new RoomUnmuteCommand());
			this.Register("roommute", new RoomMuteCommand());
			this.Register("roombadge", new RoomBadgeCommand());
			this.Register("roomalert", new RoomAlertCommand());
			this.Register("roomkick", new RoomKickCommand());
			this.Register("mutar", new MuteCommand());
			this.Register("unmute", new DesmutarUsuario());
			this.Register("massbadge", new MassBadgeCommand());
			this.Register("massgive", new MassGiveCommand());
			this.Register("globalgive", new GlobalGiveCommand());
			this.Register("kick", new KickCommand());
			this.Register("ha", new HotelAlertCommand());
			this.Register("hal", new HALCommand());
			this.Register("give", new GiveCommand());
			this.Register("givebadge", new GiveBadgeCommand());
			this.Register("takebadge", new TakeUserBadgeCommand());
			this.Register("dc", new DisconnectCommand());
			this.Register("alert", new AlertCommand());
			this.Register("tradeban", new TradeBanCommand());
			this.Register("poll", new PollCommand());
			this.Register("lastmsg", new LastMessagesCommand());
			this.Register("lastconsolemsg", new LastConsoleMessagesCommand());
			this.Register("tele", new TeleportCommand());
			this.Register("summon", new SummonCommand());
			this.Register("senduser", new SendUserCommand());
			this.Register("override", new OverrideCommand());
			this.Register("massenable", new MassEnableCommand());
			this.Register("massdance", new MassDanceCommand());
			this.Register("massitem", new MassItemCommand());
			this.Register("masskick", new MassKickCommand());
			this.Register("masskiss", new MassKissCommand());
			this.Register("masssign", new MassSignCommand());
			this.Register("freeze", new FreezeCommand());
			this.Register("unfreeze", new UnFreezeCommand());
			this.Register("amarrar", new AmarrarCommand());
			this.Register("desamarrar", new DesamarrarCommand());
			this.Register("fastwalk", new FastwalkCommand());
			this.Register("superfastwalk", new SuperFastwalkCommand());
			this.Register("coords", new CoordsCommand());
			this.Register("alleyesonme", new AllEyesOnMeCommand());
			this.Register("allroundme", new AllAroundMeCommand());
			this.Register("forcesit", new ForceSitCommand());
			this.Register("ignorewhispers", new IgnoreWhispersCommand());
			this.Register("forced_effects", new DisableForcedFXCommand());
			this.Register("makesay", new MakeSayCommand());
			this.Register("flaguser", new FlagUserCommand());
			this.Register("filtro", new FilterCommand());
			this.Register("usermsj", new UserMessageCommand());
			this.Register("globalmsj", new GlobalMessageCommand());
			this.Register("viewonline", new ViewOnlineCommand());
			this.Register("customalert", new CustomizedHotelAlert());
			this.Register("premiar", new PremiarCommand());
			//this.Register("loginstaff", new LogMeInCommand());
			this.Register("endpoll", new EndPollCommand());
			this.Register("virarpolicial", new OfficerCommand());
			this.Register("prender", new PrisonCommand());
			this.Register("desprender", new UnPrisonCommand());
			this.Register("notifica", new NotificaCommand());
			this.Register("spull", new SuperPullCommand());
			this.Register("bolha", new BolhaCommand());
			this.Register("bemvindo", new BemVindoCommand());
			this.Register("disco", new DiscoCommand());
			this.Register("alertarquarto", new AlertarQuartoCommand());
			this.Register("wired", new WiredVariable());
			this.Register("selfie", new SelfieCommand());
			this.Register("pay", new PayCommand());
			this.Register("onlines", new OnlineCommand());
			this.Register("apostar", new SetBetCommand());
			this.Register("removerwired", new RemoveWiredCommand());
			this.Register("controlabot", new ControlBotCommand());
			this.Register("colocapack", new AddPredesignedCommand());
			this.Register("tirapack", new RemovePredesignedCommand());
			this.Register("bubble", new BubbleCommand());
			this.Register("staffson", new StaffInfo());
			this.Register("bubblebot", new BubbleBotCommand());
			this.Register("update", new UpdateCommand());
			this.Register("emptyuser", new EmptyUser());
			this.Register("deletegroup", new DeleteGroupCommand());
			this.Register("handitem", new CarryCommand());
			this.Register("goto", new GOTOCommand());
			this.Register("dj", new DJAlert());
			this.Register("summonall", new SummonAll());
			this.Register("cataltd", new CatalogUpdateAlert());
			this.Register("reiniciargalaxy", new RestartGalaxyServerCommand());
			this.Register("allsay", new MassSayCommand());
			this.Register("userson", new UsersOncommand());
			this.Register("fakes", new ViewFakesHotelCommand());
			this.Register("contasfakes", new ViewFakesCommand());
			this.Register("rha", new RadioEventAlert());
			this.Register("warproom", new WarpRoomCommand());
			this.Register("warpuser", new WarpUserCommand());
			this.Register("inv", new ViewInventoryCommand());
			this.Register("mobi", new MobiInfoCommand());
			this.Register("loteria", new LoteriaCommandPHB());
			this.Register("aq", new AbrirQuartoCommand());
			this.Register("fq", new FecharQuartoCommand());
			this.Register("explodir", new ExplodirCommand());
			this.Register("aus", new AusenteCommand());
			this.Register("menage", new MenageCommand());
			if (GalaxyServer.Tipo != 1) this.Register("roomvideo", new RoomVideoCommand());
			if (GalaxyServer.Tipo != 1) this.Register("hotelvideo", new HotelVideoCommand());
			if (GalaxyServer.Tipo != 1) this.Register("uservideo", new UserVideoAlertCommand());
			this.Register("kikar", new UserKikarCommand());
			this.Register("ondeta", new OndeTaCommand());
			if (GalaxyServer.Tipo != 1) Register("meusvideos", new ListarVideoTVCommand());
			if (GalaxyServer.Tipo != 1) Register("addvideo", new AdicionarVideoTVCommand());
			if (GalaxyServer.Tipo != 1) Register("removervideo", new RemoverVideoTVCommand());
			if (GalaxyServer.Tipo != 1) Register("limparvideos", new LimparVideoTVCommand());
			this.Register("roomimage", new RoomImageCommand());
			this.Register("iha", new ImageHotelAlertCommand());
			this.Register("userimage", new UserImageAlertCommand());
			if (GalaxyServer.Tipo != 1) Register("player", new PlayerControlCommand());
			if (GalaxyServer.Tipo != 1) Register("roommusic", new RoomSpotifyCommand());
			if (GalaxyServer.Tipo != 1) Register("changelog", new ChangelogCommand());
			if (GalaxyServer.Tipo != 1) Register("ra", new StaffAlertRankCommand());
			if (GalaxyServer.Tipo != 1) this.Register("youtube", new RoomYoutubeCommand());
			this.Register("promo", new PromoAlertCommand());
			this.Register("copiarquarto", new CopiarQuartoCommand());
			this.Register("emptypets", new EmptyPets());
			this.Register("emptybots", new EmptyBot());

			this.Register("blockcmd", new BloquearComando());
			this.Register("unblockcmd", new DesbloquearComando());
			this.Register("blockcmds", new ComandosBloqueados());

			if(GalaxyServer.Tipo != 1)
			this.Register("boquete", new BoqueteCommand());

			if (GalaxyServer.Tipo == 1 || GalaxyServer.Tipo == 4)
				this.Register("atravessarcadeira", new AtravessarCadeira());
		}

		/// <summary>
		/// Registers a Chat Command.
		/// </summary>
		/// <param name="CommandText">Text to type for this command.</param>
		/// <param name="Command">The command to execute.</param>
		public void Register(string CommandText, IChatCommand Command)
		{
			this._commands.Add(CommandText, Command);
		}

		public static string MergeParams(string[] Params, int Start)
		{
			var Merged = new StringBuilder();
			for (int i = Start; i < Params.Length; i++)
			{
				if (i > Start)
					Merged.Append(" ");
				Merged.Append(Params[i]);
			}

			return Merged.ToString();
		}

		public void LogCommand(int UserId, string Data, string MachineId)
		{
			using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
			{
				dbClient.SetQuery("INSERT INTO `logs_client_user` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
				dbClient.AddParameter("UserId", UserId);
				dbClient.AddParameter("Data", Data);
				dbClient.AddParameter("MachineId", MachineId);
				dbClient.AddParameter("Timestamp", GalaxyServer.GetUnixTimestamp());
				dbClient.RunQuery();
			}
		}

		public void LogCommandStaff(int UserId, string Data, string MachineId, string Username)
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

		public bool TryGetCommand(string Command, out IChatCommand IChatCommand)
		{
			return this._commands.TryGetValue(Command, out IChatCommand);
		}
	}
}