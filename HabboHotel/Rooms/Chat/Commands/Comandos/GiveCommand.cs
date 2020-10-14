using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class GiveCommand : IChatCommand
    {
        public string PermissionRequired => "command_give";
        public string Parameters => "[USUÁRIO] [MOEDA] [QUANTIDADE]";
        public string Description => "Dar " + ExtraSettings.NomeMoedas.ToLower() + ", " + ExtraSettings.NomeDuckets.ToLower() + ",  " + ExtraSettings.NomeDiamantes.ToLower() + " e " + ExtraSettings.NomeGotw.ToLower() + " a um usuário.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                StringBuilder List = new StringBuilder();
                List.Append("Como posso dar créditos, diamantes, duckets ou " + ExtraSettings.NomeGotw.ToLower() + "?\n········································································\n");
                List.Append(":give [USUÁRIO] credits [QUANTIDADE] - " + ExtraSettings.NomeMoedas.ToLower() + " a um usuário.\n········································································\n");
                List.Append(":give [USUÁRIO] diamonds [QUANTIDADE] - " + ExtraSettings.NomeDiamantes.ToLower() + " a um usuário.\n········································································\n");
                List.Append(":give [USUÁRIO] duckets [QUANTIDADE] - " + ExtraSettings.NomeDuckets.ToLower() + " a um usuário.\n········································································\n");
                List.Append(":give [USUÁRIO] gotw [QUANTIDADE] - " + ExtraSettings.NomeGotw.ToLower() + " a um usuário.\n········································································\n");
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

            GameClient Target = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Uau, não conseguiu encontrar esse usuário!");
                return;
            }

            string UpdateVal = Params[2];
            switch (UpdateVal.ToLower())
            {
                case "coins":
                case "credits":
                case "creditos":
                case "moeda":
                case "moedas":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_coins"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        else
                        {
                            int Amount;
                            if (int.TryParse(Params[3], out Amount))
                            {
                                Target.GetHabbo().Credits = Target.GetHabbo().Credits += Amount;
                                Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));

                                Session.SendMessage(new RoomNotificationComposer("tickets", "message", "Você enviou, " + Amount + " " + ExtraSettings.NomeMoedas.ToLower() + "(s) a " + Target.GetHabbo().Username + "!"));
                                Target.SendMessage(new RoomNotificationComposer("cred", "message", "Você recebeu " + Amount + " " + ExtraSettings.NomeMoedas.ToLower() + "(s) de " + Session.GetHabbo().Username + "!"));

                                // GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Amount + " créditos para " + Target.GetHabbo().Username + "!"));

                                GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Amount + " créditos para " + Target.GetHabbo().Username + "!", ""));

                                GalaxyServer.discordWH(Session.GetHabbo().Username + " enviou " + Amount + " créditos para " + Target.GetHabbo().Username + "!");
								
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                                break;
                            }
                        }
                    }

                case "pixels":
                case "duckets":
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_pixels"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        else
                        {
                            int Amount;
                            if (int.TryParse(Params[3], out Amount))
                            {
                                Target.GetHabbo().Duckets += Amount;
                                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Duckets, Amount));

                                Session.SendMessage(new RoomNotificationComposer("tickets", "message", "Você enviou, " + Amount + " " + ExtraSettings.NomeDuckets.ToLower() + "(s) a " + Target.GetHabbo().Username + "!"));
                                Target.SendMessage(new RoomNotificationComposer("duckets", "message", "Você recebeu " + Amount + " " + ExtraSettings.NomeDuckets.ToLower() + "(s) de " + Session.GetHabbo().Username + "!"));
                            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Amount + " duckets para " + Target.GetHabbo().Username + "!", ""));

                            //GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("duckets", "message", "" + Session.GetHabbo().Username + " enviou " + Amount + " duckets para " + Target.GetHabbo().Username + "!"));
                            GalaxyServer.discordWH(Session.GetHabbo().Username + " enviou " + Amount + " duckets para " + Target.GetHabbo().Username + "!");
							
                            break;
                            }
                            else
                            {
                                Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                                break;
                            }
                        }

                case "diamonds":
                case "diamantes":
                case "diamond":
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_diamonds"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        else
                        {
                            int Amount;
                            if (int.TryParse(Params[3], out Amount))
                            {
                                Target.GetHabbo().Diamonds += Amount;
                            Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Diamonds, 0, 5));
                            Session.SendMessage(new RoomNotificationComposer("tickets", "message", "Você enviou, " + Amount + " " + ExtraSettings.NomeDiamantes.ToLower() + "(s) a " + Target.GetHabbo().Username + "!"));
                                Target.SendMessage(new RoomNotificationComposer("diamonds", "message", "Você recebeu " + Amount + " " + ExtraSettings.NomeDiamantes.ToLower() + "(s) de " + Session.GetHabbo().Username + "!"));

                            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Amount + " diamantes para " + Target.GetHabbo().Username + "!", ""));

                            // GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("diamonds", "message", "" + Session.GetHabbo().Username + " enviou " + Amount + " diamantes para " + Target.GetHabbo().Username + "!"));
                            GalaxyServer.discordWH(Session.GetHabbo().Username + " enviou " + Amount + " diamantes para " + Target.GetHabbo().Username + "!");
							break;
                            }
                            else
                            {
                                Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                                break;
                            }
                        }
                case "gotw":
                case "gotws":
                case "gotwpoints":
                case "fame":
                case "fama":
                case "famepoints":
                case "meteoritos":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        else
                        {
                            int Amount;
                        if (int.TryParse(Params[3], out Amount))
                        {
                            if (Amount > 500)
                            {
                                Session.SendWhisper("Não podem enviar mais de 500 pontos, isso será notificado ao CEO e as ações apropriadas serão tomadas.");
                                return;
                            }

                            Target.GetHabbo().GOTWPoints = Target.GetHabbo().GOTWPoints + Amount;
                            Target.GetHabbo().UserPoints = Target.GetHabbo().UserPoints + 1;
                            Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().GOTWPoints, Amount, 103));

                            if (Target.GetHabbo().Id != Session.GetHabbo().Id)
                                Target.SendMessage(RoomNotificationComposer.SendBubble("moedas", "" + Session.GetHabbo().Username + " enviou a você " + Amount + " " + Core.ExtraSettings.NomeGotw.ToLower() + ".\n", "catalog/open/gotws"));
                            Session.SendMessage(RoomNotificationComposer.SendBubble("furni_placement_error", "Você acabou de enviar " + Amount + " " + Core.ExtraSettings.NomeGotw.ToLower() + " " + Target.GetHabbo().Username + "\nLembre - se de que depositamos sua confiança em você e que esses comandos são vistos ao vivo.", "catalog/open/gotws"));
                            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Target, "ACH_EventsWon", 1);

                            // GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Amount + " " + ExtraSettings.NomeGotw.ToLower() + " para " + Target.GetHabbo().Username + "!"));
                            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Amount + " gotw para " + Target.GetHabbo().Username + "!", ""));

                            GalaxyServer.discordWH(Session.GetHabbo().Username + " enviou " + Amount + " gotw para " + Target.GetHabbo().Username + "!");
							break;
                        }
                        else
                        {
                            Session.SendWhisper("Você só pode inserir parâmetros numéricos, de 1 a 50.");
                            break;
                        }
                    }
              
                default:
                    Session.SendWhisper("'" + UpdateVal + "' não é uma moeda válida!");
                    break;
            }
        }
    }
}