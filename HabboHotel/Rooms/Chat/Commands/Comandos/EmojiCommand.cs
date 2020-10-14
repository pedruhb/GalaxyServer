using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class EmojiCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_emoji"; }
        }
        public string Parameters
        {
            get { return ""; }
        }
        public string Description
        {
            get { return "Mande um emoji no chat do hotel! 1 a 189"; }
        }
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Opa, digite um número de 1-189! Para ver a lista de emojis digite :emoji lista");
                Session.SendWhisper("Ou use :emoji user username para aparecer a carinha do usuário.");
                return;
            }
            string emoji = Params[1];
            if (emoji.ToLower() == "user")
            {
                GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[2]);
                if (TargetClient == null)
                {
                    Session.SendWhisper("Essa pessoa não se encontra no quarto ou não está online.");
                    return;
                }
                string Username;
                Username = "<img src='http://habbo.city/habbo-imaging/avatarimage?figure="+TargetClient.GetHabbo().Look+ "&headonly=1'>            <br><br><br><br><br>               ";
                string Message = "         ";
                RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
                Room.SendMessage(new UserNameChangeComposer(Session.GetHabbo().CurrentRoomId, TargetUser.VirtualId, Username));
                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, Message, 0, TargetUser.LastBubble));
                TargetUser.SendNamePacket();
                return;
            }
            if (emoji.Equals("lista"))
            {
                ServerPacket notif = new ServerPacket(ServerPacketHeader.NuxAlertMessageComposer);
                notif.WriteString("habbopages/chat/emoji.txt");
                Session.SendMessage(notif);
            }
            else
            {
                int emojiNum;
                bool isNumeric = int.TryParse(emoji, out emojiNum);
                if (isNumeric)
                {
                    switch (emojiNum)
                    {
                        default:
                            bool isValid = true;
                            if (emojiNum < 1)
                            {
                                isValid = false;
                            }

                            if (emojiNum > 189 && Session.GetHabbo().Rank < 6)
                            {
                                isValid = false;
                            }

							string LinkEmoji = "https://swf.habbografx.me/c_images/emoji/emojis/";

							if (GalaxyServer.Tipo == 1)
								LinkEmoji = "https://swf.galaxyservers.com.br/c_images/emoji/emojis/";

							if (isValid)
                            {
                                string Username;
                                RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
                                if (emojiNum < 10)
                                {
                                    Username = "<img src='"+ LinkEmoji + "Emoji Smiley-0" + emojiNum + ".png' height='24' width='24'><br><br>         ";
                                }
                                else
                                {
                                    Username = "<img src='" + LinkEmoji + "Emoji Smiley-" + emojiNum + ".png' height='24' width='24'><br><br>         ";
                                }
                                if (Room != null)
                                    Room.SendMessage(new UserNameChangeComposer(Session.GetHabbo().CurrentRoomId, TargetUser.VirtualId, Username));

                                string Message = " ";
                                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, Message, 0, TargetUser.LastBubble));
                                TargetUser.SendNamePacket();

                            }
                            else
                            {
                                Session.SendWhisper("Emoji inválido, para ver a lista de emojis digite :emoji lista");
                                Session.SendWhisper("Ou use :emoji user username para aparecer a carinha do usuário.");
                            }

                            break;
                    }
                }
                else
                {
                    Session.SendWhisper("Emoji inválido, deve ser um número entre 1-189. Para ver a lista de Emojis digite ':emoji lista'");
                    Session.SendWhisper("Ou use :emoji user username para aparecer a carinha do usuário.");
                }
            }
        }
    }
}
