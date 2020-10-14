using Galaxy.Communication.Packets.Outgoing.Users;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Groups;
using Galaxy.HabboHotel.Users;
using System.Collections.Generic;


namespace Galaxy.Communication.Packets.Incoming.Users
{
    class OpenPlayerProfileEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int userID = Packet.PopInt();

            Habbo targetData = GalaxyServer.GetHabboById(userID);
            if (targetData == null)
            {
                Session.SendNotification("Ocorreu um erro ao encontrar o perfil do usuário.");
                return;
            }

            List<Group> groups = GalaxyServer.GetGame().GetGroupManager().GetGroupsForUser(targetData.Id);

            int friendCount = 0;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT COUNT(0) FROM `messenger_friendships` WHERE (`user_one_id` = @userid OR `user_two_id` = @userid)");
                dbClient.AddParameter("userid", userID);
                friendCount = dbClient.getInteger();
            }

            Session.SendMessage(new ProfileInformationComposer(targetData, Session, groups, friendCount));
        }
    }
}
