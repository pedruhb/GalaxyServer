using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class BotGivesHandItemBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectBotGivesHanditemBox; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public BotGivesHandItemBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            int ChatMode = Packet.PopInt();
            string ChatConfig = Packet.PopString();

            this.StringData = ChatConfig;
            if (ChatMode == 1)
            {
                this.BoolData = true;
            }
            else
            {
                this.BoolData = false;
            }

        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            if (String.IsNullOrEmpty(this.StringData))
                return false;

            this.StringData.Split(' ');
            string BotName = this.StringData.Split('	')[0];
            string Chat = this.StringData.Split('	')[1];

            string Message = StringData.Split('	')[1];
            string MessageFiltered = Message;

            RoomUser User = this.Instance.GetRoomUserManager().GetBotByName(BotName);
            if (User == null)
                return false;

            Habbo Player = (Habbo)Params[0];

            RoomUser Actor = this.Instance.GetRoomUserManager().GetRoomUserByHabbo(Player.Id);

            if (Actor == null)
                return false;

            if (User.BotData.TargetUser == 0)
            {
                if (!Instance.GetGameMap().CanWalk(Actor.SquareBehind.X, Actor.SquareBehind.Y, false))
                {
                    Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, "Não consigo alcançá-lo, você deve se aproximar de mim!", 0, 31));
                }
                else
                {
                    

                    int DrinkId = int.Parse(Message);

                    if (Convert.ToInt32(DrinkId) < 1)
                    {
                        Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, "Você deve colocar o id de um item de mão!", 0, 31));
                        return false;
                    }

                    User.CarryItem(DrinkId);
                    User.BotData.TargetUser = Actor.HabboId;

                    if (this.BoolData)
                    {
                        Player.GetClient().SendMessage(new WhisperComposer(User.VirtualId, "Aqui está, " + Player.GetClient().GetHabbo().Username + "!", 0, 31));
                    }
                    else
                    {
                        User.Chat("Aqui está, " + Player.GetClient().GetHabbo().Username + "!", false, User.LastBubble);
                    }

                    User.MoveTo(Actor.SquareBehind.X, Actor.SquareBehind.Y);
                }
            }
            return true;
        }
    }
}

