using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Data;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ChangelogCommand : IChatCommand
    {
        public string PermissionRequired => "";
        public string Parameters => "";
        public string Description => "Mostra as últimas atualizações do hotel.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            ServerPacket notif = new ServerPacket(ServerPacketHeader.NuxAlertMessageComposer);
            notif.WriteString("habbopages/changelog.php");
            Session.GetHabbo().GetClient().SendMessage(notif);
        }
    }
}