using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Database.Interfaces;
using System.Collections.Generic;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class HideWiredCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_hidewired"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Esconda os wireds do quarto."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (!Room.CheckRights(Session, false, false))
            {
                Session.SendWhisper("Você não pode fazer isso nesse quarto.");
                return;
            }

            Room.HideWired = !Room.HideWired;
            if (Room.HideWired) 
                Session.SendWhisper("Agora os Wireds não podem ser vistos.");
            else 
                Session.SendWhisper("Agora os Wireds podem ser vistos.");

            
            using (IQueryAdapter con = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                con.SetQuery("UPDATE `rooms` SET `hide_wired` = @enum WHERE `id` = @id LIMIT 1");
                con.AddParameter("enum", GalaxyServer.BoolToEnum(Room.HideWired));
                con.AddParameter("id", Room.Id);
                con.RunQuery();
                
            }

            List<ServerPacket> list = new List<ServerPacket>();

            list = Room.HideWiredMessages(Room.HideWired);

            Room.SendMessage(list);


        }
    }
}
