using Galaxy.Communication.Interfaces;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using System;
using System.Threading;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    internal class WeedCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get
			{
				return "command_fumar";
			}
		}

		public string Parameters
		{
			get
			{
				return "[SIM]";
			}
		}

		public string Description
		{
			get
			{
				return "Fumar um baseado";
			}
		}

		public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
		{

			RoomUser roomUserByHabbo = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (roomUserByHabbo == null)
				return;

				roomUserByHabbo.GetClient().SendWhisper("Comprou Maconha!");
				Thread.Sleep(1000);
				Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "*Enrola baseado *", 0, 6), false);
				Thread.Sleep(2000);
				Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "*vou acender e começar a fumar*", 0, 6), false);
				Thread.Sleep(2000);
				roomUserByHabbo.ApplyEffect(53);
				Thread.Sleep(2000);
				switch (new Random().Next(1, 4))
				{
					case 1:
						Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "Hehehe Eu vejo muitas aves :D  ", 0, 6), false);
						break;
					case 2:
						roomUserByHabbo.ApplyEffect(70);
						Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "Eu me sinto um panda :D ", 0, 6), false);
						break;
					default:
						Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "Hehehe to muito chapado :D ", 0, 6), false);
						break;
				}
				Thread.Sleep(2000);
				Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "HAHAAHHAHAHAHAAHAHAHHAHAHAHA LOL", 0, 6), false);
				Thread.Sleep(2000);
				roomUserByHabbo.ApplyEffect(0);
				Thread.Sleep(2000);
				Room.SendMessage((IServerPacket)new ChatComposer(roomUserByHabbo.VirtualId, "*Que Maconha boa que eu comprei *", 0, 6), false);
				Thread.Sleep(2000);
			

		}
	}
}