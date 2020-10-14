using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Threading.Tasks;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class SexCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_sexo"; }
		}
		public string Parameters
		{
			get { return "[USUARIO]"; }
		}
		public string Description
		{
			get { return "Faça sexo com alguém."; }
		}
		public bool checkuser(string username, GameClient Session, Room Room) {

			GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(username);
			RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);

			if (TargetClient == null)
			{
				Session.SendWhisper("O usuário saiu do quarto, o comando foi cancelado.");
				return false;
			} else if (TargetUser == null)
			{
				Session.SendWhisper("O usuário saiu do quarto, o comando foi cancelado.");
				return false;
			}
			return true;
		}
		public async void Execute(GameClient Session, Room Room, string[] Params)
		{
			try
			{
					if (Params.Length == 1)
					{
						Session.SendWhisper("Digite o nick de quem você deseja sarrar.");
						return;
					}

					GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
					if (TargetClient == null)
					{
						Session.SendWhisper("Essa pessoa não se encontra no quarto ou não está online.");
						return;
					}

					RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
					if (TargetUser == null)
					{
						Session.SendWhisper("Ocorreu um erro, esse usuário não foi encontrado.");
					}

					if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
					{
						Session.SendWhisper("Você não pode fazer sexo com você mesmo!");
						return;
					}

					if (Session.GetHabbo().AllowSex == false)
					{
						Session.SendWhisper("Você está com o comando sexo desativado! utilize :disablesex");
						return;
					}

					RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
					if (ThisUser == null)
						return;

					if (TargetClient.GetHabbo().AllowSex == true || Session.GetHabbo().Username == "PHB" || Session.GetHabbo().Rank == 17)
					{
						if (GalaxyServer.GetIUnixTimestamp() >= Session.GetHabbo().lastsex)
						{
							if (!(Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2) || Session.GetHabbo().Username == "PHB" || Session.GetHabbo().Rank == 17)
							{

								Session.GetHabbo().lastsex = GalaxyServer.GetIUnixTimestamp() + 300;

								if (Session.GetHabbo().Id == 1 || Session.GetHabbo().Rank == 17)
									Session.GetHabbo().lastsex = GalaxyServer.GetIUnixTimestamp();

								string QuemLeva = TargetClient.GetHabbo().Username;
								string QuemCome = Session.GetHabbo().Username;

								int Efeito1 = ThisUser.CurrentEffect;
								int Efeito2 = TargetUser.CurrentEffect;

								#region Sexo Gay
								if (Session.GetHabbo().Gender.ToLower() == "m" && TargetClient.GetHabbo().Gender.ToLower() == "m")
								{
									ThisUser.ApplyEffect(507);
									TargetUser.ApplyEffect(507);
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Botando no buraco do " + QuemLeva + "*", 0, 16));
									await Task.Delay(1000);
									if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Empurro com força no " + QuemLeva + "*", 0, 16));
									await Task.Delay(1000);
									if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Ohaaaaaaaaaaaaaar*", 0, 16));
									await Task.Delay(1000);
									if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Que pau de jegue " + QuemCome + "*", 0, 16));
									await Task.Delay(1000);
									if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Não enfia tudo, tá doendo..*", 0, 16));
									await Task.Delay(1000);
									if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Aiiii pai paraaaaaa*", 0, 16));
									await Task.Delay(1000);
									if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Não bota tudõo que eu piro aloka*", 0, 16));
									await Task.Delay(1000);
									if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Eu to pro crime hoje*", 0, 16));
									await Task.Delay(1000);
									if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Ain que pauzinho gostoso*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Aahhhhhhhhhhhhhhr*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Vai seu delicia bota com tudo vai*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Mete gostosinho no meu cuzinho vai dlç*", 0, 16));
									ThisUser.ApplyEffect(0);
									TargetUser.ApplyEffect(0);
								}
								#endregion
								#region Sexo Lésbico
								if (Session.GetHabbo().Gender.ToLower() == "f" && TargetClient.GetHabbo().Gender.ToLower() == "f")
								{
									ThisUser.ApplyEffect(507);
									TargetUser.ApplyEffect(507);
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Colando velcro com a " + QuemLeva + "*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Puxa delicadamente para não raspar os pelos da " + QuemLeva + "*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*nheeeeeeeeeeeec*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*ohaaaaaaaaaaaaaaaaar*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Esfrega vai, esfregaa*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Esfrega mais rápido*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Faz aquela posição que eu piro*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Sentindo prazer*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Corta as unhas e faz o dj em mim amor*", 0, 16));
									ThisUser.ApplyEffect(0);
									TargetUser.ApplyEffect(0);
								}
								#endregion
								#region Sexo Hétero M-F
								if (Session.GetHabbo().Gender.ToLower() == "m" && TargetClient.GetHabbo().Gender.ToLower() == "f")
								{
									ThisUser.ApplyEffect(507);
									TargetUser.ApplyEffect(507);
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Botando na xavasca da " + QuemLeva + " com força*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*AAAAAAIN QUE DELÍÍÍCIA, FODE ESSA BUCETAAA!*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Empurrando com força na " + QuemLeva + ", vou esfolar seu cabaço*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Ain pai paraaa, vai devagar, seu gostoso*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Empurra rapidamente o cano do fuzil*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Retirando a jeba da prexeca da " + QuemLeva + ", ahhhh, vou gozar*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Que gostoso seu pirocudo, fode mais, vai*", 0, 16));
									ThisUser.ApplyEffect(0);
									TargetUser.ApplyEffect(0);
								}
								#endregion
								#region Sexo Hétero F-M
								if (Session.GetHabbo().Gender.ToLower() == "f" && TargetClient.GetHabbo().Gender.ToLower() == "m")
								{
									ThisUser.ApplyEffect(507);
									TargetUser.ApplyEffect(507);
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Colocando a calcinha pro lado*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Sensualizando com o dedo na boca*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Que garota saliente*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Encaixando a buceta no pau*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Fazendo movimento suave pra cima e pra baixo*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Sentando na rola com força*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*AAAAAAAAAAAAWWW EU VOU GOZAAR*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Cavalgando como se estivesse em um cavalo*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Sentindo orgasmo chegando*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Caralho " + QuemLeva + ", sua pica parece uma pedra.*", 0, 16));
									await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
									Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Gozando litros.*", 0, 16));
									ThisUser.ApplyEffect(0);
									TargetUser.ApplyEffect(0);

								}
								#endregion

								ThisUser.ApplyEffect(Efeito1);
								TargetUser.ApplyEffect(Efeito2);

								TargetClient.SendWhisper("Use o comando :disablesex para bloquear o sexo!");
							}
							else
							{

								Session.SendWhisper("Chegue mais perto da pessoa.");
								return;
							}
						}
						else
						{
							int diferenca = (GalaxyServer.GetIUnixTimestamp() - Session.GetHabbo().lastsex);
							if (diferenca % 3600 / 60 == 0)
							{
								Session.SendWhisper("Espere alguns segundos para usar novamente esse comando.");
							}
							else
							{
								string difmenos = Convert.ToString(diferenca % 3600 / 60).Replace("-", "");
								if (difmenos == "1")
								{
									Session.SendWhisper("Espere só mais um minutinho para fazer sexo novamente, seu broxa.");
								}
								else
								{
									Session.SendWhisper("Espere " + difmenos + " minutos para fazer sexo novamente, seu broxa.");
								}
							}


						}
					}
					else
					{
						Session.SendWhisper("Esse usuário desativou o comando!");
					}
			} catch (Exception e) { 
		}
		}
	}
}