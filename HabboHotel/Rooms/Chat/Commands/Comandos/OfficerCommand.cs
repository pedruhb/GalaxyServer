using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.Database.Interfaces;
using System.Data;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class OfficerCommand : IChatCommand
    {

        public string PermissionRequired
        {
            get { return "command_virar_policial"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Faz de você um oficial da lei!"; ; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (Session.GetHabbo().Rank >= 2 && Session.GetHabbo().isOfficer == false)
            {
                Session.GetHabbo().isOfficer = true;
                ThisUser.ApplyEffect(19);
                Session.SendWhisper("Você agora é um policial!");
            }
            else if (Session.GetHabbo().Rank >= 3 && Session.GetHabbo().isOfficer == true)
            {
               Session.GetHabbo().isOfficer = false;
               ThisUser.ApplyEffect(0);
               Session.SendWhisper("Você não é mais um policial!");
            }
            else
            {
                Session.SendWhisper("Você não tem permissão para usar isso!");
            }
        }
    }
}