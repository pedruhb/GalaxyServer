using Galaxy.Communication.Packets.Outgoing.Rooms.Furni.Wired;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Items.Wired;
using Galaxy.HabboHotel.Rooms;

namespace Galaxy.Communication.Packets.Incoming.Rooms.Furni.Wired
{
    class SaveWiredConfigEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null || !Room.CheckRights(Session, false, true))
                return;

            int ItemId = Packet.PopInt();

            Session.SendMessage(new HideWiredConfigComposer());

            Item SelectedItem = Room.GetRoomItemHandler().GetItem(ItemId);
            if (SelectedItem == null)
                return;

			if (!Session.GetHabbo().CurrentRoom.GetWired().TryGet(ItemId, out IWiredItem Box))
				return;

            //// Proteção Wireds de equipe
            if (Box.Type == WiredBoxType.EffectGiveUserBadge && !Session.GetHabbo().GetPermissions().HasRight("room_item_wired_rewards"))
            { /// Wired de dar emblema
                Session.SendNotification("Você não tem permissões para fazer isso!");
                return;
            }
            if (Box.Type == WiredBoxType.EffectGiveFurni && !Session.GetHabbo().GetPermissions().HasRight("room_item_wired_rewards"))
            { /// Wired de dar mobis
                Session.SendNotification("Você não tem permissões para fazer isso!");
                return;
            }
            if (Box.Type == WiredBoxType.EffectGiveReward && !Session.GetHabbo().GetPermissions().HasRight("room_item_wired_rewards"))
            { /// Super wired
                Session.SendNotification("Você não tem permissões para fazer isso!");
                return;
            }
            if (Box.Type == WiredBoxType.EffectGiveUserCreditsBox && !Session.GetHabbo().GetPermissions().HasRight("room_item_wired_rewards"))
            { /// Wired de moedas
                Session.SendNotification("Você não tem permissões para fazer isso!");
                return;
            }
            if (Box.Type == WiredBoxType.EffectGiveUserDucketsBox && !Session.GetHabbo().GetPermissions().HasRight("room_item_wired_rewards"))
            { /// Wired de duckets
                Session.SendNotification("Você não tem permissões para fazer isso!");
                return;
            }
            if (Box.Type == WiredBoxType.EffectGiveUserDiamondsBox && !Session.GetHabbo().GetPermissions().HasRight("room_item_wired_rewards"))
            { /// Wired de diamantes
                Session.SendNotification("Você não tem permissões para fazer isso!");
                return;
            }
            ////

            Box.HandleSave(Packet);
            Session.GetHabbo().CurrentRoom.GetWired().SaveBox(Box);
        }
    }
}
