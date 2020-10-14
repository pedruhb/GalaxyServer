using Galaxy.Communication.Packets.Outgoing.Inventory.Purse;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms.AI.Speech;
using System;
using System.Drawing;

namespace Galaxy.HabboHotel.Rooms.AI.Types
{
    class SayBot : BotAI
    {
        private int VirtualId;
        private static readonly Random Random = new Random();
        private int ActionTimer = 0;
        private int SpeechTimer = 0;

        public SayBot(int VirtualId)
        {
            this.VirtualId = VirtualId;
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
        }

        public override void OnUserSay(RoomUser User, string Message)
        {
            if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                return;

            if (GetBotData() == null)
                return;

            if (Gamemap.TileDistance(GetRoomUser().X, GetRoomUser().Y, User.X, User.Y) > 8)
                return;

            string Command = Message.Substring(GetBotData().Name.ToLower().Length + 1);

            if ((Message.ToLower().StartsWith(GetBotData().Name.ToLower() + " ")))
            {
                switch (Command)
                {
                    case "vem":
                    case "comehere":
                    case "come here":
                    case "vem aqui":
                    case "come":
                        GetRoomUser().Chat("Eu vou!", false, 0);
                        GetRoomUser().MoveTo(User.SquareInFront);
                        break;

                    case "sirve":
                    case "serve":
                        if (GetRoom().CheckRights(User.GetClient()))
                        {
                            foreach (var current in GetRoom().GetRoomUserManager().GetRoomUsers()) current.CarryItem(Random.Next(1, 38));
                            GetRoomUser().Chat("Ok. Você já tem algo para o zampar.", false, 0);
                            return;
                        }
                        break;
                    case "agua":
                    case "té":
                    case "te":
                    case "tea":
                    case "juice":
                    case "water":
                    case "zumo":
                        GetRoomUser().Chat("Aqui tens.", false, 0);
                        User.CarryItem(Random.Next(1, 3));
                        break;

                    case "helado":
                    case "icecream":
                    case "sorvete":
                    case "ice cream":
                        GetRoomUser().Chat("Aqui tens. Não fique na língua, heh!", false, 0);
                        User.CarryItem(4);
                        break;

                    case "rose":
                    case "rosa":
                        GetRoomUser().Chat("Aqui ... tenha um bom tempo em sua data.", false, 0);
                        User.CarryItem(Random.Next(1000, 1002));
                        break;

                    case "girasol":
                    case "sunflower":
                        GetRoomUser().Chat("Aqui você tem algo muito bonito da natureza.", false, 0);
                        User.CarryItem(1002);
                        break;

                    case "flor":
                    case "flower":
                        GetRoomUser().Chat("Aqui você tem algo muito bonito da natureza.", false, 0);
                        if (Random.Next(1, 3) == 2)
                        {
                            User.CarryItem(Random.Next(1019, 1024));
                            return;
                        }
                        User.CarryItem(Random.Next(1006, 1010));
                        break;

                    case "zanahoria":
                    case "zana":
                    case "carrot":
                        GetRoomUser().Chat("Aqui está um bom vegetal. Eu me abençoo!", false, 0);
                        User.CarryItem(3);
                        break;

                    case "café":
                    case "cafe":
                    case "capuccino":
                    case "coffee":
                    case "latte":
                    case "mocha":
                    case "espresso":
                    case "expreso":
                        GetRoomUser().Chat("Aqui está o seu café. Está espumante!", false, 0);
                        User.CarryItem(Random.Next(11, 18));
                        break;

                    case "fruta":
                    case "fruit":
                        GetRoomUser().Chat("Aqui você tem algo saudável, fresco e natural. Aproveite!", false, 0);
                        User.CarryItem(Random.Next(36, 40));
                        break;

                    case "naranja":
                    case "orange":
                    case "laranja":
                        GetRoomUser().Chat("Aqui você tem algo saudável, fresco e natural. Aproveite!", false, 0);
                        User.CarryItem(38);
                        break;

                    case "manzana":
                    case "apple":
                    case "maça":
                        GetRoomUser().Chat("Aqui você tem algo saudável, fresco e natural. Aproveite!", false, 0);
                        User.CarryItem(37);
                        break;

                    case "cola":
                    case "habbocola":
                    case "cocacola":
                    case "coca cola":
                    case "coca-cola":
                    case "habbo cola":
                    case "habbo-cola":
                        GetRoomUser().Chat("Aqui você tem um refresco muito famoso.", false, 0);
                        User.CarryItem(19);
                        break;

                    case "pear":
                    case "pera":
                        GetRoomUser().Chat("Aqui você tem algo saudável, fresco e natural. Aproveite!", false, 0);
                        User.CarryItem(36);
                        break;

                    case "ananá":
                    case "pineapple":
                    case "piña":
                    case "rodaja de piña":
                        GetRoomUser().Chat("Aqui você tem algo saudável, fresco e natural. Aproveite!", false, 0);
                        User.CarryItem(39);
                        break;

                    case "puta":
                    case "puto":
                    case "gilipollas":
                    case "metemela":
                    case "polla":
                    case "pene":
                    case "penis":
                    case "idiot":
                    case "fuck":
                    case "bastardo":
                    case "idiota":
                    case "chupamela":
                    case "tonta":
                    case "tonto":
                    case "mierda":
                        GetRoomUser().Chat("Não me trate assim, hey!", true, 0);
                        break;

                    case "casa comigo":
                        GetRoomUser().Chat("Irei agora!", true, 0);
                        break;

                    case "protocolo destruir":
                        GetRoomUser().Chat("Iniciando Auto Destruição do Mundo", true, 0);
                        break;

                    case "lindo":
                    case "hermoso":
                    case "linda":
                    case "guapa":
                    case "beautiful":
                    case "handsome":
                    case "love":
                    case "guapo":
                    case "i love you":
                    case "hermosa":
                    case "preciosa":
                    case "teamo":
                    case "amor":
                    case "miamor":
                    case "mi amor":
                        GetRoomUser().Chat("Eu sou um bot, err ... isso está ficando desconfortável, você sabe?", false, 0);
                        break;
                    case "chupala":
                    case "boquete":
                        if (User.GetClient().GetHabbo().Credits < 500)
                        {
                            GetRoomUser().Chat("Opps você não tem créditos suficientes para receber o seu...você precisa de 500 $", false, 0);
                            return;
                        }
                        GetRoomUser().Chat("* Sugue " + User.GetClient().GetHabbo().Username + " *", false, 0);
                        User.Chat("* Oh Torne-se um Tradutor! ohh seh!! *", false, 0);
                        User.GetClient().GetHabbo().Credits = User.GetClient().GetHabbo().Credits - 500;
                        User.GetClient().SendMessage(new CreditBalanceComposer(User.GetClient().GetHabbo().Credits));
                        break;
                    case "pajilla":
                    case "sexo":
                        if (User.GetClient().GetHabbo().Credits < 300)
                        {
                            GetRoomUser().Chat("Opps você não tem créditos suficientes para receber o seu ... você precisa de 300$", false, 0);
                            return;
                        }
                        GetRoomUser().Chat("* Fazendo sexo com " + User.GetClient().GetHabbo().Username + " *", false, 0);
                        User.Chat("* Oh seh baby! ohh seh!! *", false, 0);
                        User.GetClient().GetHabbo().Credits = User.GetClient().GetHabbo().Credits - 300;
                        User.GetClient().SendMessage(new CreditBalanceComposer(User.GetClient().GetHabbo().Credits));
                        break;
                }
            }
        }

