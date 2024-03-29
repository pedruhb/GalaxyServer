﻿using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Conditions
{
    class DonoNaoEstaNoQuarto : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.ConditionDonoNaoEstaNoQuarto; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public DonoNaoEstaNoQuarto(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Unknown2 = Packet.PopString();

            this.StringData = Unknown2;
        }

        public bool Execute(params object[] Params)
        {

            Habbo Player = (Habbo)Params[0];
            if (Player == null)
                return false;

            RoomUser DonoQuarto = Instance.GetRoomUserManager().GetRoomUserByHabbo(Player.GetClient().GetHabbo().CurrentRoom.OwnerId);

            if (Player.GetClient().GetHabbo().CurrentRoomId == DonoQuarto.RoomId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
