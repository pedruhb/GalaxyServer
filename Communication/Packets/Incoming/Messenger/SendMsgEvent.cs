
using Galaxy.Core;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using System.Data;
using System;

namespace Galaxy.Communication.Packets.Incoming.Messenger
{
    class SendMsgEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            int userId = Packet.PopInt();
            if (userId == 0 || userId == Session.GetHabbo().Id)
                return;

            string message = Packet.PopString();
            if (string.IsNullOrWhiteSpace(message)) return;
            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendWhisper("Opa, você foi silenciado por 15 segundos e não pode enviar mensagens durante este período.");
                return;
            }

            string word;
            if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override") &&
                GalaxyServer.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(message, out word))
            {
                Session.GetHabbo().BannedPhraseCount++;
                if (Session.GetHabbo().BannedPhraseCount >= 1)
                {

					Session.GetHabbo().TimeMuted = 25;
                    Session.SendNotification("Você foi silenciado por divulgar um Hotel! " + Session.GetHabbo().BannedPhraseCount + "/3");
                    GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("Alerta de divulgador:",
                        "Atenção, um usuário mencionou a palavra '<b>" + word.ToUpper() + "</b>'<br><br><b>Frase:</b><br><i>" + message +
                        "</i>.<br><br><b>Tipo</b><br>Spam por divulgação no chat.\r\n" + "- Usuário: <b>" +
                        Session.GetHabbo().Username + "</b>", NotificationSettings.NOTIFICATION_FILTER_IMG, "", ""));
                }
                if (Session.GetHabbo().BannedPhraseCount >= 5)
                {
                    GalaxyServer.GetGame().GetModerationManager().BanUser("Sistema", HabboHotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Banido por fazer spam com frases (" + message + ")", (GalaxyServer.GetUnixTimestamp() + 78892200));
                    Session.Disconnect();
                    return;
                }
                return;
            }

            if (Session.GetHabbo().Rank > 0)
            {
                DataRow preso = null;
                using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT Presidio FROM users WHERE id = '" + Session.GetHabbo().Id + "'");
                    preso = dbClient.getRow();
                }

                if (Convert.ToBoolean(preso["Presidio"]) == true)
                {
                        string presovisual = Session.GetHabbo().Look;
                        Session.SendMessage(new RoomNotificationComposer("police_announcement", "message", "Você esta preso e não pode enviar mensagens."));
                        return;
                }
            }
  
            Session.GetHabbo().GetMessenger().SendInstantMessage(userId, message);

        }
    }
}