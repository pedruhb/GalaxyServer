using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RoomImageCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return "[LINK]"; }
        }

        public string Description
        {
            get { return "Enviar um vídeo para todos do quarto"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Você deve por o link do vídeo do youtube.");
                return;
            }

            if (Session.GetHabbo().Rank < 5 && Session.GetHabbo().Id != Room.OwnerId)
            {
                Session.SendWhisper("Somente o dono pode colocar vídeos.");
                return;
            }

            string link = Params[1];

            if(GalaxyServer.Tipo != 1)
			{
				if (link.Contains(".png") || link.Contains(".gif") || link.Contains(".jpg"))
				{
					dynamic product = new JObject();
					product.tipo = "imagem";
					product.link = link;

					List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
					if (Users.Count > 0)
					{
						foreach (RoomUser U in Users.ToList())
						{
							GalaxyServer.SendUserJson(U, product);
						}
					}
					Session.SendWhisper("A imagem abrirá na client dos usuários.");
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
				}
				else
				{
					Session.SendWhisper("Precisa ser um link direto para a imagem hospedado no beeimg.");
					return;
				}
				
				List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
				if (Users.Count > 0)
				{
					foreach (RoomUser U in Users.ToList())
					{
						U.GetClient().SendMessage(new GraphicAlertComposer(link));
					}
				}
				Session.SendWhisper("A imagem abrirá na client dos usuários.");
				return;
			}
        }
    }
}