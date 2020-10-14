using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Database.Interfaces;
using System.Collections.Generic;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
	class AtravessarCadeira : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_hidewired"; }
		}

		public string Parameters
		{
			get { return ""; }
		}

		public string Description
		{
			get { return "Ativa/desativa o atravessar cadeiras do quarto."; }
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{

			if (!Room.CheckRights(Session, false, false))
			{
				Session.SendWhisper("Você não pode fazer isso nesse quarto.");
				return;
			}

			Room.AtravessarCadeira = !Room.AtravessarCadeira;
			if (Room.AtravessarCadeira)
				Session.SendWhisper("Agora vários usuários podem sentar na mesma cadeira.");
			else
				Session.SendWhisper("Agora os usuários não podem mais sentar na mesma cadeira.");


			using (IQueryAdapter con = GalaxyServer.GetDatabaseManager().GetQueryReactor())
			{
				con.SetQuery("UPDATE `rooms` SET `atravessar_cadeira` = @enum WHERE `id` = @id LIMIT 1");
				con.AddParameter("enum", GalaxyServer.BoolToEnum(Room.AtravessarCadeira));
				con.AddParameter("id", Room.Id);
				con.RunQuery();

			}

			List<ServerPacket> list = new List<ServerPacket>();

			list = Room.HideWiredMessages(Room.HideWired);

			Room.SendMessage(list);


		}
	}
}
