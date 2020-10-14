using System;
using System.Text;

using Galaxy.HabboHotel.Games;
using Galaxy.Communication.Packets.Outgoing.GameCenter;

namespace Galaxy.Communication.Packets.Incoming.GameCenter
{
    class JoinPlayerQueueEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendNotification("Os games ainda não estão disponiveis no "+GalaxyServer.HotelName+"!");
            return;

            if (Session == null || Session.GetHabbo() == null)
                return;

            int GameId = Packet.PopInt();

            GameData GameData = null;
            if (GalaxyServer.GetGame().GetGameDataManager().TryGetGame(GameId, out GameData))
            {
                string SSOTicket = Session.GetHabbo().ssoTicket;

                Session.SendMessage(new JoinQueueComposer(GameData.GameId));
                Session.SendMessage(new LoadGameComposer(Session, GameData, SSOTicket));
            }
        }

        private string GenerateSSO(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
}
