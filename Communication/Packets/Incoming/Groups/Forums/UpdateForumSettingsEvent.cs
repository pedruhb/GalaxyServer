﻿using Galaxy.Communication.Packets.Outgoing.Groups;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Groups
{
    class UpdateForumSettingsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var ForumId = Packet.PopInt();
            var WhoCanRead = Packet.PopInt();
            var WhoCanReply = Packet.PopInt();
            var WhoCanPost = Packet.PopInt();
            var WhoCanMod = Packet.PopInt();


            var forum = GalaxyServer.GetGame().GetGroupForumManager().GetForum(ForumId);

            if (forum == null)
            {
              //  Session.SendNotification(LanguageLocale.Value("forums.not.found"));
                return;
            }

            if (forum.Settings.GetReasonForNot(Session, forum.Settings.WhoCanModerate) != "")
            {
               // Session.SendNotification(LanguageLocale.Value("forums.settings.update.error.rights"));
                return;
            }

            forum.Settings.WhoCanRead = WhoCanRead;
            forum.Settings.WhoCanModerate = WhoCanMod;
            forum.Settings.WhoCanPost = WhoCanReply;
            forum.Settings.WhoCanInitDiscussions = WhoCanPost;

            forum.Settings.Save();

            Session.SendMessage(new ForumDataComposer(forum, Session));

            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SelfModForumCanModerateSeen", 1);
            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SelfModForumCanReadSeen", 1);
            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SelfModForumCanPostThrdSeen", 1);
            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SelfModForumCanPostSeen", 1);
            //Session.SendMessage(new ThreadsListDataComposer(forum, Session));

        }
    }


}