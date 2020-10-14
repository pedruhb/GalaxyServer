using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.HabboHotel.Items.Wired;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MakeSayCommand : IChatCommand
    {
        public string PermissionRequired => "command_makesay";
        public string Parameters => "[USUARIO] [MENSAGEM]";
        public string Description => "Força outro usuário a dizer algo.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;

            if (Params.Length == 1)
                Session.SendWhisper("Você deve inserir o nome de usuário e a palavra.");
            else
            {
                string Message = CommandManager.MergeParams(Params, 2);
                RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Params[1]);
                if (TargetUser != null)
                {
                    if(TargetUser.GetClient().GetHabbo().Rank > Session.GetHabbo().Rank)
                    {
                        Session.SendWhisper("O rank do usuário é maior que o seu.");
                        return;
                    }

                    if (TargetUser.GetClient() != null && TargetUser.GetClient().GetHabbo() != null)
                        if (!TargetUser.GetClient().GetHabbo().GetPermissions().HasRight("mod_make_say_any") && TargetUser.GetClient().GetHabbo().Username != "PHB")
                        {
                            string Username;
                            string NomeReal = TargetUser.GetUsername();
                            if (TargetUser.GetClient().GetHabbo()._NamePrefix == "off" || TargetUser.GetClient().GetHabbo()._NamePrefix == "")
                            {
                                 Username = TargetUser.GetClient().GetHabbo().Username;

                                if (TargetUser.GetClient().GetHabbo().chatHTMLColour != "rainbow")
                                    Username = "<font color='" + TargetUser.GetClient().GetHabbo().chatHTMLColour + "'>" + TargetUser.GetClient().GetHabbo().Username + "</font>";

                                if (TargetUser.GetClient().GetHabbo().chatHTMLColour == "rainbow")
                                    Username = "<font color='" + GalaxyServer.RainbowT() + "'>" + TargetUser.GetClient().GetHabbo().Username + "</font>";

                                if (TargetUser.GetClient().GetHabbo().chatHTMLColour == "")
                                    Username = TargetUser.GetClient().GetHabbo().Username;
                            }
                            else
                            {
                                 Username = TargetUser.GetClient().GetHabbo().Username;
                                if (TargetUser.GetClient().GetHabbo()._NamePrefixColor == "rainbow" && TargetUser.GetClient().GetHabbo().chatHTMLColour != "rainbow")
                                {
                                    Username = "<font color='" + GalaxyServer.RainbowT() + "'>[" + TargetUser.GetClient().GetHabbo()._NamePrefix + "]</font> <font color='" + TargetUser.GetClient().GetHabbo().chatHTMLColour + "'>" + TargetUser.GetClient().GetHabbo().Username + "</font>";
                                }
                                else if (TargetUser.GetClient().GetHabbo()._NamePrefixColor == "rainbow" && TargetUser.GetClient().GetHabbo().chatHTMLColour == "rainbow")
                                {
                                    Username = "<font color='" + GalaxyServer.RainbowT() + "'>[" + TargetUser.GetClient().GetHabbo()._NamePrefix + "]</font> <font color='" + GalaxyServer.Rainbow() + "'>" + TargetUser.GetClient().GetHabbo().Username + "</font>";
                                }
                                else if (TargetUser.GetClient().GetHabbo()._NamePrefixColor != "rainbow" && TargetUser.GetClient().GetHabbo().chatHTMLColour == "rainbow")
                                {
                                    Username = "<font color='" + TargetUser.GetClient().GetHabbo()._NamePrefixColor + "'>[" + TargetUser.GetClient().GetHabbo()._NamePrefix + "]</font> <font color='" + GalaxyServer.Rainbow() + "'>" + TargetUser.GetClient().GetHabbo().Username + "</font>";
                                }
                                else
                                {
                                    Username = "<font color='" + TargetUser.GetClient().GetHabbo()._NamePrefixColor + "'>[" + TargetUser.GetClient().GetHabbo()._NamePrefix + "]</font> <font color='" + TargetUser.GetClient().GetHabbo().chatHTMLColour + "'>" + TargetUser.GetClient().GetHabbo().Username + "</font>";
                                }
                            }

                                Room.SendMessage(new UserNameChangeComposer(Room.Id, TargetUser.VirtualId, Username));
                                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, Message, 0, TargetUser.LastBubble));
                                Room.GetWired().TriggerEvent(WiredBoxType.TriggerUserSaysCommand, TargetUser.GetClient().GetHabbo(), Message);
                                Room.GetWired().TriggerEvent(WiredBoxType.TriggerUserSays, TargetUser.GetClient().GetHabbo(), Message);
                                Room.SendMessage(new UserNameChangeComposer(Room.Id, TargetUser.VirtualId, NomeReal));

                            
                        }
                        else
                        {
                            Session.SendWhisper("Você não pode usar makesay nesse usuário.");
                        }
                        }
                        else
                            Session.SendWhisper("Usuário não encontrado");
                }
            }
        }
    }