        public override void OnUserShout(RoomUser User, string Message)
        {

        }


        public override void OnTimerTick()
        {
            if (GetBotData() == null)
                return;

            if (SpeechTimer <= 0)
            {
                if (GetBotData().RandomSpeech.Count > 0)
                {
                    if (GetBotData().AutomaticChat == false)
                        return;

                    RandomSpeech Speech = GetBotData().GetRandomSpeech();

					string String = GalaxyServer.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Speech.Message, out string word) ? "Spam" : Speech.Message;
					if (String.Contains("<img src") || String.Contains("<font ") || String.Contains("</font>") || String.Contains("</a>") || String.Contains("<i>"))
                        String = "Eu realmente não deveria usar o HTML dentro de discursos de bot.";
                    GetRoomUser().Chat(String, false, GetBotData().ChatBubble); 
                }
                SpeechTimer = GetBotData().SpeakingInterval;
            }
            else
                SpeechTimer--;

            if (ActionTimer <= 0)
            {
                Point nextCoord;
                switch (GetBotData().WalkingMode.ToLower())
                {
                    default:
                    case "stand":
                        // (8) Why is my life so boring?
                        break;

                    case "freeroam":
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
                            RoomUser Target = GetRoom().GetRoomUserManager().GetRoomUserByHabbo(GetBotData().ForcedUserTargetMovement);
                            if (Target == null)
                            {
                                GetBotData().ForcedUserTargetMovement = 0;
                                GetRoomUser().ClearMovement(true);
                            }
                            else
                            {
                                var Sq = new Point(Target.X, Target.Y);

                                if (Target.RotBody == 0)
                                {
                                    Sq.Y--;
                                }
                                else if (Target.RotBody == 2)
                                {
                                    Sq.X++;
                                }
                                else if (Target.RotBody == 4)
                                {
                                    Sq.Y++;
                                }
                                else if (Target.RotBody == 6)
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
                        break;

                    case "specified_range":

                        break;
                }

                ActionTimer = new Random(DateTime.Now.Millisecond + this.VirtualId ^ 2).Next(5, 15);
            }
            else
                ActionTimer--;
        }
    }
}