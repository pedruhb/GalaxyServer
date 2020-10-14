using Galaxy.Database.Interfaces;
using log4net;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Galaxy.Core
{
    class ExtraSettings
    {

        public static bool WELCOME_MESSAGE_ENABLED = false;
        public static bool TARGETED_OFFERS_ENABLED = false;
        public static bool WELCOME_NEW_MESSAGE_ENABLED = false;
        public static string WELCOME_MESSAGE_URL = "habbopages/bienvenidax.txt";
        public static string COMMAND_USER_URL = "";
        public static string COMMAND_STAFF_URL = "";
        public static bool STAFF_EFFECT_ENABLED_ROOM = false;
        public static bool STAFF_MENSG_ENTER = false;
        public static bool DEBUG_ENABLED = false;
        public static string YOUTUBE_THUMBNAIL_SUBURL = "/youtubethumbnail.php?img=";
        public static string LICENSE = "";
        public static bool CAMERA_ENABLE = true;
        public static string CAMERA_API = "";
        public static string CAMERA_OUTPUT_PICTURES = "http://localhost/camera/pictures/";
        public static int CAMERA_PRICECOINS = 20;
        public static bool CAMERA_ALERT = true;
        public static int CAMERA_PRICEDUCKETS = 10;
        public static int CAMERA_PUBLISHPRICE = 10;
        public static string CAMERA_ITEMID = "202030";
        public static string CAMERA_MAXCACHE = "1000";
        public static int AmbassadorMinRank = 7;
        public static string NomeGotw = "";
        public static string WelcomeMessage = "";
        public static readonly ILog log = LogManager.GetLogger("Galaxy.Core");
        public static bool EMBLEMACABECAEMB = true;
        public static bool EMBLEMACABECASTAFF = true;
        public static int Quartovip = 0;
        public static int Prisao = 0;
        public static bool MensagemAoReceber = true;
        public static int Intervalo = 0;
        public static int NiveltotalGames = 200;
        public static string CodEmblemaNivel = "NV";
        public static string ReiniciarPermissao = "PHB";
        public static int RankEmbaixador = 7;
        public static bool PremiacaoDiaria = false;
        public static string NomeMoedas = "Moedas";
        public static string NomeDuckets = "Duckets";
        public static string NomeDiamantes = "Diamantes";
		public static string ImagemBemVindo = "";
		public static bool PremioPorAtividade = true;
		public static bool LoginStaff = true;

		public static int RespeitosUsers = 10;
		public static int RespeitosStaffs = 200;

		public static bool RunExtraSettings()
        {
            if (File.Exists("Config/bemvindo.txt"))
                WelcomeMessage = File.ReadAllText("Config/bemvindo.txt");
            if (!File.Exists("Config/extras.ini"))
                return false;
            foreach (var @params in from line in File.ReadAllLines("Config/extras.ini", Encoding.UTF8) where !String.IsNullOrWhiteSpace(line) && line.Contains("=") select line.Split('='))
            {
                switch (@params[0])
                {
					case "ImagemBemVindo":
						ImagemBemVindo = @params[1];
						break;
					case "welcome.message.enabled":
                        WELCOME_MESSAGE_ENABLED = @params[1] == "true";
                        break;
                    case "targeted.offers.enabled":
                        TARGETED_OFFERS_ENABLED = @params[1] == "true";
                        break;
                    case "welcome.new.message.enabled":
                        WELCOME_NEW_MESSAGE_ENABLED = @params[1] == "true";
                        break;
                    case "welcome.message.url":
                        WELCOME_MESSAGE_URL = @params[1];
                        break;
                    case "youtube.thumbnail.suburl":
                        YOUTUBE_THUMBNAIL_SUBURL = @params[1];
                        break;
                    case "camera.photo.purchase.price.coins":
                        CAMERA_PRICECOINS = int.Parse(@params[1]);
                        break;
                    case "camera.photo.purchase.price.duckets":
                        CAMERA_PRICEDUCKETS = int.Parse(@params[1]);
                        break;
                    case "camera.photo.publish.price.duckets":
                        CAMERA_PUBLISHPRICE = int.Parse(@params[1]);
                        break;
                    case "camera.photo.purchase.item_id":
                        CAMERA_ITEMID = @params[1];
                        break;
                    case "camera.api.http":
                        CAMERA_API = @params[1];
                        break;
                    case "camera.output.pictures":
                        CAMERA_OUTPUT_PICTURES = @params[1];
                        break;
                    case "camera.picture.purchase.alert.id":
                        CAMERA_ALERT = @params[1] == "true";
                        break;
                    case "camera.enable":
                        CAMERA_ENABLE = @params[1] == "true";
                        break;
                    case "staff.effect.inroom":
                        STAFF_EFFECT_ENABLED_ROOM = @params[1] == "true";
                        break;
                    case "staff.mensg.inroom":
                        STAFF_MENSG_ENTER = @params[1] == "true";
                        break;
                    case "debug.enabled":
                        DEBUG_ENABLED = @params[1] == "true";
                        break;
                    case "coin.points.name":
                        NomeGotw = @params[1];
                        break;
                    case "coin.credits.name":
                        NomeMoedas = @params[1];
                        break;
                    case "coin.duckets.name":
                        NomeDuckets = @params[1];
                        break;
                    case "coin.diamonds.name":
                        NomeDiamantes = @params[1];
                        break;
                    case "ambassador.minrank":
                        AmbassadorMinRank = int.Parse(@params[1]);
                        break;
                    case "command.users.url":
                        COMMAND_USER_URL = @params[1];
                        break;
                    case "command.staff.url":
                        COMMAND_STAFF_URL = @params[1];
                        break;
                    case "emblemacabeca.embaixador":
                        EMBLEMACABECAEMB = @params[1] == "true";
                        break;
                    case "rankembaixador":
                    case "RankEmbaixador":
                        RankEmbaixador = int.Parse(@params[1]);
                        break;
                    case "emblemacabeca.staff":
                        EMBLEMACABECASTAFF = @params[1] == "true";
                        break;
                    case "Quartovip":
                        Quartovip = int.Parse(@params[1]);
                        break;
                    case "Prisao":
                        Prisao = int.Parse(@params[1]);
                        break;
                    case "MensagemAoReceber":
                        MensagemAoReceber = @params[1] == "true";
                        break;
                    case "Intervalo":
                        Intervalo = int.Parse(@params[1]);
                        break;
                    case "NiveltotalGames":
                        NiveltotalGames = int.Parse(@params[1]);
                        break;
                    case "CodEmblemaNivel":
                        CodEmblemaNivel = @params[1];
                        break;
                    case "ReiniciarPermissao":
                        ReiniciarPermissao = @params[1];
                        break;
                    case "PremiacaoDiaria":
                        PremiacaoDiaria = @params[1] == "true";
                        break;
					case "PremioPorAtividade":
						PremioPorAtividade = @params[1] == "true";
						break;
					case "RespeitosUsers":
						RespeitosUsers = int.Parse(@params[1]);
						break;
					case "RespeitosStaffs":
						RespeitosStaffs = int.Parse(@params[1]);
						break;
					case "loginstaff":
						LoginStaff = @params[1] == "true";
						break;
				}

				using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.runFastQuery("UPDATE `server_settings` SET `value`= '" + Intervalo + "' WHERE  `key`= 'user.currency_scheduler.tick';");
				}

			}
            log.Info("» Configurações do servidor -> PRONTO! ");
            return true;
        }
    }
}