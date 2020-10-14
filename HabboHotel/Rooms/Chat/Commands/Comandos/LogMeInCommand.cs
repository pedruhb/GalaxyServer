using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Users;
using System;
using System.Data;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class LogMeInCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_loginstaff"; }
        }
        public string Parameters
        {
            get { return "[SENHA]"; }
        }
        public string Description
        {
            get { return "Efetuar login como staff"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

			if (Params.Length == 1)
                {
                    Session.SendWhisper("Algo está faltando!");
                    return;
                }

                if (Session.GetHabbo().isLoggedIn == true)
                {
                    Session.SendWhisper("Você já entrou!");
                    return;
                }

                    DataRow password = null;

                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT `pin`,ip_last FROM users WHERE `id` = @Id LIMIT 1");
                        dbClient.AddParameter("Id", Session.GetHabbo().Id);
                        password = dbClient.getRow();
                    }

                    if (password["pin"].ToString() == Params[1])
                    {
                        Session.GetHabbo().isLoggedIn = true;
                        Session.SendWhisper("Login staff efetuado com sucesso!");
                        GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " fez o login staff!", ""));
						GalaxyServer.discordWH(":lock: O usuário " + Session.GetHabbo().Username+" acaba de realizar o login staff! IP: "+ password["ip_last"].ToString());

					Session.SendWhisper("Cotação do dólar é atualizada sempre que é realizado o loginstaff.");
					GalaxyServer.ValorDolar();

					return;
                    }
                    else if (password["pin"].ToString() != Params[1])
                    {
                        Session.SendWhisper("Senha incorreta.");
                        GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, "O " + Session.GetHabbo().Username + " errou a senha no login staff!", ""));
                        return;             
                    }


		}

	}

}
