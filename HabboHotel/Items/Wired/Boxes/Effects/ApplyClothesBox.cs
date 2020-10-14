using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Database.Interfaces;
using Galaxy.Communication.Packets.Incoming.Users;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class ApplyClothesBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectApplyClothes; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public ApplyClothesBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string BotConfiguration = Packet.PopString();

            if (this.SetItems.Count > 0)
                this.SetItems.Clear();

            RoomUser User22 = Instance.GetRoomUserManager().GetRoomUserByHabbo(Item.UserID);
            this.StringData = BotConfiguration + ";" + User22.GetClient().GetHabbo().Gender;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            if (String.IsNullOrEmpty(this.StringData))
                return false;


            string[] Stuff = this.StringData.Split('\t');
            if (Stuff.Length != 2)
                return false;
            string Figure = Stuff[1];
            string[] Stuff2 = Figure.Split(';');
            if (Stuff2.Length != 2)
                return false;

            string visual = Stuff2[0];
            string genero = Stuff2[1];

            Habbo Player = (Habbo)Params[0];
            if (Player == null)
                return false;

            RoomUser User = Instance.GetRoomUserManager().GetRoomUserByHabbo(Player.Id);
            if (User == null)
                return false;

            User.GetClient().GetHabbo().Gender = genero.ToUpper();
            User.GetClient().GetHabbo().Look = visual;

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `gender` = @gender, `look` = @look WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("gender", User.GetClient().GetHabbo().Gender);
                dbClient.AddParameter("look", User.GetClient().GetHabbo().Look);
                dbClient.AddParameter("id", User.GetClient().GetHabbo().Id);
                dbClient.RunQuery();
            }

            if (User != null)
            {
                this.Instance.SendMessage(new UserChangeComposer(User, true));
                Player.GetClient().SendMessage(new UserChangeComposer(User, false));
                this.Instance.SendMessage(new AvatarAspectUpdateMessageComposer(User.GetClient().GetHabbo().Look, User.GetClient().GetHabbo().Gender)); //esto
            }

            return true;
        }
    }
}
