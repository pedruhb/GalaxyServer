﻿using Galaxy.Communication.Packets.Incoming;
using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users;
using System;
using System.Collections.Concurrent;
namespace Galaxy.HabboHotel.Items.Wired.Boxes.Effects
{
    class GiveUserCreditsBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectGiveUserCreditsBox; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public GiveUserCreditsBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket Packet)
        {
            int Unknown = Packet.PopInt();
            string Credit = Packet.PopString();

            this.StringData = Credit;
        }

        public bool Execute(params object[] Params)
        {
            if (Params == null || Params.Length == 0)
                return false;

            Habbo Player = (Habbo)Params[0];
            if (Player == null || Player.GetClient() == null)
                return false;

            RoomUser User = Player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Player.Username);
            if (User == null)
                return false;

            if (String.IsNullOrEmpty(StringData))
                return false;

			int Amount;
            Amount = Convert.ToInt32(StringData);
            if (Amount > 500)
            {
                Player.GetClient().SendWhisper("A quantidade de " + ExtraSettings.NomeMoedas + " passa do limite (500)");
                return false;
            }
            else
            {
            string Credit = StringData;

            Player.Credits = Player.Credits += Convert.ToInt32(Credit);
            Player.GetClient().SendMessage(new CreditBalanceComposer(Player.Credits));
            Player.GetClient().SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Você acaba de ganhar " + Credit + " " + ExtraSettings.NomeMoedas + "(s)!"));

                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("INSERT INTO `log_wiredstaff` (`timestamp`, `quantidade`, `user`, `tipo`) VALUES ('" + GalaxyServer.GetUnixTimestamp() + "', @message, '" + Player.GetClient().GetHabbo().Username + "', '" + ExtraSettings.NomeMoedas + "')");
                    dbClient.AddParameter("message", Amount.ToString());
                    dbClient.RunQuery();
                }

                return true;
			}
        }
    }
}