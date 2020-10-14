using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Triggers
{
    class UserWalksOnBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.TriggerWalkOnFurni; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public UserWalksOnBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.StringData = "";
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Unknown2 = Packet.PopString();

            if (this.SetItems.Count > 0)
                this.SetItems.Clear();

            int FurniCount = Packet.PopInt();
            for (int i = 0; i < FurniCount; i++)
            {
                Item SelectedItem = Instance.GetRoomItemHandler().GetItem(Packet.PopInt());
                if (SelectedItem != null)
                    SetItems.TryAdd(SelectedItem.Id, SelectedItem);
            }
        }

        public bool Execute(params object[] Params)
        {
            Habbo Player = (Habbo)Params[0];
            if (Player == null)
                return false;

            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;

            Item Item = (Item)Params[1];
            if (Item == null)
                return false;

            //if (User.Z != Item.TotalHeight + Item.GetZ && !Item.Data.IsSeat && Item.Data.InteractionType != InteractionType.BED)
            //    return false;

            if (!User.isRolling && User.Z != Item.TotalHeight && !Item.Data.IsSeat)
                return false;


            if (!this.SetItems.ContainsKey(Item.Id))
                return false;

            ICollection<IWiredItem> Effects = Instance.GetWired().GetEffects(this);
            ICollection<IWiredItem> Conditions = Instance.GetWired().GetConditions(this);

            foreach (IWiredItem Condition in Conditions.ToList())
            {
                if (!Condition.Execute(Player))
                    return false;

                if (Instance != null)
                    Instance.GetWired().OnEvent(Condition.Item);
            }

            bool HasRandomEffectAddon = Effects.Where(x => x.Type == WiredBoxType.AddonRandomEffect).ToList().Count() > 0;
            if (HasRandomEffectAddon)
            {
                IWiredItem RandomBox = Effects.FirstOrDefault(x => x.Type == WiredBoxType.AddonRandomEffect);
                if (!RandomBox.Execute())
                    return false;

                IWiredItem SelectedBox = Instance.GetWired().GetRandomEffect(Effects.ToList());
                if (!SelectedBox.Execute())
                    return false;

                if (Instance != null)
                {
                    Instance.GetWired().OnEvent(RandomBox.Item);
                    Instance.GetWired().OnEvent(SelectedBox.Item);
                }
            }
            else
            {
                foreach (IWiredItem Effect in Effects.ToList())
                {
                    if (!Effect.Execute(Player))
                        return false;

                    if (Instance != null)
                        Instance.GetWired().OnEvent(Effect.Item);
                }
            }

            return true;
        }
    }
}
