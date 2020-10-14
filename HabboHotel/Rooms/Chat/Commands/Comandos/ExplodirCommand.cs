using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Users;
using System;
using System.Threading.Tasks;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ExplodirCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_explodir"; }
        }
        public string Parameters
        {
            get { return "[USUARIO]"; }
        }
        public string Description
        {
            get { return "Exploda alguém."; }
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

		public async void Execute(GameClient Session, Rooms.Room Room, string[] Params)
		{
			try
			{
				if (Params.Length == 1)
				{
					Session.SendWhisper("Digite o nick de quem você deseja Explodir.");
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
					Session.SendWhisper("Você não pode explodir você mesmo!");
					return;
				}
				RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
				if (ThisUser == null)
					return;

				Habbo Habbo = GalaxyServer.GetHabboByUsername(Params[1]);

				if (GalaxyServer.GetIUnixTimestamp() >= Session.GetHabbo().UltimoExplodir)
				{

					Session.GetHabbo().UltimoExplodir = GalaxyServer.GetIUnixTimestamp() + 300;

					if (Session.GetHabbo().Id == 1 || Session.GetHabbo().Rank == 17)
						Session.GetHabbo().UltimoExplodir = GalaxyServer.GetIUnixTimestamp();

					int TargetEfeitoOld = TargetUser.CurrentEffect;
					Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Jogou bomba no(a) " + TargetUser.GetUsername() + "*", 0, ThisUser.LastBubble));
					await Task.Delay(1000);
					if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(TargetEfeitoOld); return; }
					TargetUser.ApplyEffect(108);
					await Task.Delay(1000);
					if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(TargetEfeitoOld); return; }
					Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*MORRI*", 0, TargetUser.LastBubble));
					TargetUser.ApplyEffect(93);
					Habbo.GetClient().SendWhisper("Você foi explodido! Irá renascer em 2 segundos...");
					await Task.Delay(2000);
					if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(TargetEfeitoOld); return; }
					TargetUser.ApplyEffect(23);
					await Task.Delay(500);
					if (checkuser(Params[1], Session, Room) == false) { ThisUser.ApplyEffect(TargetEfeitoOld); return; }
					Room.SendMessage(new ChatComposer(TargetUser.VirtualId, "*Renascido com Sucesso*", 0, TargetUser.LastBubble));
					TargetUser.ApplyEffect(TargetEfeitoOld);
					return;
				}
				else
				{
					int diferenca = (GalaxyServer.GetIUnixTimestamp() - Session.GetHabbo().UltimoExplodir);
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
			catch (Exception e)
			{
			}
		}
    }
}
