using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Quests;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Rooms.Chat.Logs;
using Galaxy.HabboHotel.Rooms.Chat.Styles;
using Galaxy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Chat
{
    public class ChatEvent : IPacketEvent
	{
		public void Parse(GameClient Session, ClientPacket Packet)
		{
			if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
				return;

			Room Room = Session.GetHabbo().CurrentRoom;
			if (Room == null)
				return;

			RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (User == null)
				return;

			string Message = StringCharFilter.Escape(Packet.PopString());

			if (Message.Length > 100)
				Message = Message.Substring(0, 100);

			int Colour = Packet.PopInt();

			if (!GalaxyServer.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Colour, out ChatStyle Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().GetPermissions().HasRight(Style.RequiredRight)))
				Colour = 0;

			User.UnIdle();

			if (GalaxyServer.GetUnixTimestamp() < Session.GetHabbo().FloodTime && Session.GetHabbo().FloodTime != 0)
				return;

			if (Session.GetHabbo().TimeMuted > 0)
			{
				if(Session.GetHabbo().TimeMuted > GalaxyServer.GetIUnixTimestamp())
                {
                    int SegundosRestantes = Convert.ToInt32(Session.GetHabbo().TimeMuted) - GalaxyServer.GetIUnixTimestamp();
                    Session.SendWhisper("Ops, você está mutado por "+ SegundosRestantes +" segundos!");
                    return;
                } else
                {
                    Session.GetHabbo().TimeMuted = 0;
                }
			}

			if (!Session.GetHabbo().GetPermissions().HasRight("room_ignore_mute") && Room.CheckMute(Session))
			{
				Session.SendWhisper("Ops, você está mutado!");
				return;
			}

			User.LastBubble = Session.GetHabbo().CustomBubbleId == 0 ? Colour : Session.GetHabbo().CustomBubbleId;

			if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
			{
				if (User.IncrementAndCheckFlood(out int MuteTime))
				{
					Session.SendMessage(new FloodControlComposer(MuteTime));
					return;
				}
			}

			if (Message.StartsWith(":", StringComparison.CurrentCulture))
            {
                if(GalaxyServer.GetGame().GetChatManager().GetCommands().Parse(Session, Message))
                    return;

                else if(Message == ":)" || Message == ":3" || Message == ":(" || Message.ToLower() == ":c" || Message.ToLower() == ":p" || Message == ":]" || Message == ":[" || Message.ToLower() == ":o" || Message.ToLower() == ":v" || Message.ToLower() == ":d" || Message.Contains(": ") || Message.Contains(":/") || Message.Contains(":\\") )
                {                 
                    /// usuário envia emote.
                }
                else
				{
                    Session.SendWhisper("O comando informado não existe!");
                    return;
                }

            }

			GalaxyServer.GetGame().GetChatManager().GetLogs().StoreChatlog(new ChatlogEntry(Session.GetHabbo().Id, Room.Id, Message, UnixTimestamp.GetNow(), Session.GetHabbo(), Room));

            if (Session.GetHabbo().Rank < Convert.ToInt32(GalaxyServer.GetConfig().data["MineRankStaff"]))
            {
                if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override") &&
                GalaxyServer.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Message, out string word))
				{
					Message = "bobba";

               /* Session.GetHabbo().BannedPhraseCount++;
                if (Session.GetHabbo().BannedPhraseCount >= 1)
                {

                    User.MoveTo(Room.GetGameMap().Model.DoorX, Room.GetGameMap().Model.DoorY);
                    Session.GetHabbo().TimeMuted = GalaxyServer.GetIUnixTimestamp() + 60;
                    Session.SendNotification("Você foi silenciado, um moderador vai rever o seu caso, aparentemente, você está divulgando!<font size =\"11\" color=\"#fc0a3a\">Infrações:  <b>" + Session.GetHabbo().BannedPhraseCount + "/5</b></font>");
                    GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("Alerta publicitário:",
                        "Atenção colaboradores do " + GalaxyServer.HotelName + ", o usuário <b>" + Session.GetHabbo().Username + "</b> divulgou um link de um site ou hotel na frase, você poderia investigar?<i> Mensagem do usuário:<font size =\"11\" color=\"#f40909\">  <b>  " + Message +
                        "</b></font></i>\r\n" + "- Nome do usuário: <font size =\"11\" color=\"#0b82c6\">  <b>" +
                        Session.GetHabbo().Username + "</b>", "", "Ir ao Quarto", "event:navigator/goto/" +
                        Session.GetHabbo().CurrentRoomId));
                }

                if (Session.GetHabbo().BannedPhraseCount >= 5)
                {
                    GalaxyServer.GetGame().GetModerationManager().BanUser("GalaxyServer anti-divulgação", HabboHotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Banido por spam com a frase (" + Message + ")", (GalaxyServer.GetUnixTimestamp() + 78892200));
                    Session.Disconnect();
                    return;
                }
                Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Mensagem inapropiada no " + GalaxyServer.HotelName + ". Estamos investigando o que você falou" + " " + Session.GetHabbo().Username + " " + "na sala!"));
          return;
		  */    
            } 

            }

            //Room.GetGameMap().GenerateMaps();
            GalaxyServer.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_CHAT);

			/// sistema de menção
			if (GalaxyServer.Tipo == 0 || GalaxyServer.Tipo == 4)
			{
				if(Session.GetHabbo().RespostaMencao != "")
				{
					Message = "@" + Session.GetHabbo().RespostaMencao + " " + Message;
					Session.GetHabbo().RespostaMencao = "";
				}

				if (Message.StartsWith("@"))
				{
					string[] user_mencionado = Message.Split(' ');
					string usuario = user_mencionado[0].Substring(1);
					
					if(GalaxyServer.Tipo == 0)
					if (usuario.ToLower() == "vo" || usuario.ToLower() == "vã³")
						usuario = "Nenha";

					int QuartoUser = Session.GetHabbo().CurrentRoom.Id;
					/*if (Session.GetHabbo().FollowStatus == false)
					{
						QuartoUser = 0;
					}*/
					dynamic product = new Newtonsoft.Json.Linq.JObject();
					product.tipo = "mencao";
					product.mensagem = Message;
					product.remetente = Session.GetHabbo().Username;
					product.quarto = QuartoUser;
					if (usuario == "everyone")
					{
						if (Session.GetHabbo().Rank > 13)
						{
							if (Session.GetHabbo().isLoggedIn)
							{
								GalaxyServer.GetGame().GetClientManager().SendJson(product.ToString());
							}
							else Session.SendWhisper("Você não fez o login staff.");
						}
						else Session.SendWhisper("Você não pode mencionar todos do hotel!");
					} 
					else if (usuario == "staff" || usuario == "sa")
					{
						if (Session.GetHabbo().Rank > 5)
						{
							if (Session.GetHabbo().isLoggedIn)
							{
								dynamic product2 = new Newtonsoft.Json.Linq.JObject();
								product2.tipo = "mencao";
								product2.mensagem = Session.GetHabbo().Username + ": " + Message.Replace("@sa ", "").Replace("@staff ", "");
								product2.remetente = "staff";
								product2.quarto = QuartoUser;
								GalaxyServer.GetGame().GetClientManager().SendJsonStaff(product2.ToString());
								return;
							}
							else Session.SendWhisper("Você não fez o login staff.");
						}
						else Session.SendWhisper("Você não pode mencionar a equipe!");
					}
					else if (usuario == "room")
					{
						if (Session.GetHabbo().Rank > 13)
						{
							if (Session.GetHabbo().isLoggedIn)
							{
								List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
								if (Users.Count > 0)
								{
									foreach (RoomUser U in Users.ToList())
									{
										GalaxyServer.SendUserJson(U, product);
									}
								}
							}
							else Session.SendWhisper("Você não fez o login staff.");
						}
						else Session.SendWhisper("Você não pode mencionar todos do quarto!");
					}
					else
					{
						GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(usuario);

						if (TargetClient == null)
						{
							Session.SendWhisper("O usuário que você tentou mencionar não está online ou não existe!");
						}
						else if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username && Session.GetHabbo().Id != 1)
						{
							Session.SendWhisper("Você não pode se mencionar.");
						}
						else if (TargetClient.GetHabbo().StatusMencao == false && Session.GetHabbo().Rank < 14)
						{
							Session.SendWhisper("O usuário desativou a capacidade de ser mencionado.");
						}
						else
						{
							GalaxyServer.SendUserJson(TargetClient, product);
						}
					}

				}
			} 
   else
			{
				if (Message.StartsWith("@"))
				{
					string[] user_mencionado = Message.Split(' ');
					string usuario = user_mencionado[0].Substring(1);
					if (usuario == "everyone")
					{
						if (Session.GetHabbo().Rank > 13)
						{
							if (Session.GetHabbo().isLoggedIn)
							{
								GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer(Session.GetHabbo().Username + " mencionou você em uma mensagem: " + Message, "fig/" + Session.GetHabbo().Look));
							}
							else Session.SendWhisper("Você não fez o login staff.");
						}
						else Session.SendWhisper("Você não pode mencionar todos do hotel!");
					}
					else if (usuario == "room")
					{
						if (Session.GetHabbo().Rank > 13)
						{
							if (Session.GetHabbo().isLoggedIn)
							{
								List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
								if (Users.Count > 0)
								{
									foreach (RoomUser U in Users.ToList())
									{
										U.GetClient().SendMessage(new RoomNotificationComposer(Session.GetHabbo().Username + " mencionou você em uma mensagem: " + Message, "fig/" + Session.GetHabbo().Look));
									}
								}
							}
							else Session.SendWhisper("Você não fez o login staff.");
						}
						else Session.SendWhisper("Você não pode mencionar todos do quarto!");
					}
					else
					{
						GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(usuario);

						if (TargetClient == null)
						{
							Session.SendWhisper("O usuário que você tentou mencionar não está online ou não existe!");
						}
						else if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username && Session.GetHabbo().Id != 1)
						{
							Session.SendWhisper("Você não pode se mencionar.");
						}
						else if (TargetClient.GetHabbo().StatusMencao == false && Session.GetHabbo().Rank < 14)
						{
							Session.SendWhisper("O usuário desativou a capacidade de ser mencionado.");
						}
						else
						{
							TargetClient.SendMessage(new RoomNotificationComposer("fig/" + Session.GetHabbo().Look, 3, Session.GetHabbo().Username + " mencionou você em uma mensagem: " + Message, "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
						}
					}

				}
			}
			///

			User.OnChat(User.LastBubble, Message, false);

		}
	}
}