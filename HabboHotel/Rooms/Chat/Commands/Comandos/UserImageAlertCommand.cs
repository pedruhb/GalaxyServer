using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.HabboHotel.GameClients;
using Newtonsoft.Json.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class UserImageAlertCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_imagealert"; }
		}

		public string Parameters
		{
			get { return "[LINK]"; }
		}

		public string Description
		{
			get { return "Enviar uma Imagem para o usuário"; }
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{
			if (Session.GetHabbo().isLoggedIn == false)
			{
				Session.SendWhisper("Você não logou como staff!");
				return;
			}
			if (Params.Length == 1)
			{
				Session.SendWhisper("Por favor, introduza o nome do usuário que deseja enviar a imagem...");
				return;
			}
			if (Params.Length == 2)
			{
				Session.SendWhisper("Falta o link da imagem!");
				return;
			}
			GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
			if (TargetClient == null)
			{
				Session.SendWhisper("Usuário não encontrado, talvez ele não esteja online.");
				return;
			}
			RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
			if (TargetUser == null)
			{
				Session.SendWhisper("Usuário não encontrado, talvez ele não esteja online.");
				return;
			}

			string link = Params[2];

            if (link.Contains(".png") || link.Contains(".gif") || link.Contains(".jpg"))
            {
				if (GalaxyServer.Tipo != 1)
				{
					dynamic product = new JObject();
					product.tipo = "imagem";
					product.link = link;

					GalaxyServer.SendUserJson(TargetUser, product);
				}
				else
				{
					TargetUser.GetClient().SendMessage(new GraphicAlertComposer(link));

				}
				Session.SendWhisper("A imagem abrirá na client do usuário.");
            }
            else
            {
                Session.SendWhisper("Precisa ser o link direto para uma imagem, com .png, .gif ou .jpg no final.");
            }
            return;
		}
	}
}