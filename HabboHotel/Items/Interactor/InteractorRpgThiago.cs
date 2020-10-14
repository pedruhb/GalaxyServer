using Galaxy.Communication.Packets.Outgoing.Rooms.Chat;
using Galaxy.HabboHotel.Rooms;
using System;

namespace Galaxy.HabboHotel.Items.Interactor
{
    class InteractorRpgThiago : IFurniInteractor
    {
        public void OnPlace(GameClients.GameClient Session, Item Item)
        {
            Item.ExtraData = "0";
            Item.UpdateState(true, true);
        }

        public void OnRemove(GameClients.GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClients.GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (Session == null || Session.GetHabbo() == null || Item == null)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (Actor == null)
                return;

            var tick = int.Parse(Item.ExtraData);

            if (tick < 23)
            {
                if (Actor.CurrentEffect == 27)
                {
                    if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) < 2)
                    {
                        RoomUser ThisUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
                        Random random = new Random();
                        tick++;
                        Item.ExtraData = tick.ToString();
                        Item.UpdateState(true, true);
                        int X = Item.GetX, Y = Item.GetY, Rot = Item.Rotation;
                        Double Z = Item.GetZ;
                        int randomNumber = random.Next(1, 9);
                        if (randomNumber == 1)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Contra ataque - Estilo Terra chão de água roxa- no(a) [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                            Session.GetHabbo().Effects().ApplyEffect(185);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Estou preso em um chão de água roxa*", 0, 6));
                            System.Threading.Thread.Sleep(3000);
                            Session.GetHabbo().Effects().ApplyEffect(27);
                        }
                        if (randomNumber == 2)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Contra ataque - Estilo Vacuo explosão de bombas- no(a) [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                            Session.GetHabbo().Effects().ApplyEffect(108);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Fui atingido por uma bomba*", 0, 6));
                            System.Threading.Thread.Sleep(3000);
                            Session.GetHabbo().Effects().ApplyEffect(27);
                        }
                        if (randomNumber == 3)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Contra ataque - Estilo Cura sugador de vida- no(a) [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                            Session.GetHabbo().Effects().ApplyEffect(23);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Minha vida esta sendo sugada!", 0, 6));
                            System.Threading.Thread.Sleep(3000);
                            Session.GetHabbo().Effects().ApplyEffect(27);
                            tick--;
                            tick--;
                            Item.ExtraData = tick.ToString();
                            Item.UpdateState(true, true);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Peguei 50% da vida do [" + ThisUser.GetUsername().ToString() + "] para me curar 2%", 0, 34));
                        }
                        if (randomNumber == 4)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Essa doeu [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                        if (randomNumber == 5)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Até que você não é ruim! [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                        if (randomNumber == 6)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Você ate que é forte, ate me lembra um antiga pessoa. [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                        if (randomNumber == 7)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Como ousar em me enfrentar? [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                        if (randomNumber == 8)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Até que você não é ruim! [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                        }
                        if (randomNumber == 9)
                        {
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Contra ataque -Estilo Cura sugador de vida- no(a) [" + ThisUser.GetUsername().ToString() + "]", 0, 34));
                            Session.GetHabbo().Effects().ApplyEffect(23);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "*Minha vida esta sendo sugada!", 0, 6));
                            System.Threading.Thread.Sleep(3000);
                            Session.GetHabbo().Effects().ApplyEffect(27);
                            tick--;
                            tick--;
                            tick--;
                            tick--;
                            tick--;
                            tick--;
                            tick--;
                            Item.ExtraData = tick.ToString();
                            Item.UpdateState(true, true);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Peguei 80% da vida do [" + ThisUser.GetUsername().ToString() + "] para me curar 10%", 0, 34));
                        }

                        if (tick == 19)
                        {
                            GalaxyServer.GetGame().GetPinataManager().ReceiveCrackableReward(Actor, Room, Item);
                            Room.SendMessage(new ChatComposer(ThisUser.VirtualId, "[Boss] " + "Fui derrotado por um simples usuário! Você ganhou " + ThisUser.GetUsername().ToString() + "!", 0, 34));
                        }
                    }
                }
                else
                {
                    Session.SendWhisper("Ops, você não esta com os equipamentos de luta! digite o comando :efeito 27");
                    return;
                }
            }
        }

        public void OnWiredTrigger(Item Item)
        {

        }
    }
}
