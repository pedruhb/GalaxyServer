using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class PayCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_pay"; }
        }
        public string Parameters
        {
            get { return "[USUARIO] [QNT] [TIPO]"; }
        }
        public string Description
        {
            get { return "Pague um usuário."; }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                System.Text.StringBuilder List = new System.Text.StringBuilder("");
                List.AppendLine("Efetue pagamentos com o comando pay!");
                List.AppendLine(":pay " + Session.GetHabbo().Username + " 100 diamantes - Paga 100 " + ExtraSettings.NomeDiamantes + " ao usuário " + Session.GetHabbo().Username + "!");
                List.AppendLine(":pay " + Session.GetHabbo().Username + " 200 creditos - Paga 200 " + ExtraSettings.NomeMoedas + " ao usuário " + Session.GetHabbo().Username + "!");
				if (GalaxyServer.Tipo != 0) List.AppendLine(":pay " + Session.GetHabbo().Username + " 300 duckets - Paga 300 " + ExtraSettings.NomeDuckets + " ao usuário " + Session.GetHabbo().Username + "!");
                List.AppendLine("");
                List.AppendLine("Todas as transações são registradas no banco de dados!");
                Session.SendNotification(List.ToString());

                Session.SendWhisper("Digite o nick de quem você deseja pagar.");
                return;
            }
            if (Params.Length == 2)
            {
                Session.SendWhisper("Digite a quantidade que deseja pagar.");
                return;
            }
            if (Params.Length == 3)
            {
                Session.SendWhisper("Digite o tipo de moeda que deseja pagar.");
                return;
            }
            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Essa pessoa não se encontra no quarto ou não está online.");
                return;
            }
            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(TargetClient.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Essa pessoa não se encontra no quarto ou não está online.");
				return;
            }
            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode fazer pagar a si mesmo!");
                return;
            }
            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
                return;
            string ApenasNumeros(string str)
            {
                var apenasDigitos = new System.Text.RegularExpressions.Regex(@"[^\d]");
                return apenasDigitos.Replace(str, "");
            }
            string Valor = ApenasNumeros(Params[2]);
            string Tipo = Params[3].ToLower();
            var Sucesso = false;

            if (Convert.ToInt32(Valor) <= 0)
            {
                Session.SendWhisper("Valor inválido!");
                return;
            }

            if (Tipo == "diamantes" || Tipo == "diamante" || Tipo == "dima")
            {
               if (Session.GetHabbo().Diamonds >= Convert.ToInt32(Valor))
                {
                    Session.GetHabbo().Diamonds -= Convert.ToInt32(Valor); /// Remove os diamantes
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5)); /// Atualiza diamantes.

                    TargetClient.GetHabbo().Diamonds += Convert.ToInt32(Valor); /// Dá os diamantes
                    TargetClient.SendMessage(new HabboActivityPointNotificationComposer(TargetClient.GetHabbo().Diamonds, 0, 5)); /// Atualiza diamantes.

                    Sucesso = true;
                } else
                {
                    Session.SendWhisper("Você não tem " + Tipo + " o suficiente para fazer isso.");
                    Sucesso = false;
                }
            }
            else if (Tipo == "moedas" || Tipo == "creditos" || Tipo == "moeda")
            {
                if (Session.GetHabbo().Credits >= Convert.ToInt32(Valor))
                {
                    Session.GetHabbo().Credits -= Convert.ToInt32(Valor); /// Remove os créditos.
                    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits)); /// Atualiza moedas.

                    TargetClient.GetHabbo().Credits += Convert.ToInt32(Valor); /// Dá os créditos.
                    TargetClient.SendMessage(new CreditBalanceComposer(TargetClient.GetHabbo().Credits)); /// Atualiza moedas.

                    Sucesso = true;
                }
                else
                {
                    Session.SendWhisper("Você não tem " + Tipo + " o suficiente para fazer isso.");
                    Sucesso = false;
                }
            }
			else if (Tipo == "duckets" || Tipo == "pixels" || Tipo == "ducket")
			  {
				if (GalaxyServer.Tipo == 1)
				{
					if (Session.GetHabbo().Duckets >= Convert.ToInt32(Valor))
					{
						Session.GetHabbo().Duckets -= Convert.ToInt32(Valor); /// Remove Duckets
						Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Session.GetHabbo().Duckets)); /// Atualiza

						TargetClient.GetHabbo().Duckets += Convert.ToInt32(Valor); /// Dá duckets
						TargetClient.SendMessage(new HabboActivityPointNotificationComposer(TargetClient.GetHabbo().Duckets, TargetClient.GetHabbo().Duckets)); /// Atualiza

						Sucesso = true;
					}
					else
					{
						Session.SendWhisper("Você não tem " + Tipo + " o suficiente para fazer isso.");
						Sucesso = false;
					}
				}
				else
				{
					Session.SendWhisper("Tipo de moeda \"" + Tipo + "\" não encontrado!");
					Sucesso = false;
				}
			  }
			else
            {
                Session.SendWhisper("Tipo de moeda \"" + Tipo + "\" não encontrado!");
                Sucesso = false;
            }

            if(Sucesso)
            {
            Session.SendWhisper("Você acabou de efetuar um pagamento de "+ Valor + " "+ Tipo + " para "+TargetClient.GetHabbo().Username + ".");
            TargetClient.SendWhisper("Você acabou de receber " + Valor + " " + Tipo + " de " + Session.GetHabbo().Username+".");
            IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor();
                dbClient.runFastQuery("INSERT INTO `logs_pay` (`de`, `para`, `tipo`, `quantidade`, `data`) VALUES ('"+ Session.GetHabbo().Id+ "', '"+TargetClient.GetHabbo().Id+ "', '"+Tipo+"', '"+Valor+"', '"+GalaxyServer.GetUnixTimestamp()+"');"); /// insere log na tabela
            }
            else
            {
                Session.SendWhisper("Houve um erro ao fazer a transação.");
                TargetClient.SendWhisper("O usuário " + Session.GetHabbo().Username + " tentou fazer uma transação, porém obteve um erro..");
            }

        }
    }
}