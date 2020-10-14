using Galaxy.Communication.Packets.Outgoing.Moderation;
using Galaxy.Core;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RestartGalaxyServerCommand : IChatCommand
    {
        public string PermissionRequired => "command_reiniciar";
        public string Parameters => "["+ ExtraSettings.ReiniciarPermissao + "]";
        public string Description => "Reinicia o emulador.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Session.GetHabbo().Username == ExtraSettings.ReiniciarPermissao || Session.GetHabbo().Id == 1 || Session.GetHabbo().Rank >= 15) { 
            GalaxyServer.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer("O Galaxy Server será reinciado! após a client ser desconectada entre novamente em 30 segundos." + "\r\n" + "Administração "+ GalaxyServer.HotelName));
            System.Threading.Thread.Sleep(5000);
            ExceptionLogger.DisablePrimaryWriting(true);
            GalaxyServer.PerformShutDown(true);} else { Session.SendWhisper("Apenas o "+ ExtraSettings.ReiniciarPermissao+" pode executar esse comando!");  }
        }
    }
}

