
using Galaxy.Communication.ConnectionManager;
using Galaxy.Communication.Packets.Outgoing;
using Galaxy.Communication.Packets.Outgoing.Handshake;
using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Notifications;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.Groups;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Users.Messenger;
using log4net;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Galaxy.HabboHotel.GameClients
{
    public class GameClientManager
    {
        public delegate void ClientDisconnected(GameClient client);
        private static readonly ILog log = LogManager.GetLogger("Galaxy.HabboHotel.GameClients.GameClientManager");
        public ConcurrentDictionary<int, GameClient> _clients;
        private Dictionary<int, GameClient> guides;
        private Dictionary<int, GameClient> alphas;
        private ConcurrentDictionary<int, GameClient> _userIDRegister;
        private ConcurrentDictionary<string, GameClient> _usernameRegister;
		private readonly Queue timedOutConnections;
        private readonly Stopwatch clientPingStopwatch;
        public event ClientDisconnected OnClientDisconnect;

        public GameClientManager()
        {
            guides = new Dictionary<int, GameClient>();
            alphas = new Dictionary<int, GameClient>();
            _clients = new ConcurrentDictionary<int, GameClient>();
            _userIDRegister = new ConcurrentDictionary<int, GameClient>();
            _usernameRegister = new ConcurrentDictionary<string, GameClient>();


			timedOutConnections = new Queue();

            clientPingStopwatch = new Stopwatch();
            clientPingStopwatch.Start();
        }

        public void DispatchEventDisconnect(GameClient client)
        {
			OnClientDisconnect?.Invoke(client);
		}

        public void OnCycle()
        {
            TestClientConnections();
            HandleTimeouts();
            GalaxyServer.GetGame().ClientManagerCycleEnded = true;
        }

        public GameClient GetClientByUserID(int userID)
        {
            if (_userIDRegister.ContainsKey(userID))
                return _userIDRegister[userID];
            return null;
        }

        internal Dictionary<int, GameClient> GetGuides()
        {
            return guides;
        }

        internal Dictionary<int, GameClient> GetAlphas()
        {
            return alphas;
        }

        internal void AddToAlphas(int id, GameClient client)
        {
            //visaUsers[id] = client;
            alphas[id] = client;
        }

        internal void ModifyGuide(bool online, GameClient c)
        {
            if (c == null || c.GetHabbo() == null) return;
            if (online)
                guides[c.GetHabbo().Id] = c;
            else
                guides.Remove(c.GetHabbo().Id);
        }

        public GameClient GetClientByUsername(string username)
        {
            if (_usernameRegister.ContainsKey(username.ToLower()))
                return _usernameRegister[username.ToLower()];
            return null;
        }

        public bool TryGetClient(int ClientId, out GameClient Client)
        {
            return _clients.TryGetValue(ClientId, out Client);
        }

		public bool UpdateClientUsername(GameClient Client, string OldUsername, string NewUsername)
        {
            if (Client == null || !_usernameRegister.ContainsKey(OldUsername.ToLower()))
                return false;

            _usernameRegister.TryRemove(OldUsername.ToLower(), out Client);
            _usernameRegister.TryAdd(NewUsername.ToLower(), Client);
            return true;
        }

        public string GetNameById(int Id)
        {
            GameClient client = GetClientByUserID(Id);

            if (client != null)
                return client.GetHabbo().Username;

            string username;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT username FROM users WHERE id = @id LIMIT 1");
                dbClient.AddParameter("id", Id);
                username = dbClient.getString();
            }

            return username;
        }

        public IEnumerable<GameClient> GetClientsById(Dictionary<int, MessengerBuddy>.KeyCollection users)
        {
            foreach (int id in users)
            {
                GameClient client = GetClientByUserID(id);
                if (client != null)
                    yield return client;
            }
        }

		public void StaffAlert(ServerPacket Message, int Exclude = 0)
		{
			foreach (GameClient client in GetClients.ToList())
			{
				if (client == null || client.GetHabbo() == null)
					continue;

				if (client.GetHabbo().Rank < 10 || client.GetHabbo().Id == Exclude)
					continue;

				client.SendMessage(Message);
			}
		}

		public void SendJsonStaff(string json)
		{
			foreach (GameClient client in GetClients.ToList())
			{
				if (client == null || client.GetHabbo() == null)
					continue;

				if (client.GetHabbo().Rank < 10)
					continue;

				GalaxyServer.SendUserJson(client, json);
			}
		}

		public void StaffAlertRank(ServerPacket Message, int Rank)
		{
			foreach (GameClient client in GetClients.ToList())
			{
				if (client == null || client.GetHabbo() == null)
					continue;

				if (client.GetHabbo().Rank == Rank)
					client.SendMessage(Message);

			}
		}

        public void EventAlertPHBLindo(string Usuario, int IdQuarto, string Mensagem)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                    continue;


                if (client.GetHabbo().AllowEvents == true)
                    client.SendMessage(new RoomNotificationComposer("Está acontecendo um evento!",
                           "Está acontecendo um novo evento realizado pela equipe!<br><br>Este, tem o intuito de proporcionar um entretenimento a mais para os usuários!<br><br>Evento:<b>  " + Mensagem +
                           "</b><br>Por:<b>  " + Usuario +
                           "</b> <br><br>Caso deseje participar, clique no botão abaixo! <br><br>Para desativar os alertas digite <b> :eventosoff</b><br>",
                           GalaxyServer.HotelName + "-evento", "Participar do Evento", "event:navigator/goto/" + IdQuarto));
                else
                    client.SendWhisper("Está acontecendo um novo evento no " + GalaxyServer.HotelName + ", para receber notificações novamente digite :eventosoff");

            }
        }

        public void EventAlertJson(object product)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                    continue;


                if (client.GetHabbo().AllowEvents == true)
                    GalaxyServer.SendUserJson(client, product);
                else
                    client.SendWhisper("Está acontecendo um novo evento no " + GalaxyServer.HotelName + ", para receber notificações novamente digite :eventosoff");

            }
        }

        public void VipAlert(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                    continue;

				if(client.GetHabbo().VIPRank > 0)
				client.SendMessage(Message);
            }
        }

        public void QuizzAlert(ServerPacket Message, Item Item, Room room, int Exclude = 0)
        {
            foreach (RoomUser RoomUser in room.GetRoomUserManager().GetRoomUsers())
            {
                if (RoomUser == null || RoomUser.GetClient().GetHabbo() == null)
                    continue;

                RoomUser Human = room.GetRoomUserManager().GetRoomUserByHabbo(RoomUser.GetClient().GetHabbo().Id);

                if (Human.X != Item.GetX && Human.Y != Item.GetY || RoomUser.GetClient().GetHabbo().Id == Exclude)
                    continue;

                RoomUser.GetClient().SendMessage(Message);
            }
        }

        public void ManagerAlert(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                    continue;

                if (client.GetHabbo().Rank < 9 || client.GetHabbo().Id == Exclude)
                    continue;

                client.SendMessage(Message);
            }
        }

        public void GroupChatAlert(ServerPacket Message, Group Group, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                    continue;

                if (!Group.IsMember(client.GetHabbo().Id) || client.GetHabbo().Id == Exclude)
                    continue;

                client.SendMessage(Message);
            }
        }

        public void GuideAlert(ServerPacket Message, int Exclude = 0)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                    continue;

                if (client.GetHabbo()._guidelevel < 1 || client.GetHabbo().Id == Exclude)
                    continue;

                client.SendMessage(Message);
            }
        }

        public void LogsNotif(string Message, string Key)
        {
            GalaxyServer.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer(Message, Key), 0);
        }

        public void SendBubble(string Message, string Key)
        {
            GalaxyServer.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer(Message, Key));
        }

        public void ModAlert(string Message)
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null)
                    continue;

                if (client.GetHabbo().GetPermissions().HasRight("mod_tool") && !client.GetHabbo().GetPermissions().HasRight("staff_ignore_mod_alert"))
                {
                    try { client.SendWhisper(Message, 5); }
                    catch { }
                }
            }
        }

        public void DoAdvertisingReport(GameClient Reporter, GameClient Target)
        {
            if (Reporter == null || Target == null || Reporter.GetHabbo() == null || Target.GetHabbo() == null)
                return;

            StringBuilder Builder = new StringBuilder();
            Builder.Append("Novo relatório!\r\r");
            Builder.Append("Reportador: " + Reporter.GetHabbo().Username + "\r");
            Builder.Append("Usuário reportado: " + Target.GetHabbo().Username + "\r\r");
            Builder.Append(Target.GetHabbo().Username + "Ultimos 10 msj:\r\r");

            DataTable GetLogs = null;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `message` FROM `chatlogs` WHERE `user_id` = '" + Target.GetHabbo().Id + "' ORDER BY `id` DESC LIMIT 10");
                GetLogs = dbClient.getTable();

                if (GetLogs != null)
                {
                    int Number = 11;
                    foreach (DataRow Log in GetLogs.Rows)
                    {
                        Number -= 1;
                        Builder.Append(Number + ": " + Convert.ToString(Log["message"]) + "\r");
                    }
                }
            }

            foreach (GameClient Client in GetClients.ToList())
            {
                if (Client == null || Client.GetHabbo() == null)
                    continue;

                if (Client.GetHabbo().GetPermissions().HasRight("mod_tool") && !Client.GetHabbo().GetPermissions().HasRight("staff_ignore_advertisement_reports"))
                    Client.SendMessage(new MOTDNotificationComposer(Builder.ToString()));
            }
        }


		public void SendMessage(ServerPacket Packet, string fuse = "")
		{
			foreach (GameClient Client in _clients.Values.ToList())
			{
				if (Client == null || Client.GetHabbo() == null)
					continue;

				if (!string.IsNullOrEmpty(fuse))
				{
					if (!Client.GetHabbo().GetPermissions().HasRight(fuse))
						continue;
				}

				Client.SendMessage(Packet);
			}
		}

		public void SendJson(string json)
		{
			foreach (GameClient Client in _clients.Values.ToList())
			{
				if (Client == null || Client.GetHabbo() == null)
					continue;

				GalaxyServer.SendUserJson(Client, json);
			}
		}

		public void CreateAndStartClient(int clientID, ConnectionInformation connection)
        {
            GameClient Client = new GameClient(clientID, connection);
            if (_clients.TryAdd(Client.ConnectionID, Client))
                Client.StartConnection();
            else
                connection.Dispose();
        }

        public void DisposeConnection(int clientID)
        {
			if (!TryGetClient(clientID, out GameClient Client))
				return;

			if (Client != null)
            {

                if (OnClientDisconnect != null && Client.GetHabbo() != null)
                    OnClientDisconnect(Client);

                Client.Dispose();
            }

            _clients.TryRemove(clientID, out Client);
        }

        public void LogClonesOut(int UserID)
        {
            GameClient client = GetClientByUserID(UserID);
            if (client != null)
                client.Disconnect();
        }

        public void RegisterClient(GameClient client, int userID, string username)
        {
            if (_usernameRegister.ContainsKey(username.ToLower()))
                _usernameRegister[username.ToLower()] = client;
            else
                _usernameRegister.TryAdd(username.ToLower(), client);

            if (_userIDRegister.ContainsKey(userID))
                _userIDRegister[userID] = client;
            else
                _userIDRegister.TryAdd(userID, client);
        }

        public void UnregisterClient(int userid, string username)
        {
			_userIDRegister.TryRemove(userid, out GameClient Client);
			_usernameRegister.TryRemove(username.ToLower(), out Client);
        }

        public void CloseAll()
        {
            foreach (GameClient client in GetClients.ToList())
            {
                if (client == null)
                    continue;

                if (client.GetHabbo() != null)
                {
                    try
                    {
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.runFastQuery(client.GetHabbo().GetQueryString);
                        }
                        Console.Clear();
                        log.Info("<<- DESLIGANDO SERVIDOR ->> GUARDANDO INVENTARIOS");
                    }
                    catch
                    {
                    }
                }
            }

            log.Info("PRONTO!, inventários guardados!");
            log.Info("Fechando as conexões do servidor...");
            try
            {
                foreach (GameClient client in GetClients.ToList())
                {
                    if (client == null || client.GetConnection() == null)
                        continue;

                    try
                    {
                        GalaxyServer.GetConnectionManager().Destroy();
                        client.GetConnection().Dispose();
                    }
                    catch (Exception e)
                    {
                        ExceptionLogger.LogException(e);
                    }

                    Console.Clear();
                    log.Info("<<- APAGANDO SERVIDOR ->> Fechando conexões");

                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
            }

            if (_clients.Count > 0)
                _clients.Clear();

            log.Info("Conexões fechadas!");
        }

        private void TestClientConnections()
        {
            if (clientPingStopwatch.ElapsedMilliseconds >= 30000)
            {
                clientPingStopwatch.Restart();

                try
                {
                    List<GameClient> ToPing = new List<GameClient>();

                    foreach (GameClient client in _clients.Values.ToList())
                    {
                        if (client.PingCount < 6)
                        {
                            client.PingCount++;

                            ToPing.Add(client);
                        }
                        else
                        {
                            lock (timedOutConnections.SyncRoot)
                            {
                                timedOutConnections.Enqueue(client);
                            }
                        }
                    }

                    DateTime start = DateTime.Now;

                    foreach (GameClient Client in ToPing.ToList())
                    {
                        try
                        {
                            Client.SendMessage(new PongComposer());
                        }
                        catch
                        {
                            lock (timedOutConnections.SyncRoot)
                            {
                                timedOutConnections.Enqueue(Client);
                            }
                        }
                    }

                }
                catch
                {
                }
            }
        }

        private void HandleTimeouts()
        {
            if (timedOutConnections.Count > 0)
            {
                lock (timedOutConnections.SyncRoot)
                {
                    while (timedOutConnections.Count > 0)
                    {
                        GameClient client = null;

                        if (timedOutConnections.Count > 0)
                            client = (GameClient)timedOutConnections.Dequeue();

                        if (client != null)
                            client.Disconnect();
                    }
                }
            }
        }

        public int Count
        {
            get { return _clients.Count; }
        }

        public ICollection<GameClient> GetClients
        {
            get
            {
                return _clients.Values;
            }
        }
    }
}