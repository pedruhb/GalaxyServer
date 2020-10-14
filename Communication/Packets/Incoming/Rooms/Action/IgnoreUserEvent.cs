using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using Galaxy.Communication.Packets.Outgoing.Rooms.Action;
using System;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Action
{
    class IgnoreUserEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            if (!session.GetHabbo().InRoom)
                return;

            Room Room = session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            string Username = packet.PopString();

            Habbo Player = GalaxyServer.GetHabboByUsername(Username);
            if (Player == null)
                return;

			RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Player.Id);
			if (TargetUser == null)
			{
				session.SendWhisper("Ocorreu um erro, parece que o usuário não está na sala e nem no quarto!");
				return;
			}
			RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
			if (ThisUser == null)
				return;

			if (!((Math.Abs(TargetUser.X - ThisUser.X) >= 2) || (Math.Abs(TargetUser.Y - ThisUser.Y) >= 2)))
			{
				if (TargetUser.SetX - 1 == Room.GetGameMap().Model.DoorX)
				{
					session.SendWhisper("Não faça esse usuário sair do quarto :(!");
					return;
				}

				if (TargetUser.RotBody == 4)
				{
					TargetUser.MoveTo(TargetUser.X, TargetUser.Y + 1);
				}

				if (ThisUser.RotBody == 0)
				{
					TargetUser.MoveTo(TargetUser.X, TargetUser.Y - 1);
				}

				if (ThisUser.RotBody == 6)
				{
					TargetUser.MoveTo(TargetUser.X - 1, TargetUser.Y);
				}

				if (ThisUser.RotBody == 2)
				{
					TargetUser.MoveTo(TargetUser.X + 1, TargetUser.Y);
				}

				if (ThisUser.RotBody == 3)
				{
					TargetUser.MoveTo(TargetUser.X + 1, TargetUser.Y);
					TargetUser.MoveTo(TargetUser.X, TargetUser.Y + 1);
				}

				if (ThisUser.RotBody == 1)
				{
					TargetUser.MoveTo(TargetUser.X + 1, TargetUser.Y);
					TargetUser.MoveTo(TargetUser.X, TargetUser.Y - 1);
				}

				if (ThisUser.RotBody == 7)
				{
					TargetUser.MoveTo(TargetUser.X - 1, TargetUser.Y);
					TargetUser.MoveTo(TargetUser.X, TargetUser.Y - 1);
				}

				if (ThisUser.RotBody == 5)
				{
					TargetUser.MoveTo(TargetUser.X - 1, TargetUser.Y);
					TargetUser.MoveTo(TargetUser.X, TargetUser.Y + 1);
				}
				TargetUser.GetClient().SendWhisper("Você foi empurrado por " + session.GetHabbo().Username + "!");
				session.SendWhisper("Você empurrou " + Username + "!");
			}
			else
			{
				session.SendWhisper("Ops, " + Username + " está longe demais!");
			}
		}
    }
}
