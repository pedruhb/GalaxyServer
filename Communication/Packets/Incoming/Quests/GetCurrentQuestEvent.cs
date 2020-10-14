namespace Galaxy.Communication.Packets.Incoming.Quests
{
	class GetCurrentQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            GalaxyServer.GetGame().GetQuestManager().GetCurrentQuest(Session, Packet);
        }
    }
}
