using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Linq;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MassGiveCommand : IChatCommand
    {
        public string PermissionRequired => "command_mass_give";
        public string Parameters => "[MOEDA] [QUANTIDADE]";
        public string Description => "Dê "+ExtraSettings.NomeMoedas.ToLower()+ ", " + ExtraSettings.NomeDuckets.ToLower() + ", " + ExtraSettings.NomeDiamantes.ToLower() + " a todos na sala.";

        public void Execute(GameClient Session, Room room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                StringBuilder List = new StringBuilder();
                List.Append("Como posso dar " + ExtraSettings.NomeMoedas.ToLower() + ", " + ExtraSettings.NomeDiamantes.ToLower() + ", " + ExtraSettings.NomeDuckets.ToLower() + " ou " + ExtraSettings.NomeGotw.ToLower() + " ?\n\n");
                List.Append(":massgive credits [QUANTIDADE] - " + ExtraSettings.NomeMoedas.ToLower() + " para todos os usuários da sala.\n\n");
                List.Append(":massgive diamonds [QUANTIDADE] - " + ExtraSettings.NomeDiamantes.ToLower() + " para todos os usuários da sala.\n\n");
                List.Append(":massgive duckets [QUANTIDADE] - " + ExtraSettings.NomeDuckets.ToLower() + " para todos os usuários da sala.\n\n");
                List.Append(":massgive GOTW [QUANTIDADE] - " + ExtraSettings.NomeGotw.ToLower() + " para todos os usuários da sala.\n\n");
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

            var updateVal = Params[1];
            switch (updateVal.ToLower())
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
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Params[2] + "  " + ExtraSettings.NomeMoedas.ToLower() + " para todos de um quarto!", ""));

                            //GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Params[2] + " " + ExtraSettings.NomeMoedas.ToLower() + " para todos de um quarto!"));
                            foreach (var client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != Session.GetHabbo().Username))
                            {
                                client.GetHabbo().Credits = client.GetHabbo().Credits += amount;
                                client.SendMessage(new CreditBalanceComposer(client.GetHabbo().Credits));

                                 client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " " + ExtraSettings.NomeMoedas.ToLower() + "(s) de " + Session.GetHabbo().Username + "!"));
                            }

                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
                    }

                case "pixels":
                case "duckets":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_pixels"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Params[2] + "  " + ExtraSettings.NomeDuckets.ToLower() + " para todos de um quarto!", ""));
                            
                            //GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Params[2] + " " + ExtraSettings.NomeDuckets.ToLower() + " para todos de um quarto!"));
                            foreach (var client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != Session.GetHabbo().Username))
                            {
                                client.GetHabbo().Duckets += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(
                                    client.GetHabbo().Duckets, amount));

                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " " + ExtraSettings.NomeDuckets.ToLower() + "(s) de " + Session.GetHabbo().Username + "!"));
                            }
                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
                    }

                case "diamonds":
                case "diamantes":
                case "diamond":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_diamonds"))
                        {
                            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Params[2] + "  " + ExtraSettings.NomeDiamantes.ToLower() + " para todos de um quarto!", ""));

                            //GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Params[2] + " " + ExtraSettings.NomeDiamantes.ToLower() + " para todos de um quarto!"));
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (var client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != Session.GetHabbo().Username))
                            {
                                client.GetHabbo().Diamonds += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().Diamonds,
                                    amount,
                                    5));

                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " " + ExtraSettings.NomeDiamantes.ToLower() + "(s) de " + Session.GetHabbo().Username + "!"));
                            }

                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
                    }

                case "gotw":
                case "gotws":
                case "gotwpoints":
                case "fame":
                case "fama":
                case "famepoints":
                case "meteoritos":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        int Amount;
                        if (int.TryParse(Params[2], out Amount))
                        {
                            if (Amount > 50)
                            {
                                Session.SendWhisper("Não podem enviar mais de 50 Pontos, isso será notificado ao CEO e tomará medidas.");
                                return;
                            }
                            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Params[2] + "  " + ExtraSettings.NomeGotw.ToLower() + " para todos de um quarto!", ""));

                            //GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Params[2] + " " + ExtraSettings.NomeGotw.ToLower() + " para todos de um quarto!"));
                            foreach (GameClient Target in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
                            {
                                if (Target == null || Target.GetHabbo() == null || Target.GetHabbo().Username == Session.GetHabbo().Username)
                                    continue;

                                Target.GetHabbo().GOTWPoints = Target.GetHabbo().GOTWPoints + Amount;
                                Target.GetHabbo().UserPoints = Target.GetHabbo().UserPoints + 1;
                                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().GOTWPoints, Amount, 103));

                                if (Target.GetHabbo().Id != Session.GetHabbo().Id)
                                    Target.SendMessage(RoomNotificationComposer.SendBubble("command_notification_credits", "" + Session.GetHabbo().Username + " enviou " + Amount + " " + ExtraSettings.NomeGotw.ToLower() + ".", "")); 
                            }

                            break;
                        }
                        else
                        {
                            Session.SendWhisper("Opa, as quantidades apenas em números ...!");
                            break;
                        }
                    }
                case "gotwt":
                case "gotwpointst":
                case "famet":
                case "famat":
                case "ptft":
                case "famepointst":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Params[2] + "  " + ExtraSettings.NomeGotw.ToLower() + " para todos de um quarto!", ""));

                            //GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Params[2] + " " + ExtraSettings.NomeGotw.ToLower() + " para todos de um quarto!"));
                            foreach (var client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != Session.GetHabbo().Username))
                            {
                                client.GetHabbo().GOTWPoints = client.GetHabbo().GOTWPoints + amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().GOTWPoints,
                                    amount, 103));

                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " " + ExtraSettings.NomeGotw.ToLower() + " de " + Session.GetHabbo().Username + "!"));
                            }
                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
                    }
                default:
                    Session.SendWhisper("'" + updateVal + "' não é uma moeda válida!");
                    break;
            }
        }
    }
}