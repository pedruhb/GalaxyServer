using Galaxy.Communication.Packets.Outgoing.Notifications;
using Newtonsoft.Json.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ImageHotelAlertCommand : IChatCommand
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
            get { return "Enviar uma Imagem para todo o Hotel"; }
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
                Session.SendWhisper("Algo deu errado! Tente novamente");
                return;
            }

            string link = Params[1];

			if (GalaxyServer.Tipo != 1)
			{
				if (link.Contains(".png") || link.Contains(".gif") || link.Contains(".jpg"))
				{
					dynamic product = new JObject();
					product.tipo = "imagem";
					product.link = link;

					GalaxyServer.GetGame().GetClientManager().SendJson(product);

					Session.SendWhisper("A imagem abrirá na client do usuário.");
				}
				else
				{
					Session.SendWhisper("Precisa ser o link direto para uma imagem, com .png, .gif ou .jpg no final.");
				}
				return;
			}
			else
			{
				if (link.Contains("png") || link.Contains("jpg") || link.Contains("gif"))
				{
					GalaxyServer.GetGame().GetClientManager().SendMessage(new GraphicAlertComposer(link));
				}
				else
				{
					Session.SendWhisper("Precisa ser um link direto para a imagem hospedado no blogger, beeimg ou GalaxyADS.");
				}
				return;
			}
        }
    }
}