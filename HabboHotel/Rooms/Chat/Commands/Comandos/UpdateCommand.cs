using Galaxy.Communication.Packets.Incoming.LandingView;
using Galaxy.Communication.Packets.Outgoing.Catalog;
using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms.TraxMachine;
using System;
using System.Linq;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class UpdateCommand : IChatCommand
    {
        public string PermissionRequired => "command_update";
        public string Parameters => "[VARIAVEL]";
        public string Description => "Atualiza uma parte específica do Hotel.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                StringBuilder List = new StringBuilder();
                List.Append("- LISTA DE COMANDOS DE UPDATE -\n\n");
                List.Append(":update catalogo - Atualiza o catalogo.\n········································································\n");
                List.Append(":update items - Atualiza os items.\n········································································\n");
                List.Append(":update jukebox - Atualiza as musicas.\n········································································\n");
                List.Append(":update wordfilter - Atualiza o filtro do hotel.\n········································································\n");
                List.Append(":update permissions - Atualiza as permissões de rank.\n········································································\n");
                List.Append(":update settings - Atualiza as configurações do hotel.\n········································································\n");
                List.Append(":update bans - Atualiza os banidos do hotel.\n········································································\n");
				List.Append(":update bots - Atualiza os bots do hotel.\n········································································\n");
				List.Append(":update badges - Atualiza a definição de emblema.\n········································································\n");
				List.Append(":update vouchers - Atualiza os vouchers do hotel.\n········································································\n");
				Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

            string UpdateVariable = Params[1];
            switch (UpdateVariable.ToLower())
            {
                case "cata":
                case "catalog":
                case "catalogo":
                case "catalogue":
                    GalaxyServer.GetGame().GetCatalog().Init(GalaxyServer.GetGame().GetItemManager());
                    GalaxyServer.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
                    Session.LogsNotif("Catálogo atualizado corretamente", "catalogue");
                    break;
                case "discos":
                case "songs":
                case "jukebox":
                case "juke":
                case "canciones":
                    int count = TraxSoundManager.Songs.Count;
                    TraxSoundManager.Init();
                    Session.LogsNotif("Músicas recarregadas com sucesso!", "advice");
                    break;
                case "wordfilter":
					GalaxyServer.GetGame().GetChatManager().GetFilter().InitWords();
                    GalaxyServer.GetGame().GetChatManager().GetFilter().InitCharacters();
                    Session.LogsNotif("Filtro atualizado corretamente", "advice");
                    break;
                case "items":
                case "furni":
                case "furniture":
                    GalaxyServer.GetGame().GetItemManager().Init();
                    Session.LogsNotif("Items atualizados corretamente", "advice");
                    break;
                case "models":
                    GalaxyServer.GetGame().GetRoomManager().LoadModels();
                    Session.LogsNotif("Salas atualizadas corretamente.", "advice");
                    break;
				case "promotions":
				case "landing":
					GalaxyServer.GetGame().GetLandingManager().LoadPromotions();
                    Session.LogsNotif("Promoções atualizadas corretamente.", "advice");
                    break;
                case "navigator":
                    GalaxyServer.GetGame().GetNavigator().Init();
                    Session.LogsNotif("Navegador de salas atualizado.", "advice");
                    break;
                case "ranks":
                case "rights":
                case "permissions":
                    GalaxyServer.GetGame().GetPermissionManager().Init();
                    foreach (GameClient Client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
                    {
                        if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().GetPermissions() == null)
                            continue;

                        Client.GetHabbo().GetPermissions().Init(Client.GetHabbo());
                    }
                    Session.LogsNotif("Permissoes atualizadas.", "advice");
                    break;
                case "pinatas":
                    GalaxyServer.GetGame().GetPinataManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                    GalaxyServer.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("catalogue", "Premios Actualizados", ""));
                    break;
                case "crafting":
                    GalaxyServer.GetGame().GetCraftingManager().Init();
                    Session.SendWhisper("Crafting actualizado correctamente.");
                    break;
                case "crackable":
                case "ecotron":
                case "pinata":
                case "piñata":
                    GalaxyServer.GetGame().GetPinataManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                    GalaxyServer.GetGame().GetFurniMaticRewardsMnager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                    GalaxyServer.GetGame().GetTargetedOffersManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                    break;

                case "relampago":
                case "targeted":
                case "targetedoffers":
                    GalaxyServer.GetGame().GetTargetedOffersManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                    break;

                case "config":
                case "settings":
					GalaxyServer.GetGame().GetSettingsManager().Init();
                    ExtraSettings.RunExtraSettings();
                    CatalogSettings.RunCatalogSettings();
                    NotificationSettings.RunNotiSettings();
                    GalaxyServer.GetGame().GetTargetedOffersManager().Initialize(GalaxyServer.GetDatabaseManager().GetQueryReactor());
                    Session.LogsNotif("Configuraçoes atualizadas.", "advice");
                    break;

                case "bans":
                    GalaxyServer.GetGame().GetModerationManager().ReCacheBans();
                    Session.LogsNotif("Cache Ban re-PRONTO!", "advice");
                    break;

                case "quests":
                    GalaxyServer.GetGame().GetQuestManager().Init();
                    Session.LogsNotif("Quests atualizadas.", "advice");
                    break;

                case "achievements":
                    GalaxyServer.GetGame().GetAchievementManager().LoadAchievements();
                    Session.LogsNotif("Achievements atualizados.", "advice");
                    break;

                case "moderation":
                    GalaxyServer.GetGame().GetModerationManager().Init();
                    GalaxyServer.GetGame().GetClientManager().ModAlert("Presets de moderación se han actualizado.Por favor, vuelva a cargar el cliente para ver los nuevos presets.");
                    Session.LogsNotif("Configurações dos moderadores atualizadas.", "advice");
                    break;
                case "vouchers":
                    GalaxyServer.GetGame().GetCatalog().GetVoucherManager().Init();
                    Session.LogsNotif("O catálogo cache atualizado.", "advice");
                    break;
                case "gc":
                case "games":
                case "gamecenter":
                    GalaxyServer.GetGame().GetGameDataManager().Init();
                    Session.LogsNotif("Cache Game Center foi atualizado com sucesso.", "advice");
                    break;

                case "pet_locale":
                    GalaxyServer.GetGame().GetChatManager().GetPetLocale().Init();
                    GalaxyServer.GetGame().GetChatManager().GetPetCommands().Init();
                    Session.LogsNotif("Cache local Animais atualizado.", "advice");
                    break;

                case "locale":
                    GalaxyServer.GetGame().GetLanguageManager().Init();
                    Session.LogsNotif("Locale caché acualizado corretamente.", "advice");
                    break;

                case "mutant":

                    GalaxyServer.GetGame().GetFigureManager().Init();
                    Session.LogsNotif("FigureData manager recarregado com sucesso!", "advice");
                    break;

                case "bots":
                    GalaxyServer.GetGame().GetBotManager().Init();
                    Session.LogsNotif("Bots actualizados.", "advice");
                    break;

                case "rewards":
                    GalaxyServer.GetGame().GetRewardManager().Reload();
                    Session.LogsNotif("Gestor De Recompensas voltou a carregar com sucesso!", "advice");
                    break;

                case "chat_styles":
                    GalaxyServer.GetGame().GetChatManager().GetChatStyles().Init();
                    Session.LogsNotif("estilos de chat recarregado com sucesso!", "advice");
                    break;

              
                case "definitions":
                case "badges":
                    GalaxyServer.GetGame().GetBadgeManager().Init();
                    Session.LogsNotif("Definições placas recarregado com sucesso!", "advice");
                    break;

                case "extras":
                    Core.ExtraSettings.RunExtraSettings();
                    Session.LogsNotif("Configurações extras recarregadas com sucesso!", "advice");
                    break;

                default:
                    Session.LogsNotif("'" + UpdateVariable + "' não é uma coisa válida para recarregar.", "advice");
                    break;
            }
        }
    }
}
