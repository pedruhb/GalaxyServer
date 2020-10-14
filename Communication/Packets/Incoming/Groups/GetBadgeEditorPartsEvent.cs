using Galaxy.Communication.Packets.Outgoing.Groups;

namespace Galaxy.Communication.Packets.Incoming.Groups
{
    class GetBadgeEditorPartsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BadgeEditorPartsComposer(
                GalaxyServer.GetGame().GetGroupManager().BadgeBases,
                GalaxyServer.GetGame().GetGroupManager().BadgeSymbols,
                GalaxyServer.GetGame().GetGroupManager().BadgeBaseColours,
                GalaxyServer.GetGame().GetGroupManager().BadgeSymbolColours,
                GalaxyServer.GetGame().GetGroupManager().BadgeBackColours));
        }
    }
}
