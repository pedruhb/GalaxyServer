using Galaxy.HabboHotel.GameClients;

namespace Galaxy.Communication.Packets.Incoming.Handshake
{
    public class SSOTicketEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

			if (GalaxyServer.Licensed == false)
			{
				System.Console.WriteLine("Licença inválida!");
				return;
			}

            if (Session == null || Session.RC4Client == null || Session.GetHabbo() != null)
                return;

            string SSO = Packet.PopString();
            if (string.IsNullOrEmpty(SSO) || SSO.Length < 15)
                return;

            Session.TryAuthenticate(SSO);
        }
    }
}