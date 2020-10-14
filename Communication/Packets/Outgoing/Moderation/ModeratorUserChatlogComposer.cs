﻿using System.Collections.Generic;
using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Rooms.Chat.Logs;
using Galaxy.Utilities;

namespace Galaxy.Communication.Packets.Outgoing.Moderation
{
    class ModeratorUserChatlogComposer : ServerPacket
    {
        public ModeratorUserChatlogComposer(Habbo habbo, List<KeyValuePair<RoomData, List<ChatlogEntry>>> chatlogs)
            : base(ServerPacketHeader.ModeratorUserChatlogMessageComposer)
        {
			WriteInteger(habbo.Id);
			WriteString(habbo.Username);

			WriteInteger(chatlogs.Count); // Room Visits Count
            foreach (KeyValuePair<RoomData, List<ChatlogEntry>> Chatlog in chatlogs)
            {
				WriteByte(1);
				WriteShort(2);//Count
				WriteString("roomName");
				WriteByte(2);
				WriteString(Chatlog.Key.Name); // room name
				WriteString("roomId");
				WriteByte(1);
				WriteInteger(Chatlog.Key.Id);

				WriteShort(Chatlog.Value.Count); // Chatlogs Count
                foreach (ChatlogEntry Entry in Chatlog.Value)
                {
                    string Username = "Desconhecido";
                    if (Entry.PlayerNullable() != null)
                    {
                        Username = Entry.PlayerNullable().Username;
                    }

					WriteString(UnixTimestamp.FromUnixTimestamp(Entry.Timestamp).ToShortTimeString());
					WriteInteger(Entry.PlayerId); // UserId of message
					WriteString(Username); // Username of message
					WriteString(!string.IsNullOrEmpty(Entry.Message) ? Entry.Message : "*O usuário enviou uma mensagem em branco*"); // Message        
					WriteBoolean(habbo.Id == Entry.PlayerId);
                }
            }
        }
    }
}