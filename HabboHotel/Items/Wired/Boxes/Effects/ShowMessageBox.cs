using System;
using System.Collections.Concurrent;

using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;
using System.Collections.Generic;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
	class ShowMessageBox : IWiredItem
	{
		public Room Instance { get; set; }

		public Item Item { get; set; }

		public WiredBoxType Type { get { return WiredBoxType.EffectShowMessage; } }

		public ConcurrentDictionary<int, Item> SetItems { get; set; }

		public string StringData { get; set; }

		public string Message2 { get; set; }

		public bool BoolData { get; set; }

		public string ItemsData { get; set; }

		public ShowMessageBox(Room Instance, Item Item)
		{
			this.Instance = Instance;
			this.Item = Item;
			this.SetItems = new ConcurrentDictionary<int, Item>();
		}

		public void HandleSave(ClientPacket Packet)
		{
			int Unknown = Packet.PopInt();
			string Message = Packet.PopString();

			this.StringData = Message;
		}

		public bool Execute(params object[] Params)
		{
			if (Params == null || Params.Length == 0)
				return false;

			Habbo Player = (Habbo)Params[0];
			if (Player == null || Player.GetClient() == null || string.IsNullOrWhiteSpace(StringData))
				return false;

			RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
			if (User == null)
				return false;

			string Message = StringData;
			string MessageFiltered = StringData;

			if (StringData.Contains("%user%")) MessageFiltered = MessageFiltered.Replace("%user%", Player.Username);
			if (StringData.Contains("%username%")) MessageFiltered = MessageFiltered.Replace("%username%", Player.Username);
			if (StringData.Contains("%userid%")) MessageFiltered = MessageFiltered.Replace("%userid%", Convert.ToString(Player.Id));
			if (StringData.Contains("%gotw%")) MessageFiltered = MessageFiltered.Replace("%gotw%", Convert.ToString(Player.GOTWPoints));
			if (StringData.Contains("%duckets%")) MessageFiltered = MessageFiltered.Replace("%duckets%", Convert.ToString(Player.Duckets));
			if (StringData.Contains("%diamonds%")) MessageFiltered = MessageFiltered.Replace("%diamonds%", Convert.ToString(Player.Diamonds));
			if (StringData.Contains("%rank%")) MessageFiltered = MessageFiltered.Replace("%rank%", Convert.ToString(Player.Rank));
			if (StringData.Contains("%roomname%")) MessageFiltered = MessageFiltered.Replace("%roomname%", Player.CurrentRoom.Name);
			if (StringData.Contains("%roomusers%")) MessageFiltered = MessageFiltered.Replace("%roomusers%", Player.CurrentRoom.UserCount.ToString());
			if (StringData.Contains("%roomowner%")) MessageFiltered = MessageFiltered.Replace("%roomowner%", Player.CurrentRoom.OwnerName.ToString());
			if (StringData.Contains("%roomlikes%")) MessageFiltered = MessageFiltered.Replace("%roomlikes%", Player.CurrentRoom.Score.ToString());
			if (StringData.Contains("%hotelname%")) MessageFiltered = MessageFiltered.Replace("%hotelname%", GalaxyServer.HotelName);
			if (StringData.Contains("%versaoGalaxy%")) MessageFiltered = MessageFiltered.Replace("%versaoGalaxy%", GalaxyServer.VersionGalaxy);
			if (StringData.Contains("%userson%")) MessageFiltered = MessageFiltered.Replace("%userson%", GalaxyServer.GetGame().GetClientManager().Count.ToString());
			if (StringData.Contains("%dolar%")) MessageFiltered = MessageFiltered.Replace("%dolar%", GalaxyServer.CotacaoDolar);
			

			if (StringData.Contains("%sit%"))
			{
				MessageFiltered = MessageFiltered.Replace("%sit%", "Você sentou");

				if (User.Statusses.ContainsKey("lie") || User.isLying || User.RidingHorse || User.IsWalking)
					return false;

				if (!User.Statusses.ContainsKey("sit"))
				{
					if ((User.RotBody % 2) == 0)
					{
						if (User == null)
							return false;

						try
						{
							User.Statusses.Add("sit", "1.0");
							User.Z -= 0.35;
							User.isSitting = true;
							User.UpdateNeeded = true;
						}
						catch { }
					}
					else
					{
						User.RotBody--;
						User.Statusses.Add("sit", "1.0");
						User.Z -= 0.35;
						User.isSitting = true;
						User.UpdateNeeded = true;
					}
				}
				else if (User.isSitting == true)
				{
					User.Z += 0.35;
					User.Statusses.Remove("sit");
					User.Statusses.Remove("1.0");
					User.isSitting = false;
					User.UpdateNeeded = true;
				}
			}

			if (StringData.Contains("%stand%"))
			{
				MessageFiltered = MessageFiltered.Replace("%stand%", "Você levantou");
				if (User.isSitting)
				{
					User.Statusses.Remove("sit");
					User.Z += 0.35;
					User.isSitting = false;
					User.UpdateNeeded = true;
				}
				else if (User.isLying)
				{
					User.Statusses.Remove("lay");
					User.Z += 0.35;
					User.isLying = false;
					User.UpdateNeeded = true;
				}
			}

			if (StringData.Contains("%lay%"))
			{
				MessageFiltered = MessageFiltered.Replace("%lay%", "Você deitou");

				Room Room = Player.GetClient().GetHabbo().CurrentRoom;

				if (!Room.GetGameMap().ValidTile(User.X + 2, User.Y + 2) && !Room.GetGameMap().ValidTile(User.X + 1, User.Y + 1))
				{
					Player.GetClient().SendWhisper("Oops, você não pode deitar aqui");
					return false;
				}

				if (User.Statusses.ContainsKey("sit") || User.isSitting || User.RidingHorse || User.IsWalking)
					return false;

				if (Player.GetClient().GetHabbo().Effects().CurrentEffect > 0)
					Player.GetClient().GetHabbo().Effects().ApplyEffect(0);

				if (!User.Statusses.ContainsKey("lay"))
				{
					if ((User.RotBody % 2) == 0)
					{
						if (User == null)
							return false;

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

			Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, MessageFiltered, 0, 34));
			return true;

		}
	}
}
