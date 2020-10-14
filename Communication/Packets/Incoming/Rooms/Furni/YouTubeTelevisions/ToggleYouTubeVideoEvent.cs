using Galaxy.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;


namespace Galaxy.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions
{
    class ToggleYouTubeVideoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int ItemId = Packet.PopInt();//Item Id
            string VideoId = Packet.PopString(); //Video ID
			return;

     //       Session.SendMessage(new GetYouTubeVideoComposer(ItemId, VideoId));


        }
    }
}