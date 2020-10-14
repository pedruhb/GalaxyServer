using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Threading.Tasks;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MatarCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_matar"; }
		}
		public string Parameters
		{
			get { return "[USUARIO]"; }
		}
		public string Description
		{
			get { return "Mate alguém."; }
		}
		public async void Execute(GameClient Session, Room Room, string[] Params)
		{
			try
			{
				if (Params.Length == 1)
			{
				Session.SendWhisper("Digite o nick de quem você deseja matar.");
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
				Session.SendWhisper("Tá louco querendo se matar?!");
				return;
			}
			RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (ThisUser == null)
				return;

			if (GalaxyServer.GetIUnixTimestamp() >= Session.GetHabbo().UltimoMatar)
			{
				if (!(Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2) || Session.GetHabbo().Username == "PHB" || Session.GetHabbo().Rank == 17)
				{
				
						Session.GetHabbo().UltimoMatar = GalaxyServer.GetIUnixTimestamp() + 300;

						if (Session.GetHabbo().Id == 1 || Session.GetHabbo().Rank == 17)
							Session.GetHabbo().UltimoMatar = GalaxyServer.GetIUnixTimestamp();

						Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Pow Pow, Te matei " + TargetUser.GetUsername() + ", se fode aí arrombado*", 0, ThisUser.LastBubble));
						await Task.Delay(1000);
						Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Eu não esperava isso de você* :(", 0, TargetUser.LastBubble));
						if(TargetUser.isLying == false)
						TargetUser.Statusses.Add("lay", "0.1");
						TargetUser.isLying = true;
						TargetUser.UpdateNeeded = true;
					
				}
				else
				{

					Session.SendWhisper("Chegue mais perto da pessoa.");
					return;
				}
			}
			else
			{
				int diferenca = (GalaxyServer.GetIUnixTimestamp() - Session.GetHabbo().UltimoMatar);
				if (diferenca % 3600 / 60 == 0)
				{
					Session.SendWhisper("Espere alguns segundos para usar novamente esse comando.");
				}
				else
				{
					string difmenos = Convert.ToString(diferenca % 3600 / 60).Replace("-", "");
					if (difmenos == "1")
					{
						Session.SendWhisper("Espere só mais um minutinho para fazer isso novamente.");
					}
					else
					{
						Session.SendWhisper("Espere " + difmenos + " minutos para fazer isso novamente.");
					}
				}
			}
			}
			catch (Exception)
			{

			}
		}
	}
}