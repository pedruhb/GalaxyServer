
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.Rooms;
using System.Data;
using Galaxy.HabboHotel.Users;

namespace Galaxy.Communication.RCON.Commands.User
{
	class ResponderMencao : IRCONCommand
	{
		public string Description
		{
			get { return "Responde a menção do usuário."; }
		}

		public string Parameters
		{
			get { return "%userid% %dest-username%"; }
		}

		public bool TryExecute(string[] parameters)
		{

			int SessionID = System.Convert.ToInt32(parameters[0]);
			string TargetUsername = System.Convert.ToString(parameters[1]);

			Habbo Session = GalaxyServer.GetHabboById(SessionID);

			Session.GetClient().SendWhisper("A sua próxima mensagem será encaminhada automaticamente para o usuário \"" + TargetUsername + "\".");
			Session.GetClient().GetHabbo().RespostaMencao = TargetUsername;

			return true;

		}
	}
}