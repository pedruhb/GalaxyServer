using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.Database.Interfaces;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RoomCommand : IChatCommand
    {
        public string PermissionRequired => "command_room";
        public string Parameters => "list/push/pull/enables/respect/spush/spull/pets";
        public string Description => "Capacidade de desativar comandos básicos da sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {


            if (Params.Length == 1)
            {
                StringBuilder List = new StringBuilder("");
                List.AppendLine("Lista de comando na sala");
                List.AppendLine("-------------------------");
                List.AppendLine("Pets: " + (Room.PetMorphsAllowed == true ? "Habilitado" : "Desabilitado"));
                List.AppendLine("Puxar: " + (Room.PullEnabled == true ? "Habilitado" : "Desabilitado"));
                List.AppendLine("Empurrar: " + (Room.PushEnabled == true ? "Habilitado" : "Desabilitado"));
                List.AppendLine("Super puxo: " + (Room.SPullEnabled == true ? "Habilitado" : "Desabilitado"));
                List.AppendLine("Super empurro: " + (Room.SPushEnabled == true ? "Habilitado" : "Desabilitado"));
                List.AppendLine("Respeitos: " + (Room.RespectNotificationsEnabled == true ? "Habilitado" : "Desabilitado"));
                List.AppendLine("Efeitos: " + (Room.EnablesEnabled == true ? "Habilitado" : "Desabilitado"));
                Session.SendNotification(List.ToString());
                return;
            }

            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Bem, somente o proprietário da sala ou a equipe pode usar este comando.");
                return;
            }

            string Option = Params[1];
            switch (Option)
            {
                case "list":
                    {
                        StringBuilder List = new StringBuilder("");
                        List.AppendLine("Lista de comando na sala");
                        List.AppendLine("-------------------------");
                        List.AppendLine("Pets: " + (Room.PetMorphsAllowed == true ? "Habilitado" : "Desabilitado"));
                        List.AppendLine("Puxar: " + (Room.PullEnabled == true ? "Habilitado" : "Desabilitado"));
                        List.AppendLine("Empurrar: " + (Room.PushEnabled == true ? "Habilitado" : "Desabilitado"));
                        List.AppendLine("Super puxo: " + (Room.SPullEnabled == true ? "Habilitado" : "Desabilitado"));
                        List.AppendLine("Super empurro: " + (Room.SPushEnabled == true ? "Habilitado" : "Desabilitado"));
                        List.AppendLine("Respeitos: " + (Room.RespectNotificationsEnabled == true ? "Habilitado" : "Desabilitado"));
                        List.AppendLine("Efeitos: " + (Room.EnablesEnabled == true ? "Habilitado" : "Desabilitado"));
                        Session.SendNotification(List.ToString());
                        break;
                    }

                case "golpe":
                case "golpes":
                    {
                        Room.GolpeEnabled = !Room.GolpeEnabled;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `golpe_enabled` = @GolpeEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("GolpeEnabled", GalaxyServer.BoolToEnum(Room.GolpeEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Golpes nesta sala " + (Room.GolpeEnabled == true ? "Habilitado!" : "Desabilitado!"));
                        break;
                    }

                case "push":
                case "empurrar":
                    {
                        Room.PushEnabled = !Room.PushEnabled;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `push_enabled` = @PushEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PushEnabled", GalaxyServer.BoolToEnum(Room.PushEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("O empurrar agora está " + (Room.PushEnabled == true ? "Habilitado!" : "Desabilitado!"));
                        break;
                    }

                case "spush":
                    {
                        Room.SPushEnabled = !Room.SPushEnabled;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `spush_enabled` = @PushEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PushEnabled", GalaxyServer.BoolToEnum(Room.SPushEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("O Super empurrão agora está " + (Room.SPushEnabled == true ? "Habilitado!" : "Desabilitado!"));
                        break;
                    }

                case "spull":
                    {
                        Room.SPullEnabled = !Room.SPullEnabled;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `spull_enabled` = @PullEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PullEnabled", GalaxyServer.BoolToEnum(Room.SPullEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("O Super puxo agora está  " + (Room.SPullEnabled == true ? "Habilitado!" : "Desabilitado!"));
                        break;
                    }

                case "puxar":
                case "pull":
                    {
                        Room.PullEnabled = !Room.PullEnabled;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `pull_enabled` = @PullEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PullEnabled", GalaxyServer.BoolToEnum(Room.PullEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("O puxar agora está " + (Room.PullEnabled == true ? "Habilitado!" : "Desabilitado!"));
                        break;
                    }

                case "enable":
                case "enables":
                case "efeito":
                    {
                        Room.EnablesEnabled = !Room.EnablesEnabled;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `enables_enabled` = @EnablesEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("EnablesEnabled", GalaxyServer.BoolToEnum(Room.EnablesEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Os efeitos da sala estão " + (Room.EnablesEnabled == true ? "Habilitados!" : "Desabilitados!"));
                        break;
                    }

                case "respect":
                case "respeitos":
                    {
                        Room.RespectNotificationsEnabled = !Room.RespectNotificationsEnabled;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `respect_notifications_enabled` = @RespectNotificationsEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("RespectNotificationsEnabled", GalaxyServer.BoolToEnum(Room.RespectNotificationsEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Notificação de respeito " + (Room.RespectNotificationsEnabled == true ? "Habilitado!" : "Desabilitado!"));
                        break;
                    }

                case "pets":
                case "morphs":
                    {
                        Room.PetMorphsAllowed = !Room.PetMorphsAllowed;
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `pet_morphs_allowed` = @PetMorphsAllowed WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PetMorphsAllowed", GalaxyServer.BoolToEnum(Room.PetMorphsAllowed));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Os pets nessa sala estão " + (Room.PetMorphsAllowed == true ? "Habilitado!" : "Desabilitado!"));

                        if (!Room.PetMorphsAllowed)
                        {
                            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers())
                            {
                                if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                                    continue;

                                User.GetClient().SendWhisper("O quarto proprietário desativou a opção de se tornar um animal de estimação.");
                                if (User.GetClient().GetHabbo().PetId > 0)
                                {
                                    //Tell the user what is going on.
                                    User.GetClient().SendWhisper("Oops, o proprietário da sala só permite que os usuários comuns, sem animais..");

                                    //Change the users Pet Id.
                                    User.GetClient().GetHabbo().PetId = 0;

                                    //Quickly remove the old user instance.
                                    Room.SendMessage(new UserRemoveComposer(User.VirtualId));

                                    //Add the new one, they won't even notice a thing!!11 8-)
                                    Room.SendMessage(new UsersComposer(User));
                                }
                            }
                        }
                        break;
                    }
            }
        }
    }
}
