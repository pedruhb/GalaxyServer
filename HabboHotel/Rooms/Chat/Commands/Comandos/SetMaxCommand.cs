using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class SetMaxCommand : IChatCommand
    {
        public string PermissionRequired => "command_setmax";
        public string Parameters => "[NUMERO]";
        public string Description => "Limitar usuarios da sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {

            if (!Room.CheckRights(Session, true))
                return;

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, introduzca um valor para o límite de visitantes da sala.");
                return;
            }

            int MaxAmount;
            if (int.TryParse(Params[1], out MaxAmount))
            {
                if (MaxAmount == 0)
                {
                    MaxAmount = 10;
                    Session.SendWhisper("Número de visitantes muito baixo, o número de visitantes foi definido para 10.");
                }
                else if (MaxAmount > 200 && !Session.GetHabbo().GetPermissions().HasRight("override_command_setmax_limit"))
                {
                    MaxAmount = 200;
                    Session.SendWhisper("Número acima do limite, máximo de visitantes foi definido para 200.");
                }
                else
                    Session.SendWhisper("Quantidade de visitantes estabelecida para " + MaxAmount + ".");

                Room.UsersMax = MaxAmount;
                Room.RoomData.UsersMax = MaxAmount;
                Room.SendMessage(RoomNotificationComposer.SendBubble("setmax", "" + Session.GetHabbo().Username + " estabeleceu a capacidade de " + MaxAmount + " usuários no quarto.", ""));
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `rooms` SET `users_max` = " + MaxAmount + " WHERE `id` = '" + Room.Id + "' LIMIT 1");
                }
            }
            else
                Session.SendWhisper("número inválido, por favor insira um número válido.");
        }
    }
}
