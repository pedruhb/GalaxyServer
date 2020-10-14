using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Session;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class PrisonCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_prender"; }
        }

        public string Parameters
        {
            get { return "[USUARIO]"; }
        }

        public string Description
        {
            get { return "Prender um jogador"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            GameClient TargetClient = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(Params[1]);

            if (Session.GetHabbo().isLoggedIn == false)
            {
                Session.SendWhisper("Você não logou como staff!");
                return;
            }

            if (Params.Length == 1)
            {
                Session.SendWhisper("Escolha o usuário que você quer prender!");
                return;
            }

            if (TargetClient.GetHabbo().Id == Session.GetHabbo().Id)
            {
                Session.SendWhisper("Você não pode prender-se!");
            }

            if (TargetClient.GetHabbo().Username == null)
            {
                Session.SendWhisper("O usuário não existe!");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_soft_ban") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Você não pode prender esse usuário! (O cargo desse usuário é maior que o seu!)");
                return;
            }

            if (TargetClient.GetHabbo().Rank > 10)
            {
                Session.SendWhisper("Você não pode prender esse usuário!");
                return;
            }

            if (TargetClient.GetHabbo().Id != Session.GetHabbo().Id && Session.GetHabbo().isOfficer || TargetClient.GetHabbo().Id != Session.GetHabbo().Id && Session.GetHabbo().Rank > 14)
            {

                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {

                    string figure = TargetClient.GetHabbo().Look;
                    GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, "O Usuário " + Params[1] + " foi preso!", ""));


                    dbClient.RunQuery("UPDATE users SET Presidio = 'true' WHERE id = " + TargetClient.GetHabbo().Id + ";");
                    dbClient.RunQuery("UPDATE users SET motto = 'Eu sou um presidiário!' WHERE ID = '" + TargetClient.GetHabbo().Id + "' LIMIT 1");

                    Session.SendWhisper("Você prendeu o jogador com exito!");
                    TargetClient.SendWhisper("Você foi preso!");

                }
                Session.SendWhisper(TargetClient.GetHabbo().Username + " agora está preso e foi enviado para a prisão!");

                if (!TargetClient.GetHabbo().InRoom)
                    TargetClient.SendMessage(new RoomForwardComposer(GalaxyServer.Prisao));
                else
                    TargetClient.GetHabbo().PrepareRoom(GalaxyServer.Prisao, "");

            }
            else
            {
                Session.SendWhisper("Você não tem acesso a isso, ou você não está no modo policial!");
            }
        }
    }
}