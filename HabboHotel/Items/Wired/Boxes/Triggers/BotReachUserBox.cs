using Galaxy.Communication.Packets.Incoming;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Triggers
{
    class BotReachUserBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.TriggerBotReachUser; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public BotReachUserBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
            this.StringData = "";

            if (this.SetItems.Count > 0)
                this.SetItems.Clear();
        }

        public void HandleSave(ClientPacket Packet)
        {
            if (this.SetItems.Count > 0)
                this.SetItems.Clear();

            int Unknown = Packet.PopInt();
            string BotName = Packet.PopString();
            this.StringData = BotName;
        }

        public bool Execute(params object[] Params)
        {
            if (string.IsNullOrEmpty(StringData))
                return false;

            RoomUser Bot = (RoomUser)Params[0];
            if (Bot == null || Bot.GetClient() == null || !Bot.IsBot || Bot.BotData == null)
                return false;

            if (Bot.BotData.Name.ToLower() != StringData.ToLower())
                return false;

            RoomUser User = (RoomUser)Params[1];
            if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null || User.IsBot || User.IsPet
                || User.GetClient().GetHabbo().CurrentRoom == null)
                return false;

            Habbo Habbo = User.GetClient().GetHabbo();
            if (Habbo == null)
                return false;

            ICollection<IWiredItem> Effects = Instance.GetWired().GetEffects(this);
            ICollection<IWiredItem> Conditions = Instance.GetWired().GetConditions(this);
            if (Conditions.Any())
            {
                foreach (IWiredItem Condition in Conditions.ToList())
                {
                    if (!Condition.Execute(Habbo))
                        return false;

                    if (Instance != null)
                        Instance.GetWired().OnEvent(Condition.Item);
                }
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
                if (Effects.Any())
                {
                    foreach (IWiredItem Effect in Effects.ToList())
                    {
                        if (!Effect.Execute(Habbo))
                            return false;

                        if (Instance != null)
                            Instance.GetWired().OnEvent(Effect.Item);
                    }
                }
            }

            return true;
        }
    }
}