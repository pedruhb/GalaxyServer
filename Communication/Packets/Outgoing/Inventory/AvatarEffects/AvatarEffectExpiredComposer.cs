
using Galaxy.HabboHotel.Users.Effects;

namespace Galaxy.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
	class AvatarEffectExpiredComposer : ServerPacket
    {
        public AvatarEffectExpiredComposer(AvatarEffect Effect)
            : base(ServerPacketHeader.AvatarEffectExpiredMessageComposer)
        {
			WriteInteger(Effect.SpriteId);
        }
    }
}
