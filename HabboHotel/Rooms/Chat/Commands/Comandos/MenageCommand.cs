using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Threading.Tasks;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class MenageCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_sexo"; }
		}
		public string Parameters
		{
			get { return "[USUARIO] [USUARIO]"; }
		}
		public string Description
		{
			get { return "Faça sexo com duas pessoas ao mesmo tempo."; }
		}
		public bool checkuser(string username, GameClient Session, Room Room)
		{

			GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(username);
			RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);

			if (TargetClient == null)
			{
				Session.SendWhisper("Um usuário saiu do quarto, o comando foi cancelado.");
				return false;
			}
			else if (TargetUser == null)
			{
				Session.SendWhisper("Um usuário saiu do quarto, o comando foi cancelado.");
				return false;
			}
			return true;
		}
		public async void Execute(GameClient Session, Room Room, string[] Params)
		{
			try {
				if (Params.Length == 1)
				{
					Session.SendWhisper("Digite o nick da primeira pessoa que você quer fuder.");
					return;
				}

				if (Params.Length == 2)
				{
					Session.SendWhisper("Digite o nick da segunda pessoa que você quer fuder.");
					return;
				}

				GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
				if (TargetClient == null)
				{
					Session.SendWhisper("A primeira pessoa não se encontra no quarto ou não está online.");
					return;
				}

				GameClient TargetClient2 = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[2]);
				if (TargetClient == null)
				{
					Session.SendWhisper("A segunda pessoa não se encontra no quarto ou não está online.");
					return;
				}

				RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
				if (TargetUser == null)
				{
					Session.SendWhisper("Ocorreu um erro, o primeiro usuário não foi encontrado.");
				}

				RoomUser TargetUser2 = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient2.GetHabbo().Id);
				if (TargetUser == null)
				{
					Session.SendWhisper("Ocorreu um erro, o segundo usuário não foi encontrado.");
				}


				if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username || TargetClient2.GetHabbo().Username == Session.GetHabbo().Username)
				{
					Session.SendWhisper("Você não pode fazer sexo com você mesmo!");
					return;
				}

				if (Session.GetHabbo().AllowSex == false)
				{
					Session.SendWhisper("Você está com o comando sexo desativado! utilize :disablesex");
					return;
				}

				if (TargetUser.GetUsername() == TargetUser2.GetUsername())
				{
					Session.SendWhisper("Você não pode escolher a mesma pessoa nos 2 usuários.");
					return;
				}

				RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (ThisUser == null)
					return;


				if (TargetClient.GetHabbo().AllowSex == true && TargetClient2.GetHabbo().AllowSex == true || Session.GetHabbo().Rank == 17)
				{
					if (GalaxyServer.GetIUnixTimestamp() >= Session.GetHabbo().lastsex)
					{
						if (!(Math.Abs(TargetUser.X - ThisUser.X) >= 3) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 3) || !(Math.Abs(TargetUser2.X - ThisUser.X) >= 3) || (Math.Abs(TargetUser2.Y - ThisUser.Y) >= 3) || Session.GetHabbo().Rank == 17)
						{

							Session.GetHabbo().lastsex = GalaxyServer.GetIUnixTimestamp() + 300;

							if (Session.GetHabbo().Rank == 17 || Session.GetHabbo().Username == "PHB")
								Session.GetHabbo().lastsex = GalaxyServer.GetIUnixTimestamp();

							string QuemLeva = TargetClient.GetHabbo().Username;
							string QuemLeva2 = TargetClient2.GetHabbo().Username;
							string QuemCome = Session.GetHabbo().Username;

							int Efeito1 = ThisUser.CurrentEffect;
							int Efeito2 = TargetUser.CurrentEffect;
							int Efeito3 = TargetUser2.CurrentEffect;

							#region Sexo Gay M-M-M OK
							if (Session.GetHabbo().Gender.ToLower() == "m" && TargetClient.GetHabbo().Gender.ToLower() == "m" && TargetClient2.GetHabbo().Gender.ToLower() == "m")
							{
								ThisUser.ApplyEffect(507);
								TargetUser.ApplyEffect(507);
								TargetUser2.ApplyEffect(507);
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Botando no buraco do " + QuemLeva + " e mamando o pau do " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Huuum que delícia, vou gozar na sua boquinha bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Ohaaaaaaaaaaaaaar, que pau grosso hein " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Verdade, esse piruzão é bem grosso mesmo*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Não enfia tudo, tá doendo..*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Enfia o pau no " + QuemLeva + " com força*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Não bota tudo que eu piro aloka*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Goza na boca do " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Ain que leitinho gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Na próxima vez eu quero sentir esse pauzão gostoso em mim*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Vai não, o pau dele é todo meu*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Goza no cú do " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Adorei ser sua passivinha, quero mais*", 0, 16));
								ThisUser.ApplyEffect(0);
								TargetUser.ApplyEffect(0);
								TargetUser2.ApplyEffect(0);
							}
							#endregion
							#region Sexo Bi M-M-F OK
							else if (Session.GetHabbo().Gender.ToLower() == "m" && TargetClient.GetHabbo().Gender.ToLower() == "m" && TargetClient2.GetHabbo().Gender.ToLower() == "f")
							{
								ThisUser.ApplyEffect(507);
								TargetUser.ApplyEffect(507);
								TargetUser2.ApplyEffect(507);
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Fazendo dupla penetração com " + QuemLeva + " e com o " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Bota um pau no cú e um na buceta*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Huuum que bucetinha gostosa bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Que cuzinho apertado, vou deixar ele todo gozado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Enfia com força na " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Não enfia tudo, tá doendo..*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Segura nas tetas da " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Puxa os cabelos da " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Ain que delícia, me fode gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Goza na bucetinha da " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Deixa o cuzinho da " + QuemLeva2 + " todo arrombado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Goza no cú da " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Adorei esse menage, estou arrombada, quero mais*", 0, 16));
								ThisUser.ApplyEffect(0);
								TargetUser.ApplyEffect(0);
								TargetUser2.ApplyEffect(0);
							}
							#endregion
							#region Sexo Bi M-F-M OK
							else if (Session.GetHabbo().Gender.ToLower() == "m" && TargetClient.GetHabbo().Gender.ToLower() == "f" && TargetClient2.GetHabbo().Gender.ToLower() == "m")
							{
								ThisUser.ApplyEffect(507);
								TargetUser.ApplyEffect(507);
								TargetUser2.ApplyEffect(507);
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Fazendo dupla penetração com " + QuemLeva2 + " e com o " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Bota um pau no cú e um na buceta*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Huuum que bucetinha gostosa bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Que cuzinho apertado, vou deixar ele todo gozado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Enfia com força na " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Não enfia tudo, tá doendo..*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Segura nas tetas da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Puxa os cabelos da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Ain que delícia, me fode gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Goza na bucetinha da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Deixa o cuzinho da " + QuemLeva + " todo arrombado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Goza no cú da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Adorei esse menage, estou arrombada, quero mais*", 0, 16));
								ThisUser.ApplyEffect(0);
								TargetUser.ApplyEffect(0);
								TargetUser2.ApplyEffect(0);
							}
							#endregion
							#region Sexo Bi M-F-M OK
							else if (Session.GetHabbo().Gender.ToLower() == "m" && TargetClient.GetHabbo().Gender.ToLower() == "f" && TargetClient2.GetHabbo().Gender.ToLower() == "m")
							{
								ThisUser.ApplyEffect(507);
								TargetUser.ApplyEffect(507);
								TargetUser2.ApplyEffect(507);
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Fazendo dupla penetração com " + QuemLeva + " e com o " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Bota um pau no cú e um na buceta*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Huuum que bucetinha gostosa bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Que cuzinho apertado, vou deixar ele todo gozado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Enfia com força na " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Não enfia tudo, tá doendo..*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Segura nas tetas da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Puxa os cabelos da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Ain que delícia, me fode gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Goza na bucetinha da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Deixa o cuzinho da " + QuemLeva + " todo arrombado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Goza no cú da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Adorei esse menage, estou arrombada, quero mais*", 0, 16));
								ThisUser.ApplyEffect(0);
								TargetUser.ApplyEffect(0);
								TargetUser2.ApplyEffect(0);
							}
							#endregion
							#region Sexo Bi F-M-M OK
							else if (Session.GetHabbo().Gender.ToLower() == "f" && TargetClient.GetHabbo().Gender.ToLower() == "m" && TargetClient2.GetHabbo().Gender.ToLower() == "m")
							{
								ThisUser.ApplyEffect(507);
								TargetUser.ApplyEffect(507);
								TargetUser2.ApplyEffect(507);
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Fazendo dupla penetração com " + QuemLeva + " e com o " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Bota um pau no cú e um na buceta*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Huuum que bucetinha gostosa bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Que cuzinho apertado, vou deixar ele todo gozado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Enfia com força na " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Não enfia tudo, tá doendo..*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Segura nas tetas da " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Puxa os cabelos da " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Ain que delícia, me fode gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Goza na bucetinha da " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Deixa o cuzinho da " + QuemCome + " todo arrombado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Goza no cú da " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Adorei esse menage, estou arrombada, quero mais*", 0, 16));
								ThisUser.ApplyEffect(0);
								TargetUser.ApplyEffect(0);
								TargetUser2.ApplyEffect(0);
							}
							#endregion
							#region Sexo Bi M-F-F OK
							else if (Session.GetHabbo().Gender.ToLower() == "m" && TargetClient.GetHabbo().Gender.ToLower() == "f" && TargetClient2.GetHabbo().Gender.ToLower() == "f")
							{
								ThisUser.ApplyEffect(507);
								TargetUser.ApplyEffect(507);
								TargetUser2.ApplyEffect(507);
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Bota a calcinha de lado e pula na rola do " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Huuum que bucetinha gostosa bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Chupa a buceta da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Fazendo o dj na " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Aiiin que dj gostoso, cai de boca na minha ppk bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Dando dedada no cuzinho dela*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Botando só a cabecinha no cuzinho da cuzinho da " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Enfia essa piroca gostosa toda, sem miséria aqui seu gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Ain que delícia, me fode gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Goza na boquinha da " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Deixa o cuzinho da " + QuemLeva + " todo arrombado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Goza no cú da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Adorei esse menage, estou arrombada, quero mais*", 0, 16));
								ThisUser.ApplyEffect(0);
								TargetUser.ApplyEffect(0);
								TargetUser2.ApplyEffect(0);
							}
							#endregion
							#region Sexo Bi F-M-F OK
							else if (Session.GetHabbo().Gender.ToLower() == "f" && TargetClient.GetHabbo().Gender.ToLower() == "m" && TargetClient2.GetHabbo().Gender.ToLower() == "f")
							{
								ThisUser.ApplyEffect(507);
								TargetUser.ApplyEffect(507);
								TargetUser2.ApplyEffect(507);
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Bota a calcinha de lado e pula na rola do " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Huuum que bucetinha gostosa bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Chupa a buceta da " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Fazendo o dj na " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Aiiin que dj gostoso, cai de boca na minha ppk bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Enfia só a cabecinha no cuzinho dela*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Enfia essa piroca gostosa toda, sem miséria aqui seu gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Ain que delícia, me fode gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Goza na boquinha da " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Deixa o cuzinho da " + QuemCome + " todo arrombado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Goza no cú da " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Adorei esse menage, estou arrombada, quero mais*", 0, 16));
								ThisUser.ApplyEffect(0);
								TargetUser.ApplyEffect(0);
								TargetUser2.ApplyEffect(0);
							}
							#endregion
							#region Sexo Bi F-F-M OK
							else if (Session.GetHabbo().Gender.ToLower() == "f" && TargetClient.GetHabbo().Gender.ToLower() == "f" && TargetClient2.GetHabbo().Gender.ToLower() == "m")
							{
								ThisUser.ApplyEffect(507);
								TargetUser.ApplyEffect(507);
								TargetUser2.ApplyEffect(507);
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Bota a calcinha de lado e pula na rola do " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Huuum que bucetinha gostosa bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Chupa a buceta da " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Fazendo o dj na " + QuemLeva + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Aiiin que dj gostoso, cai de boca na minha ppk bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Enfia só a cabecinha no cuzinho dela*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Enfia essa piroca gostosa toda, sem miséria aqui seu gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Ain que delícia, me fode gostoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Goza na boquinha da " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Deixa o cuzinho da " + QuemCome + " todo arrombado*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Goza no cú da " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Adorei esse menage, estou arrombada, quero mais*", 0, 16));
								ThisUser.ApplyEffect(0);
								TargetUser.ApplyEffect(0);
								TargetUser2.ApplyEffect(0);
							}
							#endregion
							#region Sexo Lésbico F-F-F OK
							if (Session.GetHabbo().Gender.ToLower() == "f" && TargetClient.GetHabbo().Gender.ToLower() == "f" && TargetClient2.GetHabbo().Gender.ToLower() == "f")
							{
								ThisUser.ApplyEffect(507);
								TargetUser.ApplyEffect(507);
								TargetUser2.ApplyEffect(507);
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Chupando a xoxota da " + QuemLeva + " enquanto dá dedada na " + QuemLeva2 + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Huuum que delícia, goza na minha boquinha bebê*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Awn que dedos gostosos hein " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Espera só para experimentar a boquinha maravilhosa dela*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Awwwn eu vou gozar*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Inverte as posições*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Huuum que boquinha gostosa, chupa meu clitóris bebe*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser2.VirtualId, "*Awwn que dj maravilhoso*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Goza na boquinha da " + QuemCome + "*", 0, 16));
								await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); TargetUser2.ApplyEffect(Efeito3); return; }
								Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Huuum que melzinho gostoso ƒ*", 0, 16));
								ThisUser.ApplyEffect(0);
								TargetUser.ApplyEffect(0);
								TargetUser2.ApplyEffect(0);
							}
							#endregion

							ThisUser.ApplyEffect(Efeito1);
							TargetUser.ApplyEffect(Efeito2);
							TargetUser2.ApplyEffect(Efeito3);

							TargetClient.SendWhisper("Use o comando :disablesex para bloquear o sexo!");
						}
						else
						{

							Session.SendWhisper("Chegue mais perto das pessoas.");
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
					Session.SendWhisper("Um dos usuários estão com o comando desativado.");
				}

			} catch (Exception e)
			{

			}
	}
	} 
}