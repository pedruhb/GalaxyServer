using System;
using System.Linq;
using System.Collections.Generic;

using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.Users.Messenger;
using Galaxy.HabboHotel.Users.Relationships;

namespace Galaxy.Communication.Packets.Outgoing.Messenger
{
	class BuddyListComposer : ServerPacket
    {
        public BuddyListComposer(ICollection<MessengerBuddy> Friends, Habbo Player)
            : base(ServerPacketHeader.BuddyListMessageComposer)
        {
            var friendCount = Friends.Count;
            if (Player.Rank >= 10) friendCount++;

            WriteInteger(1);
			WriteInteger(0);
            var groups = GalaxyServer.GetGame().GetGroupManager().GetGroupsForUser(Player.Id).Where(c => c.HasChat).ToList();
			WriteInteger(friendCount + groups.Count);

            foreach (var gp in groups)
            {
				WriteInteger(int.MinValue + gp.Id);
				WriteString(gp.Name);
				WriteInteger(1);//Gender.
				WriteBoolean(true);
				WriteBoolean(false);
				WriteString(gp.Badge);
				WriteInteger(1); // category id
				WriteString(string.Empty);
				WriteString("Chat de Grupo");//Alternative name?
				WriteString(string.Empty);
				WriteBoolean(true);
				WriteBoolean(false);
				WriteBoolean(false);//Pocket Habbo user.
				WriteShort(0);
            }

            foreach (MessengerBuddy Friend in Friends.ToList())
            {
                Relationship Relationship = Player.Relationships.FirstOrDefault(x => x.Value.UserId == Convert.ToInt32(Friend.UserId)).Value;

				WriteInteger(Friend.Id);
				WriteString(Friend.mUsername);
				WriteInteger(1);//Gender.
				WriteBoolean(Friend.IsOnline);
				WriteBoolean(Friend.IsOnline && Friend.InRoom);
				WriteString(Friend.mLook);
				WriteInteger(0); // category id
				WriteString(Friend.IsOnline ? Friend.mMotto : string.Empty);
				WriteString(string.Empty);//Alternative name?
				WriteString(string.Empty);
				WriteBoolean(true);
				WriteBoolean(false);
				WriteBoolean(false);//Pocket Habbo user.
				WriteShort(Relationship == null ? 0 : Relationship.Type);
            }

            #region Custom Chats
            if (Player.Rank >= 10)
            {
                base.WriteInteger(int.MinValue);  // Int.MaxValue chat staff
                base.WriteString("Staff");
                base.WriteInteger(1);
                base.WriteBoolean(true);
                base.WriteBoolean(false);
                base.WriteString("b10224s02137s70107");
                base.WriteInteger(1);
                base.WriteString(string.Empty);
                base.WriteString("Gestão do Hotel");
                base.WriteString(string.Empty);
                base.WriteBoolean(true);
                base.WriteBoolean(false);
                base.WriteBoolean(false);
                base.WriteShort(0);
            }
            #endregion

        }
    }
}
