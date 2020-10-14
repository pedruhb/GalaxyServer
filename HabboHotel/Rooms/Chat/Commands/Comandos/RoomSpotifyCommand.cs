using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RoomSpotifyCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return "[LINK]"; }
        }

        public string Description
        {
            get { return "Enviar uma música ou playlist do spotify para o quarto."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Você deve por o link do spotify.");
                return;
            }

            if (Session.GetHabbo().Rank < 5 && Session.GetHabbo().Id != Room.OwnerId)
            {
                Session.SendWhisper("Somente o dono pode colocar vídeos.");
                return;
            }

            string link = Params[1];

            if (link.Contains("https://open.spotify.com/track/"))
            {
                link = link.Replace("https://open.spotify.com/", "").Split('?')[0];
                dynamic product = new JObject();
                product.tipo = "spotify";
                product.id = link;
                List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
                if (Users.Count > 0)
                {
                    foreach (RoomUser U in Users.ToList())
                    {
                        GalaxyServer.SendUserJson(U, product);
                    }
                }
                Session.SendWhisper("A música abrirá na client dos usuários.");
            }
            else if (link.Contains("https://open.spotify.com/playlist/"))
            {
                link = link.Replace("https://open.spotify.com/", "").Split('?')[0];
                dynamic product = new JObject();
                product.tipo = "spotify";
                product.id = link;
                List<RoomUser> Users = Room.GetRoomUserManager().GetRoomUsers();
                if (Users.Count > 0)
                {
                    foreach (RoomUser U in Users.ToList())
                    {
                        GalaxyServer.SendUserJson(U, product);
                    }
                }
                Session.SendWhisper("A playlist abrirá na client dos usuários.");
            }
            else
            {
                Session.SendWhisper("Precisa ser um link do Spotify.");
            }
            return;
        }
    }
}