using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Users;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Pathfinding;


namespace Galaxy.HabboHotel.Items.Wired.Boxes.Conditions

{
    class NotMoreThanTimer : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.ConditionLessThanTimer; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public int Delay { get { return this._delay; } set { this._delay = value; this.TickCount = value; } }
        public int TickCount { get; set; }
        public string ItemsData { get; set; }
        private int timeout;

        private int _delay = 0;


        public NotMoreThanTimer(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();

        }

        public int Time
        {
            get
            {
                return timeout;
            }
        }


        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            int Delay = Packet.PopInt();

            this.Delay = Delay;
            this.TickCount = Delay;
        }


        public bool Execute(params object[] Params)
        {
            if (Params.Length == 0)
                return false;

            Habbo Player = (Habbo)Params[0];
            if (Player == null)
                return false;

            if (Instance == null || Instance.lastTimerReset == null)
                return false;

            return !((DateTime.Now - Instance.lastTimerReset).TotalSeconds > (timeout / 2));
        }

    }
}