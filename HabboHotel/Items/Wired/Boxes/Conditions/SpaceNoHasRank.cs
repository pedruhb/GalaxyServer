﻿using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Conditions
{
    class SpaceNoHasRank : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.ConditionSpaceNoHasRank; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public SpaceNoHasRank(Room Instance, Item Item)
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
            if (Params.Length == 0)
                return false;

            if (String.IsNullOrEmpty(this.StringData))
                return false;

            Habbo Player = (Habbo)Params[0];
            if (Player == null)
                return false;

            if (int.Parse(this.StringData) > 17 | int.Parse(this.StringData) < 1)
            {
                Player.GetClient().SendWhisper("Você deve colocar um rank entre 1 e 17.");
                return false;
            } else
            {
                if (Player.GetClient().GetHabbo().Rank != int.Parse(this.StringData))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
