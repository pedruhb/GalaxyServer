using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using System.Collections.Concurrent;
using System.Linq;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class TimerReset : IWiredItem, IWiredCycle
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectTimerReset; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public int TickCount { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public int Delay { get { return this._delay; } set { this._delay = value; this.TickCount = value; } }
        public string ItemsData { get; set; }

        private long _next;
        private int _delay = 0;
        private bool Requested = false;

        public TimerReset(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            this.SetItems.Clear();
            int Unknown = Packet.PopInt();
            string Unknown2 = Packet.PopString();

            int FurniCount = Packet.PopInt();
            for (int i = 0; i < FurniCount; i++)
            {
                Item SelectedItem = Instance.GetRoomItemHandler().GetItem(Packet.PopInt());
                if (SelectedItem != null)
                    SetItems.TryAdd(SelectedItem.Id, SelectedItem);
            }

            int Delay = Packet.PopInt();
            this.Delay = Delay;
        }

        public bool Execute(params object[] Params)
        {
            if (this._next == 0 || this._next < GalaxyServer.Now())
                this._next = GalaxyServer.Now() + this.Delay;


            this.Requested = true;
            this.TickCount = Delay;
            return true;
        }

        public bool OnCycle()
        {
            if (this.SetItems.Count == 0 || !Requested)
                return false;

            long Now = GalaxyServer.Now();
            if (_next < Now)
            {
                foreach (Item Item in this.SetItems.Values.ToList())
                {
                    if (Item == null)
                        continue;

                    if (!Instance.GetRoomItemHandler().GetFloor.Contains(Item))
                    {
                        SetItems.TryRemove(Item.Id, out Item n);
                        continue;
                    }

                    Item.Interactor.OnWiredTrigger(Item);
                }

                Requested = false;

                this._next = 0;
                this.TickCount = Delay;

            }
            return true;
        }
    }
}
