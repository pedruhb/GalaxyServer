using System.Linq;
using System.Collections.Generic;

using Galaxy.HabboHotel.Groups;
using Galaxy.Communication.Packets.Outgoing.Messenger;
using Galaxy.Communication.Packets.Outgoing.Groups;
using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Catalog;

namespace Galaxy.Communication.Packets.Incoming.Groups
{
    class JoinGroupEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            Group Group = null;
            if (!GalaxyServer.GetGame().GetGroupManager().TryGetGroup(Packet.PopInt(), out Group))
                return;

            if (Group.IsMember(Session.GetHabbo().Id) || Group.IsAdmin(Session.GetHabbo().Id) || (Group.HasRequest(Session.GetHabbo().Id) && Group.GroupType == GroupType.PRIVATE))
                return;

            List<Group> Groups = GalaxyServer.GetGame().GetGroupManager().GetGroupsForUser(Session.GetHabbo().Id);
            if (Groups.Count >= 1500)
            {
                Session.SendMessage(new BroadcastMessageAlertComposer("Parece que você alcançou o limite de membros em seu grupo que é 1.500."));
                return;
            }

            if (Group.GroupType == GroupType.LOCKED)
            {
                List<GameClient> GroupAdmins = (from Client in GalaxyServer.GetGame().GetClientManager().GetClients.ToList() where Client != null && Client.GetHabbo() != null && Group.IsAdmin(Client.GetHabbo().Id) select Client).ToList();
                foreach (GameClient Client in GroupAdmins)
                {
                    Client.SendMessage(new GroupMembershipRequestedComposer(Group.Id, Session.GetHabbo(), 3));
                }

                Session.SendMessage(new GroupInfoComposer(Group, Session));
            }
            else if (Group.GroupType == GroupType.PRIVATE)
            {
                Session.SendNotification("Esse grupo está fechado!");
				return;
            }
            else
            {
                Session.SendMessage(new GroupFurniConfigComposer(GalaxyServer.GetGame().GetGroupManager().GetGroupsForUser(Session.GetHabbo().Id)));
                Session.SendMessage(new GroupInfoComposer(Group, Session));

                if (Session.GetHabbo().CurrentRoom != null)
                    Session.GetHabbo().CurrentRoom.SendMessage(new RefreshFavouriteGroupComposer(Session.GetHabbo().Id));
                else
                    Session.SendMessage(new RefreshFavouriteGroupComposer(Session.GetHabbo().Id));
                if (Group.HasChat)
                {
                    Session.SendMessage(new FriendListUpdateComposer(Group, 1));
                }
            }

			Group.AddMember(Session.GetHabbo().Id);
		}
    }
}
