using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Communication.Packets.Outgoing.Rooms.Avatar;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Rooms.AI;
using Galaxy.HabboHotel.Rooms.AI.Speech;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Galaxy.Communication.Packets.Incoming.Rooms.AI.Bots
{
    class SaveBotActionEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;
            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;
            int BotId = Packet.PopInt();
            int ActionId = Packet.PopInt();
            string DataString = Packet.PopString();
            if (ActionId < 1 || ActionId > 5)
                return;
            RoomUser Bot = null;
            if (!Room.GetRoomUserManager().TryGetBot(BotId, out Bot))
                return;
            if ((Bot.BotData.ownerID != Session.GetHabbo().Id && !Session.GetHabbo().GetPermissions().HasRight("bot_edit_any_override")))
                return;
            RoomBot RoomBot = Bot.BotData;
            if (RoomBot == null)
                return;
            /* 1 = Copy looks
             * 2 = Setup Speech
             * 3 = Relax
             * 4 = Dance
             * 5 = Change Name
             */
            switch (ActionId)
            {
                #region Copy Looks (1)
                case 1:
                    {
                        ServerPacket UserChangeComposer = new ServerPacket(ServerPacketHeader.UserChangeMessageComposer);
                        UserChangeComposer.WriteInteger(Bot.VirtualId);
                        UserChangeComposer.WriteString(Session.GetHabbo().Look);
                        UserChangeComposer.WriteString(Session.GetHabbo().Gender);
                        UserChangeComposer.WriteString(Bot.BotData.Motto);
                        UserChangeComposer.WriteInteger(0);
                        Room.SendMessage(UserChangeComposer);

                        Bot.BotData.Look = Session.GetHabbo().Look;
                        Bot.BotData.Gender = Session.GetHabbo().Gender;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `bots` SET `look` = @look, `gender` = '" + Session.GetHabbo().Gender + "' WHERE `id` = '" + Bot.BotData.Id + "' LIMIT 1");
                            dbClient.AddParameter("look", Session.GetHabbo().Look);
                            dbClient.RunQuery();
                        }
                        //Room.SendMessage(new UserChangeComposer(BotUser.GetClient(), true));
                        break;
                    }

                //Código inteiro fixado Retirar Inteiro Otimized
                #endregion
                #region Setup Speech (2)
                case 2:
                    {
                        string[] ConfigData = DataString.Split(new string[]
                        {
                            ";#;"
                        }, StringSplitOptions.None);
                        string[] SpeechData = ConfigData[0].Split(new char[]
                        {
                            '\r',
                            '\n'
                        }, StringSplitOptions.RemoveEmptyEntries);
                        string AutomaticChat = Convert.ToString(ConfigData[1]);
                        string SpeakingInterval = Convert.ToString(ConfigData[2]);
                        string MixChat = Convert.ToString(ConfigData[3]);
                        if (String.IsNullOrEmpty(SpeakingInterval) || Convert.ToInt32(SpeakingInterval) <= 0 || Convert.ToInt32(SpeakingInterval) < 7)
                            SpeakingInterval = "7";
                        RoomBot.AutomaticChat = Convert.ToBoolean(AutomaticChat);
                        RoomBot.SpeakingInterval = Convert.ToInt32(SpeakingInterval);
                        RoomBot.MixSentences = Convert.ToBoolean(MixChat);
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("DELETE FROM `bots_speech` WHERE `bot_id` = '" + Bot.BotData.Id + "'");
                            #region Save Data - TODO: MAKE METHODS FOR THIS.
                            for (int i = 0; i <= SpeechData.Length - 1; i++)
                            {
                                SpeechData[i] = Regex.Replace(SpeechData[i], "<(.|\\n)*?>", string.Empty);
                                    dbClient.SetQuery("INSERT INTO `bots_speech` (`bot_id`, `text`) VALUES (@id, @data)");
                                dbClient.AddParameter("id", BotId);
                                dbClient.AddParameter("data", SpeechData[i]);
                                dbClient.RunQuery();
                                dbClient.SetQuery("UPDATE `bots` SET `automatic_chat` = @AutomaticChat, `speaking_interval` = @SpeakingInterval, `mix_sentences` = @MixChat WHERE `id` = @id LIMIT 1");
                                dbClient.AddParameter("id", BotId);
                                dbClient.AddParameter("AutomaticChat", AutomaticChat.ToLower());
                                dbClient.AddParameter("SpeakingInterval", Convert.ToInt32(SpeakingInterval));
                                dbClient.AddParameter("MixChat", GalaxyServer.BoolToEnum(Convert.ToBoolean(MixChat)));
                                dbClient.RunQuery();
                            }
                            #endregion
                            #region Handle Speech
                            RoomBot.RandomSpeech.Clear();
                            dbClient.SetQuery("SELECT `text` FROM `bots_speech` WHERE `bot_id` = @id");
                            dbClient.AddParameter("id", BotId);
                            DataTable BotSpeech = dbClient.getTable();
                            List<RandomSpeech> Speeches = new List<RandomSpeech>();
                            foreach (DataRow Speech in BotSpeech.Rows)
                            {
                                RoomBot.RandomSpeech.Add(new RandomSpeech(Convert.ToString(Speech["text"]), BotId));
                            }
                            #endregion
                        }
                        break;
                    }
                #endregion
                #region Relax (3)
                case 3:
                    {
                        if (Bot.BotData.WalkingMode == "stand")
                            Bot.BotData.WalkingMode = "freeroam";
                        else
                            Bot.BotData.WalkingMode = "stand";
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `bots` SET `walk_mode` = '" + Bot.BotData.WalkingMode + "' WHERE `id` = '" + Bot.BotData.Id + "' LIMIT 1");
                        }
                        break;
                    }
                #endregion
                #region Dance (4)
                case 4:
                    {
                        if (Bot.BotData.DanceId > 0)
                            Bot.BotData.DanceId = 0;
                        else
                        {
                            Random RandomDance = new Random();
                            Bot.BotData.DanceId = RandomDance.Next(1, 4);
                        }
                        Room.SendMessage(new DanceComposer(Bot, Bot.BotData.DanceId));
                        break;
                    }
                #endregion
                #region Change Name (5)
                case 5:
                    {
						DataString = Regex.Replace(DataString, @"[A-Za-z0-9áéíóúàèìòùâêîôûãõç]\\s", string.Empty);
						if (DataString.Length == 0)
                        {
                            Session.SendWhisper("Por favor, coloque um nome nele!");
                            return;
                        }
                        else if (DataString.Length >= 16)
                        {
                            Session.SendWhisper("O máximo é de 16 caracteres!");
                            return;
                        }
                        if (DataString.Contains("<img src") || DataString.Contains("<font ") || DataString.Contains("</font>") || DataString.Contains("</a>") || DataString.Contains("<i>"))
                        {
                            Session.SendWhisper("Você não pode colocar código HTML aqui!");
                            return;
                        }
                        Bot.BotData.Name = DataString;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `bots` SET `name` = @name WHERE `id` = '" + Bot.BotData.Id + "' LIMIT 1");
                            dbClient.AddParameter("name", DataString);
                            dbClient.RunQuery();
                        }
                        Room.SendMessage(new UsersComposer(Bot));
                        break;
                    }
                    #endregion
            }
        }
    }
}
