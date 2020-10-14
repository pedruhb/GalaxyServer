﻿using System;
using System.Linq;
using Galaxy.HabboHotel.Groups;
using Galaxy.HabboHotel.Items;
using Galaxy.Communication.Packets.Outgoing.Groups;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.Database.Interfaces;

namespace Galaxy.Communication.Packets.Incoming.Groups
{
    class UpdateGroupColoursEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int Colour1 = Packet.PopInt();
            int Colour2 = Packet.PopInt();

            Group Group = null;
            if (!GalaxyServer.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;
          
            if (Group.CreatorId != Session.GetHabbo().Id)
                return;

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `groups` SET `colour1` = @colour1, `colour2` = @colour2 WHERE `id` = @groupId LIMIT 1");
                dbClient.AddParameter("colour1", Colour1);
                dbClient.AddParameter("colour2", Colour2);
                dbClient.AddParameter("groupId", Group.Id);
                dbClient.RunQuery();
            }

            Group.Colour1 = Colour1;
            Group.Colour2 = Colour2;

            Session.SendMessage(new GroupInfoComposer(Group, Session));
            if (Session.GetHabbo().CurrentRoom != null)
            {
                foreach (Item Item in Session.GetHabbo().CurrentRoom.GetRoomItemHandler().GetFloor.ToList())
                {
                    if (Item == null || Item.GetBaseItem() == null)
                        continue;

                    if (Item.GetBaseItem().InteractionType != InteractionType.GUILD_ITEM && Item.GetBaseItem().InteractionType != InteractionType.GUILD_GATE || Item.GetBaseItem().InteractionType != InteractionType.GUILD_FORUM)
                        continue;

                    Session.GetHabbo().CurrentRoom.SendMessage(new ObjectUpdateComposer(Item, Convert.ToInt32(Item.UserID)));
                }
            }
        }
    }
}
