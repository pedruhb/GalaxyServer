using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Sound
{
    class RemoveDiscFromPlayListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var room = Session.GetHabbo().CurrentRoom;
            if (!room.CheckRights(Session))
                return;
            var itemindex = Packet.PopInt();

            var trax = room.GetTraxManager();
            if (trax.Playlist.Count < itemindex)
            {
                goto error;
            }

            var item = trax.Playlist[itemindex];
            if (!trax.RemoveDisc(item))
                goto error2;

            return;
            error:
            Session.SendMessage(new RoomNotificationComposer("jukeboxporn", "message", "Erro! tente novamente"));
            error2:
            Session.SendMessage(new RoomNotificationComposer("jukeboxporn", "message", "Desligue a jukebox para remover o CD!"));
        }
    }
}
