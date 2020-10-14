using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Communication.Packets.Outgoing.Rooms.Avatar;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Pathfinding;
using Galaxy.HabboHotel.Rooms;
using System;
using System.Data;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Action
{
    class AmbassadorWarningMessageEvent : IPacketEvent
    {
		public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
		{
			try
			{

				{
					int UserId = Packet.PopInt();
					int RoomId = Packet.PopInt();
					int Time = Packet.PopInt();
					string HotelName = GalaxyServer.HotelName;

					Room Room = Session.GetHabbo().CurrentRoom;
					RoomUser Target = Room.GetRoomUserManager().GetRoomUserByHabbo(GalaxyServer.GetUsernameById(UserId));
					if (Target == null)
						return;
					RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

					if (Session.GetHabbo().Rank < 5)
					{
						DataRow BlockCMD = null;
						using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
						{
							dbClient.SetQuery("SELECT id FROM room_blockcmd WHERE room = @room AND command = @command LIMIT 1");
							dbClient.AddParameter("command", "beijar");
							dbClient.AddParameter("room", Session.GetHabbo().CurrentRoomId);
							BlockCMD = dbClient.getRow();
							if (BlockCMD != null)
							{
								Session.SendWhisper("Você não pode usar esse comando nesse quarto.");
								return;
							}
						}
					}

					/// Cooldown
					if (Session.GetHabbo().UltimoBeijo > 0)
					{
						if ((System.Convert.ToInt32(GalaxyServer.GetIUnixTimestamp()) - Session.GetHabbo().UltimoBeijo) <= 5)
						{
							Session.SendWhisper("Espere alguns segundos para beijar novamente.");
							return;
						}
					}
					if (User != null)
					{
						User.UnIdle();

						if ((Math.Abs((int)(Target.X - User.X)) < 2) && (Math.Abs((int)(Target.Y - User.Y)) < 2) || User.GetUsername() == "PHB")
						{

							int Rot = Rotation.Calculate(User.X, User.Y, Target.X, Target.Y); /// vira o usuário
							User.SetRot(Rot, false);
							User.UpdateNeeded = true;

							int Rot2 = Rotation.Calculate(Target.X, Target.Y, User.X, User.Y); /// vira o usuário
							Target.SetRot(Rot2, false);
							Target.UpdateNeeded = true;

							System.Threading.Thread.Sleep(600); /// delay

							Room.SendMessage(new ActionComposer(User.VirtualId, 2));
							Room.SendMessage(new ActionComposer(Target.VirtualId, 2));

							Room.SendMessage(new ChatComposer(User.VirtualId, "*Beijando " + Target.GetUsername() + "*", 0, 16));
							Session.GetHabbo().UltimoBeijo = System.Convert.ToInt32(GalaxyServer.GetIUnixTimestamp());

						}
						else
						{
							Session.SendWhisper("Ops! O usuário não está perto de você!");
							return;
						}
					}

					/* 




						 if (Session.GetHabbo().Rank == 1)
					   {
						   Session.SendWhisper("Para de usar tanji :c");
						   return;
					   }

					   if (Session.GetHabbo().Rank < ExtraSettings.AmbassadorMinRank)
						   return;

					   Room Room = Session.GetHabbo().CurrentRoom;
					   RoomUser Target = Room.GetRoomUserManager().GetRoomUserByHabbo(GalaxyServer.GetUsernameById(UserId));
					   if (Target == null)
						   return;

					   long nowTime = GalaxyServer.CurrentTimeMillis();
					   long timeBetween = nowTime - Session.GetHabbo()._lastTimeUsedHelpCommand;
					   if (timeBetween < 60000)
					   {
						   Session.SendMessage(RoomNotificationComposer.SendBubble("Abuso", "Espere pelo menos 1 minuto para enviar um novo alerta.", ""));
						   return;
					   }

					   else
						   GalaxyServer.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("advice", "" + Session.GetHabbo().Username + " acaba de mandar um alerta para " + Target.GetClient().GetHabbo().Username + ", clique aqui para ir.", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
					   Target.GetClient().SendMessage(new BroadcastMessageAlertComposer("<b><font size='15px' color='#c40101'>Mensagem de um embaixador:<br></font></b>os embaixadores do " + HotelName + " Hotel consideram que o seu comportamento não é adequado. Por favor, reconsidere a sua atitude, antes que algum moderador tome medidas drásticas!"));

					   Session.GetHabbo()._lastTimeUsedHelpCommand = nowTime;
					   */
				}
			} catch (Exception e)
			{

			}
		}
	}
}
