using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{

    internal class BaterCommand : IChatCommand
	{
		public void Execute(GameClient Session, Room Room, string[] Params)
		{
			if (Params.Length == 1)
			{
				Session.SendWhisper("Coloque o nome do Usuário!");
			}
			else
			{
				GameClient clientByUsername = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
				if (clientByUsername == null)
				{
					Session.SendWhisper("Desculpe, mas não encontramos o usuário!");
				}
				else
				{
					RoomUser roomUserByHabbo = Room.GetRoomUserManager().GetRoomUserByHabbo(clientByUsername.GetHabbo().Id);
					if (roomUserByHabbo == null)
					{
						Session.SendWhisper("Desculpe, mas não encontramos o usuário!");
						return;
					}
					else if (clientByUsername.GetHabbo().Username == Session.GetHabbo().Username)
					{
						Session.SendWhisper("Você está louco? Você não pode bater-se seu doente.");
                        RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                        Room.SendMessage(new ChatComposer(roomUserByHabbo.VirtualId, "Me ajudem! eu sou um masoquista!", 0, ThisUser.LastBubble));
						return;
					}
					else if (roomUserByHabbo.TeleportEnabled)
					{
						Session.SendWhisper("Desculpe, o usuário está com o teletransporte ativo!");
						return;
					}
					else
					{
						RoomUser user2 = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                        RoomUser TargetID = Room.GetRoomUserManager().GetRoomUserByHabbo(clientByUsername.GetHabbo().Id);
                        RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(clientByUsername.GetHabbo().Id);
                        if (user2 != null)
						{
							if ((Math.Abs((int)(roomUserByHabbo.X - user2.X)) < 2) && (Math.Abs((int)(roomUserByHabbo.Y - user2.Y)) < 2))
							{
								Room.SendMessage(new ChatComposer(user2.VirtualId, "*Pa Pa* batendo* Toma " + roomUserByHabbo.GetUsername() + " esse socão na cara arrombado", 0, 3));
								Room.SendMessage(new ChatComposer(TargetID.VirtualId, "*Ai, isso dói* Para por favor :(", 0, 3));
                                if (!User.Statusses.ContainsKey("lay"))
                                {
                                    if ((User.RotBody % 2) == 0)
                                    {
                                        if (User == null)
                                            return;

                                        try
                                        {
                                            User.Statusses.Add("lay", "1.0 null");
                                            User.Z -= 0.35;
                                            User.isLying = true;
                                            User.UpdateNeeded = true;
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        User.RotBody--;//
                                        User.Statusses.Add("lay", "1.0 null");
                                        User.Z -= 0.35;
                                        User.isLying = true;
                                        User.UpdateNeeded = true;
                                    }

                                }
                                else
                                {
                                    User.Z += 0.35;
                                    User.Statusses.Remove("lay");
                                    User.Statusses.Remove("1.0");
                                    User.isLying = false;
                                    User.UpdateNeeded = true;
                                }
                            }
							else
							{
								Session.SendWhisper("Sentimos muito, " + Params[1] + " não está perto o suficiente!");
								return;
							}
						}
					}
				}
			}
		}

		public string Description =>
			"Bater em alguém.";

		public string Parameters =>
			"[USUARIO]";

		public string PermissionRequired =>
			"command_bater";
	}
}