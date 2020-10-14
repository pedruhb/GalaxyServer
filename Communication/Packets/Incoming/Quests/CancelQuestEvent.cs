namespace Galaxy.Communication.Packets.Incoming.Quests
{
	class CancelQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            GalaxyServer.GetGame().GetQuestManager().CancelQuest(Session, Packet);
        }
    }
}
