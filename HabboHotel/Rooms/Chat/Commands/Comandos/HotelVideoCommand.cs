using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class HotelVideoCommand : IChatCommand
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
			get { return "Enviar um vídeo para todos do hotel"; }
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{
			if (Params.Length == 1)
			{
				Session.SendWhisper("Você deve por o link do vídeo do youtube.");
				return;
			}

			string link = Params[1];

            if (link.Contains("youtube.com") || link.Contains("youtu.be"))
            {
                string IdVideo = GalaxyServer.YoutubeVideoId(link);

                if (IdVideo == null || IdVideo == "")
                {
                    Session.SendWhisper("Link do Youtube inválido!");
                    return;
                }

                dynamic product = new JObject();
                product.tipo = "youtube_video";
                product.video_id = IdVideo;


				GalaxyServer.GetGame().GetClientManager().SendJson(product);

				Session.SendWhisper("O vídeo abrirá na client dos usuários.");
            }
            else if (link.Contains("https://www.twitch.tv/"))
            {
                string IdVideo = link.Replace("https://www.twitch.tv/", "");
                dynamic product = new JObject();
                product.tipo = "twitch_stream";
                product.stream_id = IdVideo;


				GalaxyServer.GetGame().GetClientManager().SendJson(product);
				Session.SendWhisper("O stream abrirá na client dos usuários.");
            }
            else if (link.Contains("facebook.com/") && link.Contains("videos/"))
            {
                string IdVideo = link;
                dynamic product = new JObject();
                product.tipo = "facebook_video";
                product.facebook_url = IdVideo;


                List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
                if (Users.Count > 0)
                {
                    foreach (RoomUser U in Users.ToList())
                    {
                        GalaxyServer.SendUserJson(U, product);
                    }
                }
                Session.SendWhisper("O vídeo abrirá na client dos usuários.");
            }
            else if (link.Contains("https://www.pornhub.com/view_video.php?viewkey=") && Session.GetHabbo().Rank > 10 || link.Contains("https://pt.pornhub.com/view_video.php?viewkey=") && Session.GetHabbo().Rank > 10)
            {
                string IdVideo = link.Replace("https://www.pornhub.com/view_video.php?viewkey=", "").Replace("https://pt.pornhub.com/view_video.php?viewkey=", "");

                dynamic product = new JObject();
                product.tipo = "pornhub_video";
                product.video_id = IdVideo;
				GalaxyServer.GetGame().GetClientManager().SendJson(product);
				Session.SendWhisper("O pornô abrirá na client dos usuários.");

            }
            else if (link.Contains("https://www.xvideos.com/video") && Session.GetHabbo().Rank > 10 || link.Contains("http://www.xvideos.com/video") && Session.GetHabbo().Rank > 10)
            {
                string IdVideo = link.Replace("https://www.xvideos.com/video", "").Replace("http://www.xvideos.com/video", "").Split('/')[0];

                if (System.Convert.ToInt32(IdVideo) > 0)
                {
                    dynamic product = new JObject();
                    product.tipo = "xvideos_video";
                    product.video_id = IdVideo;
					GalaxyServer.GetGame().GetClientManager().SendJson(product);
					Session.SendWhisper("O pornô abrirá na client dos usuários.");
                }
                else
                {
                    Session.SendWhisper("Confira o link e tente novamente.");
                }
            }
            else
            {
                Session.SendWhisper("Precisa ser um link do youtube, twitch ou facebook.");
            }

            return;
        }
	}
}