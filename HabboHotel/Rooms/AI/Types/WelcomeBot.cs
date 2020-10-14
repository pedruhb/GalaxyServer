using Galaxy.Communication.Packets.Outgoing.Inventory.Furni;
using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Items;
using Galaxy.HabboHotel.Rooms;
using Galaxy.HabboHotel.Rooms.AI;
using System;
using System.Drawing;

namespace Galaxy.HabboHotel.Rewards.Rooms.AI.Types
{
    class WelcomeBot : BotAI
    {
        private int VirtualId;
        private int ActionTimer = 0;
        private int SpeechTimer = 0;
        int credits;
        int duckets;
        int diamonds;
        int furniID;
        int gotws;
        int hasSomething;

        public WelcomeBot(int VirtualId)
        {
            this.VirtualId = VirtualId;
            credits = Int32.Parse(GalaxyServer.GetGame().GetSettingsManager().TryGetValue("frank.give.credits"));
            duckets = Int32.Parse(GalaxyServer.GetGame().GetSettingsManager().TryGetValue("frank.give.duckets"));
            diamonds = Int32.Parse(GalaxyServer.GetGame().GetSettingsManager().TryGetValue("frank.give.diamonds"));
            furniID = Int32.Parse(GalaxyServer.GetGame().GetSettingsManager().TryGetValue("frank.give.furni"));
            gotws = Int32.Parse(GalaxyServer.GetGame().GetSettingsManager().TryGetValue("frank.give.gotws"));
            ActionTimer = 0;
            hasSomething = 0;
        }

        public override void OnSelfEnterRoom()
        {

        }

        public override void OnSelfLeaveRoom(bool Kicked)
        {
        }

        public override void OnUserEnterRoom(RoomUser User)
        {
        }

        public override void OnUserLeaveRoom(GameClient Client)
        {
            //Espaço em branco
        }

        public override void OnUserSay(RoomUser User, string Message)
        {

        }

        public override void OnUserShout(RoomUser User, string Message)
        {

        }

