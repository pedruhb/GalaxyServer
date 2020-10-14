using Galaxy.Communication.ConnectionManager;
using Galaxy.Communication.Encryption;
using Galaxy.Communication.Encryption.Keys;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Communication.RCON;
using Galaxy.Core;
using Galaxy.Database;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel;
using Galaxy.HabboHotel.Cache.Type;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.Users.UserData;
using Galaxy.Utilities;
using log4net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Mono.Web;
using System.Net.Http;
using Galaxy.HabboHotel.Rooms;
using System.Data;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Galaxy
{
	public static class GalaxyServer
    {
		public static int Tipo = 0; // 0 = habbografx / 1 = host / 3 = habbver / 4 = bon
		public static int AvisoOns = 0; 
		private static readonly ILog log = LogManager.GetLogger("Galaxy.GalaxyServer");
        public static string HotelName;
		public static string YoutubeAPI = "AIzaSyAFBYhVsaI3HLvz5yT61MhmSzxjNe9HPNc";
		public static string HallDaFama;
		public static string LastUpdate;
		public static bool IsLive;
		public static int LastOnlines = 0;
		public static int LastRooms = 0;
		public static bool Licensed = false;
		public static int recordreinicio;
        public static string ServerHostnamePHB;
        public static string CurrentTime = DateTime.Now.ToString("hh:mm:ss tt" + "- [" + HotelName + "]");
		public const string VersionGalaxy = "Galaxy Server";
		public static string CotacaoDolar = "0,00";
		private static Encoding _defaultEncoding;
        public static CultureInfo CultureInfo;
        private static Game _game;
        private static ConfigurationData _configuration;
        private static ConnectionHandling _connectionManager;
        private static DatabaseManager _manager;
        private static RCONSocket _rcon;
        public static bool Event = false;
        public static DateTime lastEvent;
        public static DateTime ServerStarted;
		public static int Build = Environment.Version.Build;
		private static readonly List<char> Allowedchars = new List<char>(new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.'
            });
        private static ConcurrentDictionary<int, Habbo> _usersCached = new ConcurrentDictionary<int, Habbo>();
        public static string SWFRevision = "";
        public static int Quartovip;
        public static int Prisao;
        public static void Initialize()
        {
			System.IO.File.WriteAllText("logs/Console.log", "");
			FileInfo arquivo = new FileInfo("GalaxyServer.exe");
            DateTime ultimobuildphb = arquivo.LastWriteTime;
            LastUpdate = Convert.ToString(ultimobuildphb.ToShortDateString());
            ServerStarted = DateTime.Now;
            _defaultEncoding = Encoding.Default;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Title = VersionGalaxy+ " (" + GalaxyServer.Build + ") - Carregando...";
            Console.WriteLine(@"");
            Console.WriteLine(@"             ________        __                           _________                                ");
			 Console.WriteLine(@"           /  _____/_____  |  | _____  ___  ______.__.  /   _____/ ______________  __ ___________ ");
			 Console.WriteLine(@"          /   \  ___\__  \ |  | \__  \ \  \/  <   |  |  \_____  \_/ __ \_  __ \  \/ // __ \_  __ \");
			 Console.WriteLine(@"          \    \_\  \/ __ \|  |__/ __ \_>    < \___  |  /        \  ___/|  | \/\   /\  ___/|  | \/");
			 Console.WriteLine(@"           \______  (____  /____(____  /__/\_ \/ ____| /_______  /\___  >__|    \_/  \___  >__|   ");
			 Console.WriteLine(@"                  \/     \/          \/      \/\/              \/     \/                 \/       ");
			Console.WriteLine(@"");
            CultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
			try
			{
				_configuration = new ConfigurationData(Path.Combine(Application.StartupPath, @"Config/config.ini"));

				var connectionString = new MySqlConnectionStringBuilder
				{
					ConnectionTimeout = 10,
					Database = GetConfig().data["db.name"],
					DefaultCommandTimeout = 30,
					Logging = false,
					MaximumPoolSize = uint.Parse(GetConfig().data["db.pool.maxsize"]),
					MinimumPoolSize = uint.Parse(GetConfig().data["db.pool.minsize"]),
					Password = GetConfig().data["db.password"],
					Pooling = true,
					Port = uint.Parse(GetConfig().data["db.port"]),
					Server = GetConfig().data["db.hostname"],
					UserID = GetConfig().data["db.username"],
					AllowZeroDateTime = true,
					ConvertZeroDateTime = true,
				};
				_manager = new DatabaseManager(connectionString.ToString());

				
				

				if (!_manager.IsConnected())
				{
					log.Warn("» Erro ao conectar ao banco de dados \"" + GetConfig().data["db.username"] + "@" + GetConfig().data["db.hostname"] + "\"");
					log.Warn("O emulador esta encerrando automaticamente pois encontrou um erro...");
					//     Console.ReadKey(true);
					Environment.Exit(1);
					return;
				}


                log.Info("» Conectado ao banco de dados!");
				HotelName = Convert.ToString(GetConfig().data["hotel.name"]);
				HallDaFama = Convert.ToString(GetConfig().data["halldafama"]);
				ServerHostnamePHB = Convert.ToString(GetConfig().data["db.hostname"]);
				using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
				{
					dbClient.runFastQuery("TRUNCATE `catalog_marketplace_data`");
					dbClient.runFastQuery("UPDATE `rooms` SET `users_now` = '0' WHERE `users_now` > '0'");
					dbClient.runFastQuery("UPDATE `users` SET `online` = '0' WHERE `online` = '1'");
                    dbClient.runFastQuery("UPDATE `server_settings` SET `value`= '" + ExtraSettings.Intervalo + "' WHERE  `key`= 'user.currency_scheduler.tick';");

					if (GalaxyServer.Tipo != 1)
						dbClient.runFastQuery("ALTER TABLE tv_videos DROP id; ALTER TABLE tv_videos ADD id INT NOT NULL AUTO_INCREMENT FIRST, ADD PRIMARY KEY (id), AUTO_INCREMENT=1;"); // refaz id dos videos da tv
				}

				if (GetConfig().data["db.hostname"] != "144.217.235.235" && GetConfig().data["db.hostname"] != "177.234.159.243" && GetConfig().data["db.hostname"] != "35.247.204.42")
				{
                    // VerificaLicença();
                    Licensed = true;
                } else { 
						Licensed = true;
				}
				_game = new Game();
				_game.ContinueLoading();
				HabboEncryptionV2.Initialize(new RSAKeys());
				_rcon = new RCONSocket(GetConfig().data["mus.tcp.bindip"], int.Parse(GetConfig().data["mus.tcp.port"]), (GetConfig().data["db.hostname"]+";"+GetConfig().data["mus.tcp.allowedaddr"]).Split(Convert.ToChar(";")));
				_connectionManager = new ConnectionHandling(int.Parse(GetConfig().data["game.tcp.port"]), int.Parse(GetConfig().data["game.tcp.conlimit"]), int.Parse(GetConfig().data["game.tcp.conperip"]), GetConfig().data["game.tcp.enablenagles"].ToLower() == "true");
				_connectionManager.Init();
				TimeSpan TimeUsed = DateTime.Now - ServerStarted;
				Quartovip = int.Parse(Convert.ToString(ExtraSettings.Quartovip));
				Prisao = int.Parse(Convert.ToString(ExtraSettings.Prisao));
				Console.ForegroundColor = ConsoleColor.Green;

				log.Info("» " + VersionGalaxy + " -> PRONTO! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");
				log.Info("» Última atualização: " + LastUpdate);

				if (GalaxyServer.Tipo == 1)
					log.Info("» Iniciado para Hosting!");
				else
					log.Info("» Iniciado para Hotel!");

				Console.ResetColor();
				IsLive = true;

				discordWH("O emulador foi iniciado, grande dia :thumbsup:");
				ValorDolar();
				log.Info("» Cotação atual do dólar: R$" + CotacaoDolar);

                using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0', `status` = '1'");
                }

            }
            catch (KeyNotFoundException e)
			{
				log.ErrorFormat("Verifique o seu arquivo de configuração, alguns valores parecem estar faltando.", ConsoleColor.Red);
				log.Warn("O emulador esta encerrando automaticamente pois encontrou um erro...");

				//  log.Error("Pressione qualquer tecla para desligar ...");
				ExceptionLogger.LogException(e);
				// Console.ReadKey(true);
				Environment.Exit(1);
				return;
			}
			catch (InvalidOperationException e)
			{
				log.Error("Falha ao inicializar o " + VersionGalaxy + ":" + e.Message);
				log.Warn("O emulador esta encerrando automaticamente pois encontrou um erro...");
				//log.Error("Pressione qualquer tecla para desligar ...");
				//  Console.ReadKey(true);
		        Environment.Exit(1);
				return;
			}
			catch (Exception e)
			{
				log.Error("Erro fatal durante a inicialização:" + e);
				// log.Error("Pressione uma tecla para sair");
				// Console.ReadKey();
				Environment.Exit(1);
				}
        }

		public static void SendUserJson(HabboHotel.Rooms.RoomUser u, object json)
		{
			if (GalaxyServer.Tipo == 1)
				return;

			 SendUserJson(u.GetClient(), json);
		}
		public static void SendUserJson(int u, object json)
		{
			if (GalaxyServer.Tipo == 1)
				return;

			SendUserJson(GetHabboById(u).GetClient(), json);
		}

		public static async void VerificaLicença()
		{
			var httpClient = new HttpClient();

			var parameters = new Dictionary<string, string>();
			parameters["hostname"] = Convert.ToString(GetConfig().data["db.hostname"]);
			parameters["hotelname"] = HotelName;
			parameters["mysqlpassword"] = Convert.ToString(GetConfig().data["db.password"]);
			parameters["mysqluser"] = Convert.ToString(GetConfig().data["db.username"]);

			var response = httpClient.PostAsync("http://galaxyservers.com.br/galaxyserver", new FormUrlEncodedContent(parameters)).Result;
			var contents = response.Content.ReadAsStringAsync().Result;

			if (contents.ToString() == "true")
			{
				log.Info("» Verificação de licença concluída.");
				Licensed = true;
			} else 
			{
				log.Warn("» Verificação de licença -> IP not allowed.");
				Environment.Exit(1);
				return;
			}
		}

		public static async void SendUserJson(GameClient u, object json)
        {
			if (GalaxyServer.Tipo == 1) return;
			try
            {
				DataRow JsonUser = null;
				using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.SetQuery("SELECT json FROM users WHERE id = " + u.GetHabbo().Id + " AND online = '1' LIMIT 1;");
					JsonUser = dbClient.getRow();
					if (JsonUser["json"] == "" || JsonUser["json"] == null)
					{
						dbClient.SetQuery("UPDATE users SET json = @object_json WHERE id = @id LIMIT 1;");
						dbClient.AddParameter("object_json", json.ToString());
						dbClient.AddParameter("id", u.GetHabbo().Id);
						dbClient.RunQuery();
					}
					else
					{
						Random randNum = new Random();
						await Task.Delay(randNum.Next(1000, 2000));
						SendUserJson(u, json);
					}
				}
			}
            catch (Exception)
            {
            }
        }

        public static bool EnumToBool(string Enum)
        {
            return (Enum == "1");
        }

        public static string BoolToEnum(bool Bool)
        {
            return (Bool == true ? "1" : "0");
        }

        public static int GetRandomNumber(int Min, int Max)
        {
            return RandomNumber.GenerateNewRandom(Min, Max);
        }

        public static string Rainbow()
        {
            int numColors = 1000;
            var colors = new List<string>();
            var random = new Random();
            for (int i = 0; i < numColors; i++)
            {
                colors.Add(String.Format("#{0:X2}{1:X2}00", i, random.Next(0x1000000) - i));
            }
            int index = 0;
            string rainbow = colors[index];

            if (index > numColors)
                index = 0;
             else
                index++;

            return rainbow;
        }

		public static void discordWH(string mensagem)
		{
			if(GalaxyServer.Tipo != 1)
				try
				{
					if (Convert.ToBoolean(GetConfig().data["discord.status"]) == true)
					{
						var httpWebRequest = (HttpWebRequest)WebRequest.Create(GetConfig().data["discord.link"]);
						httpWebRequest.ContentType = "application/json";
						httpWebRequest.Method = "POST";
						using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
						{
							dynamic product = new JObject();
							product.content = mensagem;
							product.username = HotelName;
							streamWriter.Write(product);
						}
						var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
						using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
						{
							var result = streamReader.ReadToEnd();
						}
					}
				} catch (Exception e)
				{
					Console.WriteLine("Erro na API do discord: " + e);
				}
		}

		public class MyObj
		{
			public string id_employe { get; set; }
		}

        public static string file_get_contents(string fileName)
        {
            try
            {
                string sContents = string.Empty;
                if (fileName.ToLower().IndexOf("https:") > -1 || fileName.ToLower().IndexOf("http:") > -1)
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    byte[] response = wc.DownloadData(fileName);
                    sContents = System.Text.Encoding.UTF8.GetString(response);
                }
                else
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                    sContents = sr.ReadToEnd();
                    sr.Close();
                }
                return sContents;
            } catch (Exception erro)
            {
                return "Erro: "+erro;
            }
        }
        public static string GetVideoNameYoutube(string id_video)
        {
            try
            {
                dynamic stuff = JObject.Parse(file_get_contents("https://www.googleapis.com/youtube/v3/videos?id=" + id_video + "&part=snippet,contentDetails&key="+YoutubeAPI));
                string dolar = stuff.items[0].snippet.title.ToString();
                return dolar;
            }
            catch (Exception ex)
            {
                return "erro: " + ex;
            }
        }
        public static string GetVideoCanalYoutube(string id_video)
        {
            try
            {
                dynamic stuff = JObject.Parse(file_get_contents("https://www.googleapis.com/youtube/v3/videos?id="+ id_video + "&part=snippet,contentDetails&key="+YoutubeAPI));
                string dolar = stuff.items[0].snippet.channelTitle.ToString();
                return dolar;
            }
            catch (Exception ex)
            {
                return "erro: " + ex;
            }
        }
      
        public static string GetWebPageTitle(string url)
        {

            try
            {
                string response = file_get_contents(url);
                string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
                        System.Text.RegularExpressions.Regex ex = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        return ex.Match(response).Value.Trim();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e);
                return null;
            }
        }
		#region Valor dólar curl
        public static void ValorDolar()
		{
			try
			{
				string DataCotacao = DateTime.Now.ToString("MM-dd-yyyy");
				DateTime DiaAtual = DateTime.Now;

				if (DiaAtual.DayOfWeek == DayOfWeek.Saturday)
					DataCotacao = DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy");
				else if (DiaAtual.DayOfWeek == DayOfWeek.Sunday)
					DataCotacao = DateTime.Now.AddDays(-2).ToString("MM-dd-yyyy");

				string url = "https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/CotacaoDolarDia(dataCotacao=@dataCotacao)?@dataCotacao=%27" + DataCotacao + "%27&$top=100&$format=json";
				ASCIIEncoding encoding = new ASCIIEncoding();
				WebRequest request = WebRequest.Create(url);
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				Stream stream = request.GetRequestStream();
				stream.Close();
				WebResponse response = request.GetResponse();
				stream = response.GetResponseStream();
				StreamReader sr = new StreamReader(stream);
				string Resposta = sr.ReadToEnd();
				dynamic JsonParse = JObject.Parse(Resposta);
				string cotacao = JsonParse.value[0].cotacaoCompra.ToString();
				CotacaoDolar = cotacao.Replace('.', ',').Substring(0, cotacao.Length - 2);
			} catch (Exception)
			{

			}
		}		
		#endregion

		public static string RainbowT()
        {
            int numColorst = 1000;
            var colorst = new List<string>();
            var randomt = new Random();
            for (int i = 0; i < numColorst; i++)
            {
                colorst.Add(String.Format("#{0:X6}", randomt.Next(0x1000000)));
            }
            int indext = 0;
            string rainbowt = colorst[indext];

            if (indext > numColorst)
                indext = 0;
            else
                indext++;

            return rainbowt;
        }

        public static double GetUnixTimestamp()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return ts.TotalSeconds;
        }

        internal static int GetIUnixTimestamp()
        {
            var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            var unixTime = ts.TotalSeconds;
            return Convert.ToInt32(unixTime);
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }


        public static long Now()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double unixTime = ts.TotalMilliseconds;
            return (long)unixTime;
        }

        public static string FilterFigure(string figure)
        {
            foreach (char character in figure)
            {
                if (!IsValid(character))
                    return "sh-3338-93.ea-1406-62.hr-831-49.ha-3331-92.hd-180-7.ch-3334-93-1408.lg-3337-92.ca-1813-62";
            }

            return figure;
        }

        private static bool IsValid(char character)
        {
            return Allowedchars.Contains(character);
        }

        public static bool IsValidAlphaNumeric(string inputStr)
        {
            inputStr = inputStr.ToLower();
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!IsValid(inputStr[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetUsernameById(int UserId)
        {
            string Name = "Usuário desconhecido";

            GameClient Client = GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client != null && Client.GetHabbo() != null)
                return Client.GetHabbo().Username;

            UserCache User = GetGame().GetCacheManager().GenerateUser(UserId);
            if (User != null)
                return User.Username;

            using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `username` FROM `users` WHERE id = @id LIMIT 1");
                dbClient.AddParameter("id", UserId);
                Name = dbClient.getString();
            }

            if (string.IsNullOrEmpty(Name))
                Name = "Usuário desconhecido";

            return Name;
        }

        public static bool ShutdownStarted { get; set; }

		public static string YoutubeVideoId(string url)
		{
			try
			{
				var uri = new Uri(url);
				var query = HttpUtility.ParseQueryString(uri.Query);
				if (query.AllKeys.Contains("v"))
				{
					return query["v"];
				}

				if (uri.Segments.Last() == "watch" || uri.Segments.Last() == "/")
					return "";
				else
					return uri.Segments.Last();
			}
			catch
			{
				return "";
			}
		}

		public static Habbo GetHabboById(int UserId)
        {
            try
            {
                GameClient Client = GetGame().GetClientManager().GetClientByUserID(UserId);
                if (Client != null)
                {
                    Habbo User = Client.GetHabbo();
                    if (User != null && User.Id > 0)
                    {
                        if (_usersCached.ContainsKey(UserId))
                            _usersCached.TryRemove(UserId, out User);
                        return User;
                    }
                }
                else
                {
                    try
                    {
                        if (_usersCached.ContainsKey(UserId))
                            return _usersCached[UserId];
                        else
                        {
                            UserData data = UserDataFactory.GetUserData(UserId);
                            if (data != null)
                            {
                                Habbo Generated = data.user;
                                if (Generated != null)
                                {
                                    Generated.InitInformation(data);
                                    _usersCached.TryAdd(UserId, Generated);
                                    return Generated;
                                }
                            }
                        }
                    }
                    catch { return null; }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static Habbo GetHabboByUsername(String UserName)
        {
            try
            {
                using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `id` FROM `users` WHERE `username` = @user LIMIT 1");
                    dbClient.AddParameter("user", UserName);
                    int id = dbClient.getInteger();
                    if (id > 0)
                        return GetHabboById(Convert.ToInt32(id));
                }
                return null;
            }
            catch { return null; }
        }

        public static void PerformShutDown()
        {
            PerformShutDown(false);
        }

        public static void PerformRestart()
        {
            PerformShutDown(true);
            using (IQueryAdapter dbClient = _manager.GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `server_status` SET `status` = '1'");
            }
        }
		
        public static void PerformShutDown(bool restart)
        {
            discordWH("O emulador foi desligado.");
            Console.Clear();
            log.Info("Desligando o "+ VersionGalaxy+"...");
            Console.Title = VersionGalaxy+ " (" + GalaxyServer.Build + ") - "+GalaxyServer.HotelName+" - DESLIGANDO!";

            ShutdownStarted = true;

            GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(GetGame().GetLanguageManager().TryGetValue("server.shutdown.message")));
            GetGame().StopGameLoop();
            System.Threading.Thread.Sleep(2500);
            GetConnectionManager().Destroy();//Stop listening.
            GetGame().GetPacketManager().UnregisterAll();//Unregister the packets.
            GetGame().GetPacketManager().WaitForAllToComplete();
            GetGame().GetClientManager().CloseAll();//Close all connections
            GetGame().GetRoomManager().Dispose();//Stop the game loop.

            GetConnectionManager().Destroy();

            using (IQueryAdapter dbClient = _manager.GetQueryReactor())
            {
                dbClient.runFastQuery("TRUNCATE `catalog_marketplace_data`");
                dbClient.runFastQuery("UPDATE `users` SET online = '0', `auth_ticket` = NULL");
                dbClient.runFastQuery("UPDATE `rooms` SET `users_now` = '0'");
                dbClient.runFastQuery("UPDATE `server_status` SET `status` = '0'");
            }

            _connectionManager.Destroy();
            _game.Destroy();

            log.Info("O "+ VersionGalaxy +" foi desligado com exito.");

            if (!restart)
                log.WarnFormat("Servidor desligado!", ConsoleColor.Blue);

            IsLive = false;

            if (restart)
                Process.Start(Assembly.GetEntryAssembly().Location);

            if (restart)
                Console.WriteLine("Reiniciando...");
            else
                Console.WriteLine("Finalizando...");

            System.Threading.Thread.Sleep(1000);
            using (IQueryAdapter dbClient = _manager.GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `server_status` SET `status` = '0', `uptime` = 'Desligado.'");
            } 


            Environment.Exit(0);
        }

        public static ConfigurationData GetConfig()
        {
            return _configuration;
        }

        public static Encoding GetDefaultEncoding()
        {
            return _defaultEncoding;
        }

        public static ConnectionHandling GetConnectionManager()
        {
            return _connectionManager;
        }

        public static Game GetGame()
        {
            return _game;
        }

        public static RCONSocket GetRCONSocket()
        {
            return _rcon;
        }

        public static DatabaseManager GetDatabaseManager()
        {
            return _manager;
        }

        public static ICollection<Habbo> GetUsersCached()
        {
            return _usersCached.Values;
        }

        public static bool RemoveFromCache(int Id, out Habbo Data)
        {
            return _usersCached.TryRemove(Id, out Data);
        }
    }
}