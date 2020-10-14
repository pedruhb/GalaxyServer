using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
	class SendVideoToUserBox : IWiredItem
	{
		public Room Instance { get; set; }

		public Item Item { get; set; }

		public WiredBoxType Type { get { return WiredBoxType.SendGrapicAlertPHB; } }

		public ConcurrentDictionary<int, Item> SetItems { get; set; }

		public string StringData { get; set; }

		public bool BoolData { get; set; }

		public string ItemsData { get; set; }

		public SendVideoToUserBox(Room Instance, Item Item)
		{
			this.Instance = Instance;
			this.Item = Item;
			this.SetItems = new ConcurrentDictionary<int, Item>();
		}

		public void HandleSave(ClientPacket Packet)
		{
			int Unknown = Packet.PopInt();
			string Message = Packet.PopString();

			this.StringData = Message;
		}

		public bool Execute(params object[] Params)
		{

			if (GalaxyServer.Tipo == 1)
				return false; 

			if (Params == null || Params.Length == 0)
				return false;

			Habbo Player = (Habbo)Params[0];
			if (Player == null || Player.GetClient() == null || string.IsNullOrWhiteSpace(StringData))
				return false;

			RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
			if (User == null)
				return false;

			string link = StringData;

			if (link.Contains("youtube.com") || link.Contains("youtu.be"))
			{
				string IdVideo = GalaxyServer.YoutubeVideoId(link);

				if (IdVideo == null || IdVideo == "")
				{
					Player.GetClient().SendWhisper("Link do Youtube inválido!");
					return false;
				}

                dynamic product = new Newtonsoft.Json.Linq.JObject();
                product.tipo = "youtube_video";
                product.video_id = IdVideo;

                GalaxyServer.SendUserJson(User, product);


                Player.GetClient().SendWhisper("Um wired fez abrir um vídeo em sua client.");
				return true;
			}
            else if (link.Contains("https://www.twitch.tv/"))
            {
                string IdVideo = link.Replace("https://www.twitch.tv/", "");
                dynamic product = new Newtonsoft.Json.Linq.JObject();
                product.tipo = "twitch_stream";
                product.stream_id = IdVideo;

                GalaxyServer.SendUserJson(User, product);

                Player.GetClient().SendWhisper("Um wired fez abrir um stream em sua client.");
                return true;
            }
            else if (link.Contains("facebook.com/") && link.Contains("videos/"))
            {
                string IdVideo = link;
                dynamic product = new Newtonsoft.Json.Linq.JObject();
                product.tipo = "facebook_video";
                product.facebook_url = IdVideo;


                GalaxyServer.SendUserJson(User, product);

                Player.GetClient().SendWhisper("Um wired fez abrir um vídeo em sua client.");
            }
            if (link.Contains(".png") || link.Contains(".gif") || link.Contains(".jpg"))
            {
                dynamic product = new Newtonsoft.Json.Linq.JObject();
                product.tipo = "imagem";
                product.link = link;

                GalaxyServer.SendUserJson(User, product);

                Player.GetClient().SendWhisper("Um wired fez abrir uma imagem em sua client.");
                return true;
            }
            else
			{
				Player.GetClient().SendWhisper("Precisa ser um link do YouTube, Twitch, Facebook ou uma imagem png, gif ou jpg.");
				return false;
			}

		}
	}
}