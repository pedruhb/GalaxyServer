using Galaxy.HabboHotel.GameClients;
using log4net;

namespace Galaxy.Communication.Packets.Incoming.Handshake
{
    public class GetClientVersionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Build = Packet.PopString();

            if (GalaxyServer.SWFRevision != Build)
            {
                      GalaxyServer.SWFRevision = Build;
                      ILog log = LogManager.GetLogger("Galaxy.GalaxyServer"); 
                      log.Info("» Versão detectada: " + Build);
            }
        }
    }
}