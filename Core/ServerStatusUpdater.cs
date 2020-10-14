using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel;
using log4net;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
namespace Galaxy.Core
{
    public class ServerStatusUpdater : IDisposable
    {
        private static ILog log = LogManager.GetLogger("Galaxy.Core.ServerStatusUpdater");
        private const int UPDATE_IN_SECS = 10;
        public static int _userPeak;

        private static string _lastDate;

        private static bool isExecuted;

        private static Stopwatch lowPriorityProcessWatch;
        private static Timer _mTimer;

        public static void Init()
        {
            Console.ForegroundColor = ConsoleColor.Green;
          //  log.Info("» Atualização do servidor iniciada!");
            Console.ResetColor();
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE server_status SET status = '1', reinciahkphb = '0'");
            }
            lowPriorityProcessWatch = new Stopwatch();
            lowPriorityProcessWatch.Start();
        }

        public static void StartProcessing()
        {
            _mTimer = new Timer(Process, null, 0, 30000);
        }

        internal static void Process(object caller)
        {
            if (lowPriorityProcessWatch.ElapsedMilliseconds >= 30000 || !isExecuted)
            {
                isExecuted = true;
                lowPriorityProcessWatch.Restart();

                var clientCount = GalaxyServer.GetGame().GetClientManager().Count;
                var loadedRoomsCount = GalaxyServer.GetGame().GetRoomManager().Count;
                var Uptime = DateTime.Now - GalaxyServer.ServerStarted;
                Game.SessionUserRecord = clientCount > Game.SessionUserRecord ? clientCount : Game.SessionUserRecord;

				//// Reinicia record de ons
				DataRow PegaNaDB = null;
				using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.SetQuery("SELECT record_on FROM server_status");
					PegaNaDB = dbClient.getRow();
				}

				string RecordOns = "0";

                if (PegaNaDB != null)
                 RecordOns = Convert.ToString(PegaNaDB["record_on"]);

                if (PegaNaDB != null)
                if (clientCount > Convert.ToInt32(RecordOns))
                {
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("UPDATE `server_status` SET `record_on` = '"+ clientCount+ "'");
							GalaxyServer.discordWH("Batemos o record de usuários online no hotel! temos "+clientCount+" onlines.");
						}
                }

				if (clientCount > 30 && GalaxyServer.AvisoOns == 0)
				{
					GalaxyServer.discordWH(":pinching_hand: Temos agora " + clientCount + " usuários onlines!");
					GalaxyServer.AvisoOns = 1;
				}
				else if (clientCount > 50 && GalaxyServer.AvisoOns == 1)
				{
					GalaxyServer.discordWH(":slight_smile: Temos agora " + clientCount + " usuários onlines!");
					GalaxyServer.AvisoOns = 2;
				}
				else if (clientCount > 70 && GalaxyServer.AvisoOns == 2)
				{
					GalaxyServer.discordWH(":sunglasses: Temos agora " + clientCount + " usuários onlines!");
					GalaxyServer.AvisoOns = 3;
				}
				else if (clientCount > 100 && GalaxyServer.AvisoOns == 3)
				{
					GalaxyServer.discordWH(":scream: Temos agora " + clientCount + " usuários onlines!");
					GalaxyServer.AvisoOns = 4;
				}




				Console.Title = string.Concat(GalaxyServer.VersionGalaxy + " ("+ GalaxyServer.Build + ") - " + GalaxyServer.HotelName + " Hotel » " + clientCount + " Onlines » " + loadedRoomsCount + " Quartos » Record: "+ RecordOns + " » Uptime: " + Uptime.Days + "d, " + Uptime.Hours + "h.");

				/// sistema do gráfico de onlines
				if (GalaxyServer.LastOnlines != clientCount && GalaxyServer.LastRooms != loadedRoomsCount)
				{
					using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
					{
						dbClient.runFastQuery("INSERT INTO `graficos_galayservers` (`onlines`, `quartos`, `timestamp`) VALUES ('" + clientCount + "', '" + loadedRoomsCount + "', '" + GalaxyServer.GetUnixTimestamp() + "');");
					}
				}

				GalaxyServer.LastOnlines = clientCount;
				GalaxyServer.LastRooms = loadedRoomsCount;

				using (var queryReactor = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    if (clientCount > _userPeak)
                    {
                        _userPeak = clientCount;
                        GalaxyServer.recordreinicio = clientCount;
						GalaxyServer.discordWH(":star: Batemos o record de reinicio no hotel! temos " + clientCount + " onlines.");

					}
					_lastDate = DateTime.Now.ToShortDateString();
                    queryReactor.runFastQuery(string.Concat("UPDATE `server_status` SET `status` = '2', `users_online` = '", clientCount, "', `loaded_rooms` = '", loadedRoomsCount, "', reinciahkphb = '0', uptime = '" + Uptime.Days + " dia(s), " + Uptime.Hours + " horas e " + Uptime.Minutes + " minutos.', ultimaatualizacao = '"+ GalaxyServer.LastUpdate + "'"));
                    queryReactor.runFastQuery("DELETE FROM cms_news WHERE title like '%location.assign%' or title like '%location.href%' or shortstory like '%location.assign%' or shortstory like '%location.href%';");
                }
            }
        }

        public void Dispose()
        {
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0', `status` = '0'");
            }

            _mTimer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}