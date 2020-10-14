using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms.Chat.Commands;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class AdicionarVideoTVCommand : IChatCommand
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
            get { return "Adiciona um vídeo a sua playlist."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Falta o link do vídeo!");
                return;
            }

            string link = Params[1];


            if (link.Contains("youtube.com") || link.Contains("youtu.be"))
            {
                string IdVideo = GalaxyServer.YoutubeVideoId(link);

                if (IdVideo == null || IdVideo == "")
                {
                    Session.SendWhisper("Link do Youtube inválido!");
                    return;
                }
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `tv_videos` (`user`, `video`, `nome`, `canal`) VALUES (@iduser, @idvideo, @nome, @canal);");
                    dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                    dbClient.AddParameter("idvideo", "youtube:"+IdVideo);
                    dbClient.AddParameter("nome", GalaxyServer.GetVideoNameYoutube(IdVideo));
                    dbClient.AddParameter("canal", "Youtube - "+ GalaxyServer.GetVideoCanalYoutube(IdVideo));
                    dbClient.RunQuery();
                }
                Session.SendWhisper("O vídeo foi inserido à sua playlist.");
            }
            else if (link.Contains("https://www.pornhub.com/view_video.php?viewkey=") || link.Contains("https://pt.pornhub.com/view_video.php?viewkey="))
            {
                string IdVideo = link.Replace("https://www.pornhub.com/view_video.php?viewkey=", "").Replace("https://pt.pornhub.com/view_video.php?viewkey=", "");
                string nomeVideo = GalaxyServer.GetWebPageTitle("https://www.pornhub.com/embed/"+ IdVideo).Replace("- Pornhub.com", "");

                if (nomeVideo == null)
                    nomeVideo = "+18 - Vídeo pornô - " + IdVideo;

                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `tv_videos` (`user`, `video`, `nome`, `canal`) VALUES (@iduser, @idvideo, @nome, @canal);");
                    dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                    dbClient.AddParameter("idvideo", "pornhub:" + IdVideo);
                    dbClient.AddParameter("nome", nomeVideo);
                    dbClient.AddParameter("canal", "PornHub");
                    dbClient.RunQuery();
                }
                Session.SendWhisper("O vídeo foi inserido à sua playlist.");
            }
            else if (link.Contains("https://www.twitch.tv/"))
            {
                string IdVideo = link.Replace("https://www.twitch.tv/", "");
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `tv_videos` (`user`, `video`, `nome`, `canal`) VALUES (@iduser, @idvideo, @nome, @canal);");
                    dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                    dbClient.AddParameter("idvideo", "twitch:" + IdVideo);
                    dbClient.AddParameter("nome", "Stream de " + IdVideo);
                    dbClient.AddParameter("canal", "Twitch");
                    dbClient.RunQuery();
                }
                Session.SendWhisper("O vídeo foi inserido à sua playlist.");
            }
            else if (link.Contains("https://www.xvideos.com/video") && Session.GetHabbo().Rank > 10 || link.Contains("http://www.xvideos.com/video") && Session.GetHabbo().Rank > 10)
            {
                string IdVideo = link.Replace("https://www.xvideos.com/video", "").Replace("http://www.xvideos.com/video", "").Split('/')[0];

                if (System.Convert.ToInt32(IdVideo) > 0)
                {

                    string nomeVideo = GalaxyServer.GetWebPageTitle(link).Replace("- XVIDEOS.COM", "");

                    if (nomeVideo == null)
                        nomeVideo = "+18 - Vídeo pornô - " + IdVideo;

                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("INSERT INTO `tv_videos` (`user`, `video`, `nome`, `canal`) VALUES (@iduser, @idvideo, @nome, @canal);");
                        dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                        dbClient.AddParameter("idvideo", "xvideos:" + IdVideo);
                        dbClient.AddParameter("nome", nomeVideo);
                        dbClient.AddParameter("canal", "Xvideos");
                        dbClient.RunQuery();
                    }
                    Session.SendWhisper("O vídeo foi inserido à sua playlist.");
                }
                else
                {
                    Session.SendWhisper("Confira o link e tente novamente.");
                }
            }
            else
            {
                Session.SendWhisper("Precisa ser um link completo do YouTube, Twitch, Pornhub ou Xvideos.");
            }
            return;
        }
    }
}