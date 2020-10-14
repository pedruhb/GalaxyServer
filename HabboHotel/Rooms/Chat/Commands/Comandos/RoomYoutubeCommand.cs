using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class RoomYoutubeCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return ""; }
		}

		public string Parameters
		{
			get { return "[Nome do Video]"; }
		}

		public string Description
		{
			get { return "Enviar um vídeo para todos do quarto"; }
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{
			if (Params.Length == 1)
			{
				Session.SendWhisper("Você deve por o nome do vídeo.");
				return;
			}

			if (Session.GetHabbo().Rank < 5 && Session.GetHabbo().Id != Room.OwnerId)
			{
				Session.SendWhisper("Somente o dono pode colocar vídeos.");
				return;
			}

			string NomeVideo = CommandManager.MergeParams(Params, 1);


			if (NomeVideo == "" || NomeVideo == null)
			{
				Session.SendWhisper("Você deve por o nome do vídeo.");
				return;
			}

			try
			{
				dynamic stuff = JObject.Parse(GalaxyServer.file_get_contents("https://www.googleapis.com/youtube/v3/search?part=snippet&q="+ NomeVideo.Replace('&', 'e') + "&type=video&key="+GalaxyServer.YoutubeAPI));
				string videoId = stuff.items[0].id.videoId.ToString();
				string videoName = stuff.items[0].snippet.title.ToString();

				dynamic product = new JObject();
				product.tipo = "youtube_video";
				product.video_id = videoId;


				List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
				if (Users.Count > 0)
				{
					foreach (RoomUser U in Users.ToList())
					{
						GalaxyServer.SendUserJson(U, product);
					}
				}
				Session.SendWhisper("O vídeo foi aberto. \"" + videoName + "\".");
				return;
			}
			catch (Exception ex)
			{
				Session.SendWhisper("Erro ao abrir vídeo.");
				return;
			}
			
		}
	}
}