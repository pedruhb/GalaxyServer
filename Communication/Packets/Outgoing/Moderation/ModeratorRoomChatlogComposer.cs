using Galaxy.Utilities;
using Galaxy.HabboHotel.Rooms;
using System.Collections.Generic;
using Galaxy.HabboHotel.Rooms.Chat.Logs;

namespace Galaxy.Communication.Packets.Outgoing.Moderation
{
    class ModeratorRoomChatlogComposer : ServerPacket
    {
        public ModeratorRoomChatlogComposer(Room Room, ICollection<ChatlogEntry> chats)
            : base(ServerPacketHeader.ModeratorRoomChatlogMessageComposer)
        {
			WriteByte(1);
			WriteShort(2);//Count
			WriteString("roomName");
			WriteByte(2);
			WriteString(Room.Name);
			WriteString("roomId");
			WriteByte(1);
			WriteInteger(Room.Id);

			WriteShort(chats.Count);
            foreach (ChatlogEntry Entry in chats)
            {
                string Username = "Desconhecido";
                if (Entry.PlayerNullable() != null)
                {
                    Username = Entry.PlayerNullable().Username;
                }

				WriteString(UnixTimestamp.FromUnixTimestamp(Entry.Timestamp).ToShortTimeString()); // time?
				WriteInteger(Entry.PlayerId); // User Id
				WriteString(Username); // Username
				WriteString(!string.IsNullOrEmpty(Entry.Message) ? Entry.Message : "*O usuário enviou uma mensagem em branco*"); // Message        
				WriteBoolean(false); //TODO, AI's?
            }
        }
    }
}
