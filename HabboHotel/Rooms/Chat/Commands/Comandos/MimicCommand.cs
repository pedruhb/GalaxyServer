using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using System.Data;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class MimicCommand : IChatCommand
    {
        public string PermissionRequired => "command_mimic";
        public string Parameters => "[USUARIO]"; 
        public string Description => "Copiar visual!";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, coloque o nome do usuário que deseja copiar!");
                return;
            }

            string prefixospace = Params[1];
            if (prefixospace.ToLower().Contains("drop") || prefixospace.ToLower().Contains("update") ||
                prefixospace.ToLower().Contains("select") || prefixospace.ToLower().Contains("alter") ||
                prefixospace.ToLower().Contains("drop") || prefixospace.ToLower().Contains("where"))
            {
                Session.SendWhisper("Você é um lixo!");
                return;
            }

            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);

            DataRow Table;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT username,look,gender,allow_mimic FROM `users` WHERE username = @username LIMIT 1");
                dbClient.AddParameter("username", Params[1]);
                Table = dbClient.getRow();
            } 

            if(TargetClient.GetHabbo().AllowMimic == false)
            {
                if (Session.GetHabbo().Rank < 5)
                {
                    Session.SendWhisper("O usuário desativou o copiar.");
                    return;
                }
            }

            Session.GetHabbo().Gender = Table["gender"].ToString();
            Session.GetHabbo().Look = Table["look"].ToString();

            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `gender` = @gender, `look` = @look WHERE `id` = @id LIMIT 1");
                dbClient.AddParameter("gender", Session.GetHabbo().Gender);
                dbClient.AddParameter("look", Session.GetHabbo().Look);
                dbClient.AddParameter("id", Session.GetHabbo().Id);
                dbClient.RunQuery();
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User != null)
            {
                Session.SendMessage(new UserChangeComposer(User, true));
                Room.SendMessage(new UserChangeComposer(User, false));
                Session.SendMessage(new AvatarAspectUpdateMessageComposer(Session.GetHabbo().Look, Session.GetHabbo().Gender)); //esto
            }
        }
    }
}