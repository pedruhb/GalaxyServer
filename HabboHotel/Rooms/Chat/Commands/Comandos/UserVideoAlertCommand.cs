using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class UserVideoAlertCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_imagealert"; }
		}

		public string Parameters
		{
			get { return "[USER] [LINK]"; }
		}

		public string Description
		{
			get { return "Enviar uma vídeo para o usuário"; }
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
				Session.SendWhisper("Por favor, introduza o nome do usuário que deseja enviar o vídeo...");
				return;
			}
			if (Params.Length == 2)
			{
				Session.SendWhisper("Falta o link do vídeo!");
				return;
			}
			GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
			if (TargetClient == null)
			{
				Session.SendWhisper("Usuário não encontrado, talvez ele não esteja online.");
				return;
			}

			string link = Params[2];


			if (link.Contains("youtube.com") || link.Contains("youtu.be"))
			{
				string IdVideo = GalaxyServer.YoutubeVideoId(link);

				if (IdVideo == null || IdVideo == "")
				{
					Session.SendWhisper("Link do Youtube inválido!");
					return;
				}

                dynamic product = new Newtonsoft.Json.Linq.JObject();
                product.tipo = "youtube_video";
                product.video_id = IdVideo;

                GalaxyServer.SendUserJson(TargetClient, product);

                Session.SendWhisper("O vídeo abrirá na client do usuário.");
			}
            else if (link.Contains("https://www.twitch.tv/"))
            {
                string IdVideo = link.Replace("https://www.twitch.tv/", "");
                dynamic product = new Newtonsoft.Json.Linq.JObject();
                product.tipo = "twitch_stream";
                product.stream_id = IdVideo;

                GalaxyServer.SendUserJson(TargetClient, product);

                Session.SendWhisper("O stream abrirá na client dos usuários.");
            }
            else if (link.Contains("facebook.com/") && link.Contains("videos/"))
            {
                string IdVideo = link;
                dynamic product = new Newtonsoft.Json.Linq.JObject();
                product.tipo = "facebook_video";
                product.stream_id = IdVideo;


                GalaxyServer.SendUserJson(TargetClient, product);

                Session.SendWhisper("O vídeo abrirá na client do usuário.");
            }
            else if (link.Contains("https://www.pornhub.com/view_video.php?viewkey=") && Session.GetHabbo().Rank > 10 || link.Contains("https://pt.pornhub.com/view_video.php?viewkey=") && Session.GetHabbo().Rank > 10)
            {
                string IdVideo = link.Replace("https://www.pornhub.com/view_video.php?viewkey=", "").Replace("https://pt.pornhub.com/view_video.php?viewkey=", "");

                dynamic product = new Newtonsoft.Json.Linq.JObject();
                product.tipo = "pornhub_video";
                product.video_id = IdVideo;

                GalaxyServer.SendUserJson(TargetClient, product);

                Session.SendWhisper("O pornô abrirá na client dos usuários.");

            }
            else if (link.Contains("https://www.xvideos.com/video") && Session.GetHabbo().Rank > 10 || link.Contains("http://www.xvideos.com/video") && Session.GetHabbo().Rank > 10)
            {
                string IdVideo = link.Replace("https://www.xvideos.com/video", "").Replace("http://www.xvideos.com/video", "").Split('/')[0];

                if (System.Convert.ToInt32(IdVideo) > 0)
                {
                    dynamic product = new Newtonsoft.Json.Linq.JObject();
                    product.tipo = "xvideos_video";
                    product.video_id = IdVideo;

                    GalaxyServer.SendUserJson(TargetClient, product);

                    Session.SendWhisper("O pornô abrirá na client do usuário.");
                }
                else
                {
                    Session.SendWhisper("Confira o link e tente novamente.");
                }
            }
            else
			{
				Session.SendWhisper("Precisa ser um link do YouTube");
			}
			return;
		}
	}
}