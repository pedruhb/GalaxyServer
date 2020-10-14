using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class PromoAlertCommand : IChatCommand
	{
		public string PermissionRequired => "command_alert_user";
		public string Parameters => "[USUARIO]";
		public string Description => "Envia um alerta bolha informando o usuário que ganhou a promo.";

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{

			if (Session.GetHabbo().isLoggedIn == false)
			{
				Session.SendWhisper("Você não logou como staff!");
				return;
			}

			if (Params.Length == 1)
			{
				Session.SendWhisper("Digite o nome do usuário.");
				return;
			}

			GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
			if (TargetClient == null)
			{
				Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez ele não esteja online.");
				return;
			}

			if (TargetClient.GetHabbo() == null)
			{
				Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez ele não esteja online.");
				return;
			}

			GalaxyServer.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("fig/" + TargetClient.GetHabbo().Look, TargetClient.GetHabbo().Username + " ganhou uma promoção no " + GalaxyServer.HotelName + "."));

		}
	}
}