        public override void OnTimerTick()
        {
            if (GetBotData() == null)
                return;

            GameClient Target = GalaxyServer.GetGame().GetClientManager().GetClientByUsername(GetRoom().OwnerName);
            if (Target == null || Target.GetHabbo() == null || Target.GetHabbo().CurrentRoom != GetRoom())
            {
                GetRoom().GetGameMap().RemoveUserFromMap(GetRoomUser(), new Point(GetRoomUser().X, GetRoomUser().Y));
                GetRoom().GetRoomUserManager().RemoveBot(GetRoomUser().VirtualId, false);
                return;
            }


            if (ActionTimer <= 0)
            {
                switch (Target.GetHabbo().GetStats().WelcomeLevel)
                {
                    case 0:
                    default:
                        Point nextCoord;
                        RoomUser Target2 = GetRoom().GetRoomUserManager().GetRoomUserByHabbo(GetBotData().ForcedUserTargetMovement);
                        if (GetBotData().ForcedMovement)
                        {
                            if (GetRoomUser().Coordinate == GetBotData().TargetCoordinate)
                            {
                                GetBotData().ForcedMovement = false;
                                GetBotData().TargetCoordinate = new Point();

                                GetRoomUser().MoveTo(GetBotData().TargetCoordinate.X, GetBotData().TargetCoordinate.Y);
                            }
                        }
                        else if (GetBotData().ForcedUserTargetMovement > 0)
                        {

                            if (Target2 == null)
                            {
                                GetBotData().ForcedUserTargetMovement = 0;
                                GetRoomUser().ClearMovement(true);
                            }
                            else
                            {
                                var Sq = new Point(Target2.X, Target2.Y);

                                if (Target2.RotBody == 0)
                                {
                                    Sq.Y--;
                                }
                                else if (Target2.RotBody == 2)
                                {
                                    Sq.X++;
                                }
                                else if (Target2.RotBody == 4)
                                {
                                    Sq.Y++;
                                }
                                else if (Target2.RotBody == 6)
                                {
                                    Sq.X--;
                                }


                                GetRoomUser().MoveTo(Sq);
                            }
                        }
                        else if (GetBotData().TargetUser == 0)
                        {
                            nextCoord = GetRoom().GetGameMap().getRandomWalkableSquare();
                            GetRoomUser().MoveTo(nextCoord.X, nextCoord.Y);
                        }
                        Target.GetHabbo().GetStats().WelcomeLevel++;
                        break;
                    //Aqui é a as fala do bot frank
                    case 1:
                        GetRoomUser().Chat("Olá " + GetRoom().OwnerName + "! Seja bem vindo ao "+GalaxyServer.HotelName+"!", false, 33);
                        Target.GetHabbo().GetStats().WelcomeLevel++;
                        break;
                    case 2:
                        if (credits != 0 && diamonds != 0 && duckets != 0 && gotws != 0)
                        {
                            GetRoomUser().Chat("Vou te dar: " + credits + " créditos, " + diamonds + " diamantes, " + duckets + " duckets é " + gotws + " parafusos!", false, 33);
                            Target.GetHabbo().Credits += credits;
                            Target.GetHabbo().Diamonds += diamonds;
                            Target.GetHabbo().Duckets += duckets;
                            Target.GetHabbo().GOTWPoints += gotws;
                            Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));
                            Target.SendMessage(new ActivityPointsComposer(Target.GetHabbo().Duckets, Target.GetHabbo().Diamonds, Target.GetHabbo().GOTWPoints));
                            hasSomething = 1;
                        }
                        else if (credits != 0 && diamonds != 0 && duckets != 0)
                        {
                            GetRoomUser().Chat("Vou te dar: " + credits + " créditos, " + diamonds + " diamantes é " + duckets + " duckets!", false, 33);
                            Target.GetHabbo().Credits += credits;
                            Target.GetHabbo().Diamonds += diamonds;
                            Target.GetHabbo().Duckets += duckets;
                            Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));
                            Target.SendMessage(new ActivityPointsComposer(Target.GetHabbo().Duckets, Target.GetHabbo().Diamonds, Target.GetHabbo().GOTWPoints));
                            hasSomething = 1;
                        }
                        else if (credits != 0 && diamonds != 0)
                        {
                            GetRoomUser().Chat("Vou te dar: " + credits + " créditos é " + diamonds + " diamantes!", false, 33);
                            Target.GetHabbo().Credits += credits;
                            Target.GetHabbo().Diamonds += diamonds;
                            Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));
                            Target.SendMessage(new ActivityPointsComposer(Target.GetHabbo().Duckets, Target.GetHabbo().Diamonds, Target.GetHabbo().GOTWPoints));
                            hasSomething = 1;
                        }
                        else if (credits != 0)
                        {
                            GetRoomUser().Chat("Como um presente de boas vindas ao hotel, irei te dar " + credits + " créditos!", false, 33);
                            Target.GetHabbo().Credits += credits;
                            Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));
                            hasSomething = 1;
                        }
          
                        if (Target.GetHabbo().isNoob)
                        {
                        }
                        Target.GetHabbo().GetStats().WelcomeLevel++;
                        break;
                    case 3:
                        if (furniID != 0)
                        {
                            hasSomething = 1;
                            GetRoomUser().Chat("Também te dei um mobi!", false, 33);
                            //Aqui o bot vai da o mobi pro usuário

                            ItemData furni = null;
                            if (GalaxyServer.GetGame().GetItemManager().GetItem(furniID, out furni))
                            {
                                Item purchasefurni = ItemFactory.CreateSingleItemNullable(furni, Target.GetHabbo(), "", "");
                                if (purchasefurni != null)
                                {
                                    Target.GetHabbo().GetInventoryComponent().TryAddItem(purchasefurni);
                                    Target.SendMessage(new FurniListNotificationComposer(purchasefurni.Id, 1));
                                    Target.SendMessage(new FurniListUpdateComposer());
                                }
                            }
                        }
                        if (hasSomething == 0)
                        {
                            GetRoomUser().Chat("Hoje eu tenho algo para lhe dar!", false, 33);

                        }
                        Target.GetHabbo().GetStats().WelcomeLevel++;
                        break;
                    case 4:
                        GetRoomUser().Chat("Estou indo nessa, qualquer dúvida, procure um Staff!", false, 33);
                        GetRoomUser().Chat("Até logo!", false, 33);
                        Target.GetHabbo().GetStats().WelcomeLevel++;
                        break;
                    case 5:
                        GetRoom().GetGameMap().RemoveUserFromMap(GetRoomUser(), new Point(GetRoomUser().X, GetRoomUser().Y));
                        GetRoom().GetRoomUserManager().RemoveBot(GetRoomUser().VirtualId, false);
                        Target.GetHabbo().GetStats().WelcomeLevel++;
                        break;
                }
                ActionTimer = new Random(DateTime.Now.Millisecond + this.VirtualId ^ 2).Next(5, 15);
            }
            else
                ActionTimer--;
        }
    }
}