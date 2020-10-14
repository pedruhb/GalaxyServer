using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.Core;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms;
using Galaxy.Utilities;
using System.Threading;

namespace Galaxy.HabboHotel.Items.Interactor
{
    class InteractorSlotmachine : IFurniInteractor
    {
        string Rand1;
        string Rand2;
        string Rand3;

        public void OnPlace(GameClient Session, Item Item)
        {
            Item.ExtraData = "0";
        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (Actor == null)
                return;

            if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) < 2)
            {
                if (Session.GetHabbo().Diamonds <= 0)
            {
                Session.SendWhisper("Para apostar você deve ter diamantes, atualmente você tem 0.", 34);
                return;
            }

            if (Session.GetHabbo()._bet > Session.GetHabbo().Diamonds)
            {
                Session.SendWhisper("Você está apostando mais "+ExtraSettings.NomeDiamantes.ToLower()+" do que tem!", 34);
                return;
            }

            if (Session.GetHabbo()._bet <= 0)
            {
                Session.SendWhisper("Opa, você não pode apostar 0 diamantes! Para saber como funciona, use o comando :apostar info", 34);
                return;
            }

            if (Session == null || Session.GetHabbo() == null || Item == null)
                return;
           
                int Bet = Session.GetHabbo()._bet;
                Session.SendWhisper("Inserindo "+Bet+ " " + ExtraSettings.NomeDiamantes.ToLower() +" na máquina...", 34);

                Thread.Sleep(1500);

                Session.GetHabbo().Diamonds -= Bet;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, -Bet, 5));

                int Random1 = RandomNumber.GenerateRandom(1, 3);
                switch (Random1)
                {
                    case 1:
                        Rand1 = "¥";
                        break;
                    case 2:
                        Rand1 = "|";
                        break;
                    case 3:
                        Rand1 = "ª";
                        break;
                }

                int Random2 = RandomNumber.GenerateRandom(1, 3);
                switch (Random2)
                {
                    case 1:
                        Rand2 = "¥";
                        break;
                    case 2:
                        Rand2 = "|";
                        break;
                    case 3:
                        Rand2 = "ª";
                        break;
                }

                int Random3 = RandomNumber.GenerateRandom(1, 3);
                switch (Random3)
                {
                    case 1:
                        Rand3 = "¥";
                        break;
                    case 2:
                        Rand3 = "|";
                        break;
                    case 3:
                        Rand3 = "ª";
                        break;
                }

                Session.SendWhisper("[ " + Rand1 + " - " + Rand2 + " - " + Rand3 + " ]", 34);
                Item.ExtraData = "1";
                Item.UpdateState(true, true);

                new Thread(() =>
                {
                    Thread.Sleep(1000);
                    Item.ExtraData = "0";
                    Item.UpdateState(true, true);
                }).Start();

                if (Random1 == Random2 && Random1 == Random3 && Random3 == Random2)
                {
                    switch (Random1)
                    {
                        case 1:
                            Session.GetHabbo().Diamonds += Bet * 4;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, -Bet, 5));
                            Session.SendWhisper("Parabéns, você ganhou " + Bet * 4 + " " + ExtraSettings.NomeDiamantes.ToLower() + " com três estrelas!", 34);
                           
                            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("INSERT INTO `logs_slotmachine` (`apostador`, `qntapostada`, `qntganha`, `timestamp`) VALUES ('"+Session.GetHabbo().Id+"', '" + Bet + "', '" + Bet * 4 + "', '"+GalaxyServer.GetUnixTimestamp()+"');");
                                // Adiciona log ao banco de dados.
                            }
                     
                            Session.GetHabbo()._bet = 0;
                                break;
                        case 2:
                            Session.GetHabbo().Diamonds += Bet * 3;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, -Bet, 5));
                            Session.SendWhisper("Parabéns, você ganhou " + Bet * 3 + " " + ExtraSettings.NomeDiamantes.ToLower() + " com três corações!", 34);

                            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("INSERT INTO `logs_slotmachine` (`apostador`, `qntapostada`, `qntganha`, `timestamp`) VALUES ('" + Session.GetHabbo().Id + "', '" + Bet + "', '" + Bet * 4 + "', '" + GalaxyServer.GetUnixTimestamp() + "');");
                                // Adiciona log ao banco de dados.
                            }

                            Session.GetHabbo()._bet = 0;
                            break;
                        case 3:
                            Session.GetHabbo().Diamonds += Bet * 2;
                            Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, -Bet, 5));
                            Session.SendWhisper("Parabéns, você ganhou " + Bet * 2 + " " + ExtraSettings.NomeDiamantes.ToLower() + " com três caveiras!", 34);
                            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("INSERT INTO `logs_slotmachine` (`apostador`, `qntapostada`, `qntganha`, `timestamp`) VALUES ('" + Session.GetHabbo().Id + "', '" + Bet + "', '" + Bet * 2 + "', '" + GalaxyServer.GetUnixTimestamp() + "');");
                                // Adiciona log ao banco de dados.
                            }
                            Session.GetHabbo()._bet = 0;
                            break;
                    }
                } else
                {
                    Session.SendWhisper("Infelizmente você perdeu :(", 34);
                    Session.GetHabbo()._bet = 0;
                }
                return;
            }else {
                Session.SendWhisper("Chegue mais perto da máquina para apostar!");
        }
        } 

        public void OnWiredTrigger(Item Item)
        {

        }
    }
}
