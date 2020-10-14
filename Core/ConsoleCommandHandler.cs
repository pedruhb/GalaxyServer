using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using log4net;
using System;

namespace Galaxy.Core
{
    public class ConsoleCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger("Galaxy.Core.ConsoleCommandHandler");

        public static void InvokeCommand(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return;

            try
            {
                #region Command parsing
                string[] parameters = inputData.Split(' ');

                switch (parameters[0].ToLower())
                {
                    #region ajuda
                    case "ajuda":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("-----------------------------------------------------------------------");
                        Console.WriteLine("");
                        Console.WriteLine("                     Comandos do GalaxyEMU - Console");
                        Console.WriteLine("");
                        Console.WriteLine("desligar - desliga o emulador.");
                        Console.WriteLine("items - atualiza os items.");
                        Console.WriteLine("catalogo - atualiza o catálogo do hotel.");
                        Console.WriteLine("alerta %mensagem% - envia um alerta para todos os usuários.");
                        Console.WriteLine("navegador - atualiza o navegador.");
                        Console.WriteLine("clear - limpa o console do emulador.");
                        Console.WriteLine("reiniciar - reinicia o emulador.");
                        Console.WriteLine("");
                        Console.WriteLine("-----------------------------------------------------------------------");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    #endregion
                    #region targeted
                    case "relampago":
                    case "targeted":
                        GalaxyServer.GetGame().GetTargetedOffersManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                        break;
                    #endregion targeted
                    #region stop
                    case "stop":
                    case "fechar":
                    case "fecharemulador":
                    case "desligar":
                    case "shutdown":
                        ExceptionLogger.DisablePrimaryWriting(true);
                        log.Warn("O servidor está economizando móveis de usuários, salas, etc. ESPERE QUE O SERVIDOR FECHE, NÃO SAIA DO PROCESSO NO GERADOR DE TAREFAS !!");
                        GalaxyServer.PerformShutDown(false);
                        break;
                    #endregion
                    #region actualizarcatalogo
                    case "catalogo":
                    case "catalogue":
                    case "cata":
                    case "atualizarcatalogo":
                        GalaxyServer.GetGame().GetCatalog().Init(GalaxyServer.GetGame().GetItemManager());
                        GalaxyServer.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
                        break;
                    #endregion
                    #region actualizaritems
                    case "items":
                    case "furnis":
                    case "furniture":
                        GalaxyServer.GetGame().GetItemManager().Init();
                        break;
                    #endregion
                    #region restart
                    case "restart":
                    case "reiniciar":
                        ExceptionLogger.DisablePrimaryWriting(true);
                        log.Warn("O servidor está economizando móveis de usuários, salas, etc. ESPERE QUE O SERVIDOR FECHE, NÃO SAIA DO PROCESSO NO GERADOR DE TAREFAS !!");
                        GalaxyServer.PerformShutDown(true);
                        break;
                    #endregion
                    #region Clear Console
                    case "clear":
                        Console.Clear();
                        break;
                    #endregion
                    #region alert
                    case "alert":
                        string Notice = inputData.Substring(6);
                        GalaxyServer.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(GalaxyServer.GetGame().GetLanguageManager().TryGetValue("server.console.alert") + "\n\n" + Notice));
                        log.Info("Alerta enviada correctamente.");
                        break;
                    #endregion
                    #region navigator
                    case "navi":
                    case "navegador":
                    case "navigator":
                        GalaxyServer.GetGame().GetNavigator().Init();
                        break;
                    #endregion
                    #region configs
                    case "config":
                    case "settings":
                        GalaxyServer.GetGame().GetSettingsManager().Init();
                        ExtraSettings.RunExtraSettings();
                        CatalogSettings.RunCatalogSettings();
                        NotificationSettings.RunNotiSettings();
                        GalaxyServer.GetGame().GetTargetedOffersManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                        break;
                    #endregion

                    default:
                        log.Error(parameters[0].ToLower() + " não é um comando válido, digite ajuda para saber mais!");
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                log.Error("Erro no comando [" + inputData + "]: " + e);
            }
        }
    }
}
