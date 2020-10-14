using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Avatar;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class WiredVariable : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_wiredvars"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Veja comandos especiais para usar no wired."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {

            StringBuilder List = new StringBuilder("");
            List.AppendLine(" Comandos para EFEITO WIRED: Mostrar mensagem:");
            List.AppendLine(" %sit% - Faz o usuário sentar.");
            List.AppendLine(" %lay% - Faz o usuário deitar.");
            List.AppendLine(" %stand% - Faz o usuário levantar.");
            List.AppendLine(" %user% / %username% - Mostra o nome do usuário.");
            List.AppendLine(" %userid% - Mostra o ID de registro do usuário.");
            List.AppendLine(" %gotw% - Mostra " + ExtraSettings.NomeGotw + " do usuário.");
            List.AppendLine(" %duckets% - Mostra " + ExtraSettings.NomeDuckets + " do usuário.");
            List.AppendLine(" %diamonds% - Mostra " + ExtraSettings.NomeDiamantes + " do usuário.");
            List.AppendLine(" %credits% - Mostra " + ExtraSettings.NomeMoedas + " do usuário.");
            List.AppendLine(" %rank% - Mostra o Cargo do usuário.");
            List.AppendLine(" %roomname% - Mostra o Nome do quarto.");
            List.AppendLine(" %roomusers% - Mostra quantos usuários tem no quarto.");
            List.AppendLine(" %roomlikes% - Mostra quantas Curtidas o quarto tem.");
            List.AppendLine(" %roomowner% - Mostra o Nome do Dono do Qquarto.");
            List.AppendLine(" %userson% - Mostra a quantidade de usuários no hotel em tempo real.");
            List.AppendLine(" %hotelname% - Mostra o nome do hotel.");
			List.AppendLine(" %versaogalaxy% - Mostra a versão do GalaxyServer.");
			List.AppendLine(" %dolar% - Mostra a cotação atual do dólar.");
			List.AppendLine(" Youtube - Para abrir um vídeo basta colocar o link dele como mensagem.");
			Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
        }
    }
}