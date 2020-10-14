using Galaxy.Communication.Packets.Outgoing.Messenger;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class GroupChatCommand : IChatCommand
    {
        public string PermissionRequired => "command_groupchat";
        public string Parameters => "[on/off]";
        public string Description => "Ativa chat de grupo";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length < 2)
            {
                Session.SendWhisper("Ocorreu um erro, especifica ON / OFF");
                return;
            }

            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Voce não tem permissão.");
                return;
            }

            if (Room.Group.CreatorId != Session.GetHabbo().Id)
            {
                Session.SendWhisper("Ops! Você não é o proprietário. Chat de grupo deve ser criado pelo proprietário do grupo...");
                return;
            }

            if (Room.Group == null)
            {
                Session.SendWhisper("Este quarto não tem um grupo, se você acabou de criar digite :reload e tente novamente");
                return;
            }

            if (Room.Group.Id != Session.GetHabbo().GetStats().FavouriteGroupId)
            {
                Session.SendWhisper("Você só pode criar um chat de grupo se ele estiver favoritado.");
                return;
            }

            var mode = Params[1].ToLower();
            var group = Room.Group;

            if (mode == "on")
            {
                if (group.HasChat)
                {
                    Session.SendWhisper("Este grupo já tem bate-papo.");
                    return;
                } else
                {
                    Session.SendWhisper("O grupo foi criado, você precisa reentrar para aparecer.");
                }

                group.HasChat = true;

                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE groups SET has_chat = '1' WHERE id = @id");
                    dbClient.AddParameter("id", group.Id);
                    dbClient.RunQuery();
                }

                var Client = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(Session.GetHabbo().Id);
                if (Client != null)
                {
                    Client.SendMessage(new FriendListUpdateComposer(group, 1));
                }
                Session.SendWhisper("Para o chat aparecer você deverá reentrar!");

            }
            else if (mode == "off")
            {
                if (!group.HasChat)
                {
                    Session.SendWhisper("Este grupo não tem bate-papo .");
                    return;
                }

                group.HasChat = false;

                using (var adap = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    adap.SetQuery("UPDATE groups SET has_chat = '0' WHERE id = @id");
                    adap.AddParameter("id", group.Id);
                    adap.RunQuery();
                }
                var Client = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(Session.GetHabbo().Id);
                if (Client != null)
                {
                    Client.SendMessage(new FriendListUpdateComposer(group, -1));
                }
                Session.SendWhisper("Grupo apagado!");
            }
            else
            {
                Session.SendNotification("Ocorreu um erro!");
            }
            

        }
    }
}
