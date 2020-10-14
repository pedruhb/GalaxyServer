using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Galaxy.HabboHotel.Rooms.AI;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.GameClients;
using Galaxy.Communication.Packets.Outgoing.Inventory.Pets;
using Galaxy.Communication.Packets.Outgoing.Rooms.Avatar;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.HabboHotel.Users;
using Galaxy.Communication.Packets.Incoming;
using System.Threading;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;

using Galaxy.Communication.Packets.Outgoing;
using Galaxy.HabboHotel.Rooms.AI.Speech;
using Galaxy.HabboHotel.Items.Utilities;
using Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.HabboHotel.Rooms.Chat.Commands;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class ControlBotCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_controlabot"; }
        }

        public string Parameters
        {
            get { return "[AÇÃO]"; }
        }

        public string Description
        {
            get { return "Controle de Bots!"; }
        }


        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendMessage(new RoomNotificationComposer("Controle de Bots",
                "Comandos básicos para criação de bots:\n\n" +
                ":controlabot <b> criar [quantidade]</b> - Cria a quantidade desejada de bots.\n\n" +
                ":controlabot <b> remover</b> - Apaga todos os bots.\n\n" +
                ":controlabot <b> dance [id]</b> - Faz o bot dançar a dança selecionada. [1 - 4]\n\n" +
                ":controlabot <b> copyme</b> - Faz com que o bot copie seu visual.\n\n" +
                ":controlabot <b> falar [fala]</b> - Faz o bot falar.\n\n" +
                ":controlabot <b> [dormir/acordar]</b> - Faz com que o bot durma ou acorde.\n\n" +
                "", "","",""));
                return;
            }
            string action = Convert.ToString(Params[1]);

            if (action == "criar" || action == "create")
            {
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);

                if (Params.Length == 2)
                {
                    Session.SendWhisper("Coloque a quantidade de bots para spawnar!");
                    return;
                }

                int i = Convert.ToInt32(Params[2]);
                if (i > 500)
                {
                    Session.SendWhisper("Você não pode spawnar tantos assim!");
                    return;
                }
                if (i < 1)
                {
                    Session.SendWhisper("Você não pode spawnar menos que 1 bot.");
                    return;
                }
                int value = 1;
                List<RandomSpeech> RndSpeechList = new List<RandomSpeech>();
                Pet Pet = PetUtility.CreatePet(Session.GetHabbo().Id, Session.GetHabbo().Username, 26, "30", "ffffff");
                if (Convert.ToInt32(Params[2]) > 1)
                {
                    Session.SendWhisper("Spawnando " + Params[2] + " bots...");
                }
                while (value <= i)
                {
                    Random random = new Random();
                    RoomUser Bot = Room.GetRoomUserManager().DeployBot(new RoomBot(999999 + value + random.Next(999, 9999999), Session.GetHabbo().CurrentRoomId, "bot", "freeroam", Session.GetHabbo().Username, "", Session.GetHabbo().Look, User.X, User.Y, 0, 0, 0, 0, 0, 0, ref RndSpeechList, "", 0, Session.GetHabbo().Id, false, 0, false, 31), Pet);
                    value++;
                    Thread.Sleep(100);

                }
                if (Convert.ToInt32(Params[2]) > 1)
                {
                    Session.SendWhisper("Você spawnou " + Params[2] + " bots!");
                }
                if (Convert.ToInt32(Params[2]) == 1)
                {
                    Session.SendWhisper("Você spawnou " + Params[2] + " bot!");
                }
                Session.SendWhisper("Não escolha o bot, use o comando :createbot remover!");
            }
            else
            if (action == "remover")
            {

                if (Room.CheckRights(Session, true))
                {
                    var roomId = Session.GetHabbo().CurrentRoomId;
                    List<RoomUser> UsersToReturn = new List<RoomUser>(Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUsers().ToList());

                    RoomData Data = GalaxyServer.GetGame().GetRoomManager().GenerateRoomData(roomId);
                    Session.GetHabbo().PrepareRoom(Session.GetHabbo().CurrentRoom.RoomId, "");
                    GalaxyServer.GetGame().GetRoomManager().LoadRoom(roomId);

                    foreach (RoomUser User in UsersToReturn)
                    {
                        if (User == null || User.GetClient() == null)
                            continue;

                        User.GetClient().SendMessage(new RoomForwardComposer(roomId));
                    }
                }

            } else
            if (action == "dance")
            {
                if (Params[2] == null)
                {
                    Session.SendWhisper("Coloque o ID da dança! (de 1 a 4) (0 para de dançar).");
                    return;
                }
                int DanceID = Convert.ToInt32(Params[2]);
                foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                {
                    if (User == null || User.IsPet || !User.IsBot)
                        continue;

                    RoomUser BotUser = null;
                    if (!Room.GetRoomUserManager().TryGetBot(User.BotData.Id, out BotUser))
                        return;

                    BotUser.DanceId = DanceID;
                    Room.SendMessage(new DanceComposer(BotUser, DanceID));
                }
            }
            else 
            if (action == "say")
            {
                if (Params[2] == null)
                {
                    Session.SendWhisper("Escreva a mensagem para o(s) bot(s) falarem.");
                    return;
                }
                foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                {
                    if (User == null || User.IsPet || !User.IsBot)
                        continue;
                    string Message = CommandManager.MergeParams(Params, 2);
                    RoomUser BotUser = null;
                    if (!Room.GetRoomUserManager().TryGetBot(User.BotData.Id, out BotUser))
                        return;
                    Room.SendMessage(new ChatComposer(BotUser.VirtualId, Message, 0, 31));
                }
            }
            else
            if (action == "dormir")
            {

                foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                {
                    if (User == null || User.IsPet || !User.IsBot)
                        continue;
                    RoomUser BotUser = null;
                    if (!Room.GetRoomUserManager().TryGetBot(User.BotData.Id, out BotUser))
                        return;
                    BotUser.Frozen = true;
                    Room.SendMessage(new SleepComposer(BotUser, true));
                }
            }
            else
            if (action == "acordar")
            {

                foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                {
                    if (User == null || User.IsPet || !User.IsBot)
                        continue;
                    RoomUser BotUser = null;
                    if (!Room.GetRoomUserManager().TryGetBot(User.BotData.Id, out BotUser))
                        return;
                    BotUser.Frozen = false;
                    Room.SendMessage(new SleepComposer(BotUser, false));
                }
            }
            else
            if (action == "help" || action == "ajuda")
            {
                Session.SendMessage(new RoomNotificationComposer("Controle de Bots",
                "Comandos básicos para criação de bots:\n\n" +
                ":controlabot <b> criar [quantidade]</b> - Cria a quantidade desejada de bots.\n\n" +
                ":controlabot <b> remover</b> - Reeentra no quarto e apaga os bots.\n\n" +
                ":controlabot <b> dance [id]</b> - Faz o bot dançar a dança selecionada. [1 - 4]\n\n" +
                ":controlabot <b> copyme</b> - Faz com que o bot copie seu visual.\n\n" +
                ":controlabot <b> falar [fala]</b> - Faz o bot falar.\n\n" +
                ":controlabot <b> [dormir/acordar]</b> - Faz com que o bot durma ou acorde.\n\n" +
                "", "", "", ""));
            }
            else
            if (action == "nick")
            {
                if (Params[2] == null)
                {
                    Session.SendWhisper("Defina o nome do bot!");
                    return;
                }
                foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                {
                    if (User == null || User.IsPet || !User.IsBot)
                        continue;

                    string Nick = CommandManager.MergeParams(Params, 2);
                    RoomUser BotUser = null;
                    if (!Room.GetRoomUserManager().TryGetBot(User.BotData.Id, out BotUser))
                        return;
                    BotUser.BotData.Name = Nick;
                    Room.SendMessage(new UsersComposer(BotUser));
                }


            }

            else
            if (action == "copyme")
            {
                foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
                {
                    if (User == null || User.IsPet || !User.IsBot)
                        continue;

                    string Nick = CommandManager.MergeParams(Params, 2);
                    RoomUser Bot = null;
                    if (!Room.GetRoomUserManager().TryGetBot(User.BotData.Id, out Bot))
                        return;

                    ServerPacket UserChangeComposer = new ServerPacket(ServerPacketHeader.UserChangeMessageComposer);
                    UserChangeComposer.WriteInteger(Bot.VirtualId);
                    UserChangeComposer.WriteString(Session.GetHabbo().Look);
                    UserChangeComposer.WriteString(Session.GetHabbo().Gender);
                    UserChangeComposer.WriteString(Bot.BotData.Motto);
                    UserChangeComposer.WriteInteger(0);
                    Room.SendMessage(UserChangeComposer);

                    //Change the defaults
                    Bot.BotData.Look = Session.GetHabbo().Look;
                    Bot.BotData.Gender = Session.GetHabbo().Gender;
                }
            }else
            {
                Session.SendWhisper("Erro no comando! confira a escrita e variáveis.");
            }


        }
    }
}
