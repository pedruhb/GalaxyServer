
using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.HabboHotel.GameClients;
using System;
using System.Data;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class PremiarCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_premiar"; }
        }

        public string Parameters
        {
            get { return "[USUARIO]"; }
        }

        public string Description
        {
            get { return "Faz todas as funções para premiar um ganhador de evento."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, digite o usuário que deseja premiar!");
                return;
            }
            GameClient Target = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Opa, não foi possível encontrar esse usuário!");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Target.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Usuário não encontrado! Talvez ele não esteja online ou nesta sala.");
                return;
            }

            if (Target.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode se premiar!");
                return;
            }

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (ThisUser == null)
            {
                return;
            }
            else
            {

				DataRow moedasRank = null;
				using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.SetQuery("SELECT * FROM ranks WHERE id = '" + Target.GetHabbo().Rank + "'");
					moedasRank = dbClient.getRow();
				}

				Target.GetHabbo().Credits = Target.GetHabbo().Credits += Convert.ToInt32(moedasRank["moedas_eventos"]);
                Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));
                Target.GetHabbo().Duckets += Convert.ToInt32(moedasRank["duckets_eventos"]);
                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Duckets, 500));
                Target.GetHabbo().Diamonds += Convert.ToInt32(moedasRank["diamantes_eventos"]);
                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Diamonds, Convert.ToInt32(moedasRank["diamantes_eventos"]), 5));
                Target.GetHabbo().GOTWPoints += Convert.ToInt32(moedasRank["gotw_eventos"]);
                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().GOTWPoints, 1, 103));

                if(GalaxyServer.Tipo != 0)
                Target.SendMessage(new RoomNotificationComposer("moedas", "message", "Você ganhou " + Convert.ToInt32(moedasRank["duckets_eventos"]) + " " + ExtraSettings.NomeDuckets + ", " + Convert.ToInt32(moedasRank["moedas_eventos"]) + " " + ExtraSettings.NomeMoedas + ", " + Convert.ToInt32(moedasRank["diamantes_eventos"]) + " " + ExtraSettings.NomeDiamantes + " e " + Convert.ToInt32(moedasRank["gotw_eventos"]) + " " + ExtraSettings.NomeGotw + "!"));
               
                DataRow nivel = null;
                using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT Premiar FROM users WHERE id = '" + Target.GetHabbo().Id + "'");
                    nivel = dbClient.getRow();
                    dbClient.RunQuery("UPDATE users SET Premiar = Premiar + '1' WHERE id = '" + Target.GetHabbo().Id + "'");
                    dbClient.RunQuery("UPDATE users SET ScoreGame = ScoreGame + '1' WHERE id = '" + Target.GetHabbo().Id + "'");
                }
                DataRow mee = null;
                using (var dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT Premiar FROM users WHERE id = '" + Target.GetHabbo().Id + "'");
                    mee = dbClient.getRow();
                }
                GalaxyServer.GetGame().GetAchievementManager().ProgressAchievement(Target, "ACH_Evento", 1);
                string figure = Target.GetHabbo().Look;
                GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, TargetUser.GetUsername() + " ganhou um evento no " + GalaxyServer.HotelName + ".", "Você agora está no nível " + Convert.ToString(nivel["Premiar"]) + "!"));
			//	Console.WriteLine(Convert.ToInt32(mee["Premiar"]));
			//	Console.WriteLine(Convert.ToInt32(ExtraSettings.NiveltotalGames));
				if (Convert.ToInt32(mee["Premiar"]) > Convert.ToInt32(ExtraSettings.NiveltotalGames))
				{
					Session.SendWhisper("O usuário não recebeu emblema, pois ele já está no nível "+ Convert.ToInt32(mee["Premiar"]));
					string emblegama = ExtraSettings.CodEmblemaNivel + Convert.ToString(mee["Premiar"]);
					if (Target.GetHabbo().Id != Session.GetHabbo().Id)
						Target.SendMessage(new RoomNotificationComposer("emblema/" + emblegama, 3, "Você acaba de chegar no nível " + Convert.ToString(mee["Premiar"]) + " !", ""));
				}
				else {
						string emblegama = ExtraSettings.CodEmblemaNivel + Convert.ToString(mee["Premiar"]);
						if (!Target.GetHabbo().GetBadgeComponent().HasBadge(emblegama))
						{
							Target.GetHabbo().GetBadgeComponent().GiveBadge(emblegama, true, Target);
							if (Target.GetHabbo().Id != Session.GetHabbo().Id)
								Target.SendMessage(new RoomNotificationComposer("emblema/" + emblegama, 3, "Você acaba de chegar no nível " + Convert.ToString(mee["Premiar"]) + " !", ""));
						}
						else if (Convert.ToString(mee["Premiar"]) == Convert.ToString(ExtraSettings.NiveltotalGames))
						{
							Session.SendWhisper("Você zerou os níveis, parabéns!");

						}
						else
						{
							Session.SendWhisper("Ops, ocorreu um erro no sistema de dar emblemas automáticos! Erro no emblema: (" + emblegama + ") !");

						}
					}

                /// Kika o usuário
                /// 
                if (GalaxyServer.Tipo == 0)
                {
                    Target.GetHabbo().PrepareRoom(Target.GetHabbo().HomeRoom, "");
                    

                } else
				{
                    Room TargetRoom;
                    if (!GalaxyServer.GetGame().GetRoomManager().TryGetRoom(Target.GetHabbo().CurrentRoomId, out TargetRoom))
                        return;

                    TargetRoom.GetRoomUserManager().RemoveUserFromRoom(Target, true, false);
                }

                    Session.SendWhisper("O usuário foi premiado com sucesso!");
                }
            
        }
    }
}
