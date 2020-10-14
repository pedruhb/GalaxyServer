using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Threading.Tasks;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class BoqueteCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_boquete"; }
		}
		public string Parameters
		{
			get { return "[USUARIO]"; }
		}
		public string Description
		{
			get { return "Paga um boquete em alguém."; }
		}
		public bool checkuser(string username, GameClient Session, Room Room)
		{

			GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(username);
			RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);

			if (TargetClient == null)
			{
				Session.SendWhisper("O usuário saiu do quarto, o comando foi cancelado.");
				return false;
			}
			else if (TargetUser == null)
			{
				Session.SendWhisper("O usuário saiu do quarto, o comando foi cancelado.");
				return false;
			}
			return true;
		}
		public async void Execute(GameClient Session, Room Room, string[] Params)
		{

			try { 
			if (Params.Length == 1)
			{
				Session.SendWhisper("Digite o nick de quem você deseja mamar.");
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
				Session.SendWhisper("Você não pode fazer um boquete em você mesmo!");
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

						if (Session.GetHabbo().Rank == 17)
							Session.GetHabbo().lastsex = GalaxyServer.GetIUnixTimestamp();

						string QuemLeva = TargetClient.GetHabbo().Username;
						string QuemCome = Session.GetHabbo().Username;

						int Efeito1 = ThisUser.CurrentEffect;
						int Efeito2 = TargetUser.CurrentEffect;

						if (Session.GetHabbo().Gender.ToLower() == "m" && TargetClient.GetHabbo().Gender.ToLower() == "m" || Session.GetHabbo().Gender.ToLower() == "f" && TargetClient.GetHabbo().Gender.ToLower() == "m")
						{
							ThisUser.ApplyEffect(507);
							TargetUser.ApplyEffect(507);
							Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Coloca a mão dentro da cueca do " + QuemLeva + "*", 0, 16));
							await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
							Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Passa a lingua na cabecinha da rola do " + QuemLeva + "*", 0, 16));
							await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
							Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Ohaaaaaaaaaaaaaar*", 0, 16));
							await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
							Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Que chupada gostosa " + QuemCome + "*", 0, 16));
							await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
							Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Faz garganta profunda*", 0, 16));
							await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
							Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Glub Glub Glub*", 0, 16));
							await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
							Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Ain que leitinho gostoso*", 0, 16));
							await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
							Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Aahhhhhhhhhhhhhhr, gozei gostoso na sua boquinha bebê*", 0, 16));
							await Task.Delay(1000); if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(Efeito1); return; }
							ThisUser.ApplyEffect(0);
							TargetUser.ApplyEffect(0);
						}
						else if (TargetClient.GetHabbo().Gender.ToLower() == "f")
						{
							Session.SendWhisper("Você não pode pagar boquete em uma mulher...");
							return;
						}

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
			}
			catch (Exception)
			{

			}
		}
	}
}