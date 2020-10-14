using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Nux
{
    class GetNuxPresentEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            Session.SendNotification("Vá ti tomar no cú rapá, tu n vai bugar dima aqui não seu cuzão do caralho kkkkkkkk");

            GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_AnimationRanking", 1);
        }
    }
}