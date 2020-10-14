﻿using Galaxy.Communication.Packets.Outgoing.LandingView;
using System;

namespace Galaxy.Communication.Packets.Incoming.LandingView
{
    class RefreshCampaignEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            try
            {
                String parseCampaings = Packet.PopString();
                if (parseCampaings.Contains("gamesmaker"))
                    return;

                String campaingName = "";
                String[] parser = parseCampaings.Split(';');

                for (int i = 0; i < parser.Length; i++)
                {
                    if (String.IsNullOrEmpty(parser[i]) || parser[i].EndsWith(","))
                        continue;

                    String[] data = parser[i].Split(',');
                    campaingName = data[1];
                }
                Session.SendMessage(new CampaignComposer(parseCampaings, campaingName));
            }
            catch { }
        }
    }
}