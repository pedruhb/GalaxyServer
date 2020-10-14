﻿using System;
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

namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class BotCopiaVisual : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectBotCopiaVisual; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public BotCopiaVisual(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            int FollowMode = Packet.PopInt();
            string BotConfiguration = Packet.PopString();

            if (this.SetItems.Count > 0)
                this.SetItems.Clear();

            this.StringData = FollowMode + ";" + BotConfiguration;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            if (String.IsNullOrEmpty(this.StringData))
                return false;
            Habbo Player = (Habbo)Params[0];
            if (Player == null)
                return false;
            RoomUser Human = Instance.GetRoomUserManager().GetRoomUserByHabbo(Player.Id);
            if (Human == null)
                return false;

            string[] Stuff = this.StringData.Split(';');
            if (Stuff.Length != 2)
                return false;

            string Username = Stuff[1];

            RoomUser User = this.Instance.GetRoomUserManager().GetBotByName(Username);
            if (User == null)
                return false;

            ServerPacket UserChangeComposer = new ServerPacket(ServerPacketHeader.UserChangeMessageComposer);
            UserChangeComposer.WriteInteger(User.VirtualId);
            UserChangeComposer.WriteString(Player.Look);
            UserChangeComposer.WriteString(Player.Gender);
            UserChangeComposer.WriteString(User.BotData.Motto);
            UserChangeComposer.WriteInteger(0);
            this.Instance.SendMessage(UserChangeComposer);

            User.BotData.Look = Player.Look;
            User.BotData.Gender = Player.Gender;

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `bots` SET `look` = @look, `gender` = '" + User.BotData.Gender + "' WHERE `id` = '" + User.BotData.Id + "' LIMIT 1");
                dbClient.AddParameter("look", User.BotData.Look);
                dbClient.RunQuery();
            }

            return true;


        }
    }
}
