using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Groups;
using Galaxy.Communication.Packets.Outgoing.Groups;
using Galaxy.Communication.Packets.Outgoing.Rooms.Permissions;



namespace Galaxy.Communication.Packets.Incoming.Groups
{
    class GiveAdminRightsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!GalaxyServer.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Session.GetHabbo().Id != Group.CreatorId || !Group.IsMember(UserId))
                return;

            Habbo Habbo = GalaxyServer.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Ocorreu um erro ao encontrar esse usuário!");
                return;
            }

            Group.MakeAdmin(UserId);
          
            Room Room = null;
            if (GalaxyServer.GetGame().GetRoomManager().TryGetRoom(Group.RoomId, out Room))
            {
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
                if (User != null)
                {
                    if (!User.Statusses.ContainsKey("flatctrl 3"))
                        User.SetStatus("flatctrl 3", "");
                    User.UpdateNeeded = true;
                    if (User.GetClient() != null)
                    {
                        GalaxyServer.GetGame().GetGroupManager().Init();
                        User.GetClient().SendMessage(new YouAreControllerComposer(3));
                    }
                       
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 1));
        }
    }
}
