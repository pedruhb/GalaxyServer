using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using System.Collections.Generic;
using System.Linq;
namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MassSayCommand : IChatCommand
    {
        public string PermissionRequired => "command_allsay";
        public string Parameters => "[MENSAGEM]";
        public string Description => "Faz todos os usuários falarem algo.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
            if (Users.Count > 0)
            {
                string Message = CommandManager.MergeParams(Params, 1);
                
                foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
                {
                    if (RoomUser == null || RoomUser.GetClient() == null || Session.GetHabbo().Id == RoomUser.UserId)
                        continue;
                    string Username;
                    string NomeReal = RoomUser.GetUsername();
                    if (RoomUser.GetClient().GetHabbo()._NamePrefix == "off" || RoomUser.GetClient().GetHabbo()._NamePrefix == "")
                    {
                        Username = RoomUser.GetClient().GetHabbo().Username;

                        if (RoomUser.GetClient().GetHabbo().chatHTMLColour != "rainbow")
                            Username = "<font color='" + RoomUser.GetClient().GetHabbo().chatHTMLColour + "'>" + RoomUser.GetClient().GetHabbo().Username + "</font>";

                        if (RoomUser.GetClient().GetHabbo().chatHTMLColour == "rainbow")
                            Username = "<font color='" + GalaxyServer.RainbowT() + "'>" + RoomUser.GetClient().GetHabbo().Username + "</font>";

                        if (RoomUser.GetClient().GetHabbo().chatHTMLColour == "")
                            Username = RoomUser.GetClient().GetHabbo().Username;
                    }
                    else
                    {
                        Username = RoomUser.GetClient().GetHabbo().Username;
                        if (RoomUser.GetClient().GetHabbo()._NamePrefixColor == "rainbow" && RoomUser.GetClient().GetHabbo().chatHTMLColour != "rainbow")
                        {
                            Username = "<font color='" + GalaxyServer.RainbowT() + "'>[" + RoomUser.GetClient().GetHabbo()._NamePrefix + "]</font> <font color='" + RoomUser.GetClient().GetHabbo().chatHTMLColour + "'>" + RoomUser.GetClient().GetHabbo().Username + "</font>";
                        }
                        else if (RoomUser.GetClient().GetHabbo()._NamePrefixColor == "rainbow" && RoomUser.GetClient().GetHabbo().chatHTMLColour == "rainbow")
                        {
                            Username = "<font color='" + GalaxyServer.RainbowT() + "'>[" + RoomUser.GetClient().GetHabbo()._NamePrefix + "]</font> <font color='" + GalaxyServer.Rainbow() + "'>" + RoomUser.GetClient().GetHabbo().Username + "</font>";
                        }
                        else if (RoomUser.GetClient().GetHabbo()._NamePrefixColor != "rainbow" && RoomUser.GetClient().GetHabbo().chatHTMLColour == "rainbow")
                        {
                            Username = "<font color='" + RoomUser.GetClient().GetHabbo()._NamePrefixColor + "'>[" + RoomUser.GetClient().GetHabbo()._NamePrefix + "]</font> <font color='" + GalaxyServer.Rainbow() + "'>" + RoomUser.GetClient().GetHabbo().Username + "</font>";
                        }
                        else
                        {
                            Username = "<font color='" + RoomUser.GetClient().GetHabbo()._NamePrefixColor + "'>[" + RoomUser.GetClient().GetHabbo()._NamePrefix + "]</font> <font color='" + RoomUser.GetClient().GetHabbo().chatHTMLColour + "'>" + RoomUser.GetClient().GetHabbo().Username + "</font>";
                        }
                    }
                    Room.SendMessage(new UserNameChangeComposer(Room.Id, RoomUser.VirtualId, Username));
                    Room.SendMessage(new ChatComposer(RoomUser.VirtualId, Message, 0, RoomUser.LastBubble));
                    Room.SendMessage(new UserNameChangeComposer(Room.Id, RoomUser.VirtualId, NomeReal));
                }
                }
            }
        }
    }

