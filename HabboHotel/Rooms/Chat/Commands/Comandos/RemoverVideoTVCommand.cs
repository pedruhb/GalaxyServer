using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using System;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class RemoverVideoTVCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return "[LINK ou ID]"; }
        }

        public string Description
        {
            get { return "Remova um vídeo da sua playlist."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Falta o link ou id do vídeo! (veja o id usando o comando :meusvideos)");
                return;
            }

            string link = Params[1];

            int ItemId = 0;
            if (int.TryParse(Convert.ToString(link), out ItemId))
            {
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("DELETE FROM `tv_videos` WHERE user = @iduser AND id = @idvideo");
                    dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                    dbClient.AddParameter("idvideo", ItemId);
                    dbClient.RunQuery();
                }
                Session.SendWhisper("O vídeo foi removido de sua playlist.");
            }
            else
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
                    dbClient.SetQuery("DELETE FROM `tv_videos` WHERE user = @iduser AND video = @idvideo");
                    dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                    dbClient.AddParameter("idvideo", "youtube:" + IdVideo);
                    dbClient.RunQuery();
                }
                Session.SendWhisper("O vídeo foi removido de sua playlist.");
            }
            else if (link.Contains("https://www.pornhub.com/view_video.php?viewkey=") || link.Contains("https://pt.pornhub.com/view_video.php?viewkey="))
            {
                string IdVideo = link.Replace("https://www.pornhub.com/view_video.php?viewkey=", "");
                IdVideo = link.Replace("https://pt.pornhub.com/view_video.php?viewkey=", "");
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("DELETE FROM `tv_videos` WHERE user = @iduser AND video = @idvideo");
                    dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                    dbClient.AddParameter("idvideo", "pornhub:" + IdVideo);
                    dbClient.RunQuery();
                }
                Session.SendWhisper("O vídeo foi removido de sua playlist.");
            }
            else if (link.Contains("https://www.twitch.tv/"))
            {
                string IdVideo = link.Replace("https://www.twitch.tv/", "");
                using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("DELETE FROM `tv_videos` WHERE user = @iduser AND video = @idvideo");
                    dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                    dbClient.AddParameter("idvideo", "twitch:" + IdVideo);
                    dbClient.RunQuery();
                }
                Session.SendWhisper("O vídeo foi removido de sua playlist.");
            }
            else if (link.Contains("https://www.xvideos.com/video") && Session.GetHabbo().Rank > 10 || link.Contains("http://www.xvideos.com/video") && Session.GetHabbo().Rank > 10)
            {
                string IdVideo = link.Replace("https://www.xvideos.com/video", "").Replace("http://www.xvideos.com/video", "").Split('/')[0];

                if (System.Convert.ToInt32(IdVideo) > 0)
                {
                    using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("DELETE FROM `tv_videos` WHERE user = @iduser AND video = @idvideo");
                        dbClient.AddParameter("iduser", Session.GetHabbo().Id);
                        dbClient.AddParameter("idvideo", "xvideos:" + IdVideo);
                        dbClient.RunQuery();
                    }
                    Session.SendWhisper("O vídeo foi removido de sua playlist.");
                }
                else
                {
                    Session.SendWhisper("Confira o link e tente novamente.");
                }
            }
            else
            {
                Session.SendWhisper("Precisa ser um link completo do YouTube ou do PornHub.");
            }
            return;
        }
    }
}