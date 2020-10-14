using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Linq;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    internal class GlobalGiveCommand : IChatCommand
    {
        public string PermissionRequired => "command_global_currency";
        public string Parameters => "[MOEDA] [QUANTIDADE]";
        public string Description => "Dar " + ExtraSettings.NomeMoedas.ToLower() + ", " + ExtraSettings.NomeDuckets.ToLower() + ",  " + ExtraSettings.NomeDiamantes.ToLower() + " e " + ExtraSettings.NomeGotw.ToLower() + " para todos.";

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
                List.Append("Como posso dar " + ExtraSettings.NomeMoedas.ToLower() + ", " + ExtraSettings.NomeDiamantes.ToLower() + ", " + ExtraSettings.NomeDuckets.ToLower() + " ou " + ExtraSettings.NomeGotw.ToLower() + "?\n········································································\n");
                List.Append(":globalgive credits [QUANTIDADE] - "+ExtraSettings.NomeMoedas.ToLower()+" para todos os usuários.\n········································································\n");
                List.Append(":globalgive diamonds [QUANTIDADE] - "+ExtraSettings.NomeDiamantes.ToLower()+" para todos os usuários.\n········································································\n");
                List.Append(":globalgive duckets [QUANTIDADE] - "+ExtraSettings.NomeDuckets.ToLower()+" para todos os usuários.\n········································································\n");
                List.Append(":globalgive gotw [QUANTIDADE] - " + ExtraSettings.NomeGotw.ToLower() + " para todos os usuários.\n········································································\n");
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

            string updateVal = Params[1];
            int amount;
            switch (updateVal.ToLower())
            {
                case "coins":
                case "credits":
                case "creditos":
                case "moeda":
                case "moedas":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_coins"))
                        {
                            Session.SendWhisper("Ops, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        
                        if (int.TryParse(Params[2], out amount))
                        {
                        GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Params[2] + "  " + ExtraSettings.NomeMoedas.ToLower() + " para todos do hotel!", ""));

                        /// GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Params[2] + " " + ExtraSettings.NomeMoedas.ToLower() + " para todo o hotel!"));

                        foreach (GameClient client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Credits += amount;
                                client.SendMessage(new CreditBalanceComposer(client.GetHabbo().Credits));
                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu "+amount+ " " + ExtraSettings.NomeMoedas.ToLower() + "(s) globais!"));
                            }
                            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("UPDATE users SET credits = credits + " + amount);
                            }
                            break;
                        }
                        Session.SendWhisper("Ops, isso parece ser um valor inválido!");
                        break;

                case "pixels":
                case "duckets":
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_pixels"))
                        {
                            Session.SendWhisper("Ops, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        if (int.TryParse(Params[2], out amount))
                        {
                        GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Params[2] + "  " + ExtraSettings.NomeDuckets.ToLower() + " para todos do hotel!", ""));

                       // GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Params[2] + " " + ExtraSettings.NomeDuckets.ToLower() + " para todo o hotel!"));
                        foreach (GameClient client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Duckets += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(
                                    client.GetHabbo().Duckets, amount));
                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " " + ExtraSettings.NomeDuckets.ToLower() + "(s) globais!"));
                            }
                            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("UPDATE users SET activity_points = activity_points + " + amount);
                            }
                            break;
                        }
                        Session.SendWhisper("Ops, isso parece ser um valor inválido!");
                        break;


                case "diamonds":
                case "diamantes":
                case "diamond":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_diamonds"))
                        {
                            Session.SendWhisper("Ops, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        if (int.TryParse(Params[2], out amount))
                        {
                        GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Params[2] + "  " + ExtraSettings.NomeDiamantes.ToLower() + " para todos do hotel!", ""));

                        //GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Params[2] + " " + ExtraSettings.NomeDiamantes.ToLower() + " para todo o hotel!"));
                        foreach (GameClient client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
                            {

                                client.GetHabbo().Diamonds += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().Diamonds,
                                    amount,
                                    5));
                            }
                            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("UPDATE users SET vip_points = vip_points + " + amount);
                            }
                            break;
                        }
                        Session.SendWhisper("Ops, isso parece ser um valor inválido!");
                        break;
                case "gotw":
                case "gotws":
                case "gotwpoints":
                case "fame":
                case "fama":
                case "famepoints":
                case "meteoritos":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            Session.SendWhisper("Ops, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        if (int.TryParse(Params[2], out amount))
                    {
                        GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " enviou " + Params[2] + "  " + ExtraSettings.NomeGotw.ToLower() + " para todos do hotel!", ""));

                        //GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("cred", "message", "" + Session.GetHabbo().Username + " enviou " + Params[2] + " " + Core.ExtraSettings.NomeGotw.ToLower() + " para todo o hotel!"));
                        foreach (GameClient client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().GOTWPoints = client.GetHabbo().GOTWPoints + amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().GOTWPoints,
                                    amount, 103));
                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " "+Core.ExtraSettings.NomeGotw.ToLower()+" globais!"));
                            }
                            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("UPDATE users SET gotw_points = gotw_points + " + amount);
                            }
                            break;
                        }
                        Session.SendWhisper("Ops, isso parece ser um valor inválido!");
                        break;
            }
        }
    }
}