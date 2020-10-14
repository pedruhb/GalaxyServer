using Galaxy.Communication.Packets.Outgoing.Notifications;
using Galaxy.Communication.Packets.Outgoing.Rooms.Engine;
using System.Text;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class PetCommand : IChatCommand
    {
        public string PermissionRequired => "command_pet";
        public string Parameters => "";
        public string Description => "Se transforma em um PET.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            

            RoomUser RoomUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (RoomUser == null)
                return;

            if (!Room.PetMorphsAllowed)
            {
                Session.SendWhisper("O proprietário desabilitou o comando desse quarto!");
                if (Session.GetHabbo().PetId > 0)
                {
                    Session.SendWhisper("Você ainda tem uma tranformação");
                    Session.GetHabbo().PetId = 0;

                    Room.SendMessage(new UserRemoveComposer(RoomUser.VirtualId));
                    Room.SendMessage(new UsersComposer(RoomUser));
                }
                return;
            }

            if (Params.Length == 1)
            {
                StringBuilder List = new StringBuilder("");
                Session.SendMessage(new MOTDNotificationComposer("Lista de Mascotes no " + GalaxyServer.HotelName + "\n" +
              
                "habbo, bob, slenderman, pato, velociraptor, pterodactilo," +
                " pedra, planta, lobo, elefante, haloompa, urso, gato, pintinho," +
                " crocodilo, cachorro, sapo, leao, cavalo, macaco, rinoceronte, " +
                "terrier, aranha, tartaruga, vaca, mario, luigi, jirachi, victini," +
                " tagepi, entei, ash, ectoplasma, beautifly, celebi, eevee, pikachu, pichu, " +
                "filhotegato, filhotecachorro, bebe"));
                return;
            }

            int TargetPetId = GetPetIdByString(Params[1].ToString());
            if (TargetPetId == 0)
            {
                Session.SendWhisper("Ops! Não existe um pet com esse nome!");
                return;
            }

            //Change the users Pet Id.
            Session.GetHabbo().PetId = (TargetPetId == -1 ? 0 : TargetPetId);

            //Quickly remove the old user instance.
            Room.SendMessage(new UserRemoveComposer(RoomUser.VirtualId));

            //Add the new one, they won't even notice a thing!!11 8-)
            Room.SendMessage(new UsersComposer(RoomUser));

            //Tell them a quick message.
            if (Session.GetHabbo().PetId > 0)
                Session.SendWhisper("Usar ':pet habbo' para voltar ao normal!");
        }

        private int GetPetIdByString(string Pet)
        {
            switch (Pet.ToLower())
            {
                default:
                    return 0;
                case "habbo":
                    return -1;
                case "perro":
                    return 60;//This should be 0.
                case "bob":
                    return 37;
                case "slenderman":
                    return 46;
                case "pato":
                    return 61;
                case "velociraptor":
                    return 34;
                case "pterodactilo":
                    return 33;
                case "pedra":
                    return 39;
                case "planta":
                    return 38;
                case "lobo":
                    return 57;
                case "elefante":
                    return 40;
                case "haloompa":
                    return 41;
                case "urso":
                    return 4;
                case "gato":
                    return 1;
                case "pintinho":
                    return 10;
                case "crocodilo":
                    return 3;
                case "cachorro":
                    return 0;
                case "sapo":
                    return 11;
                case "leao":
                    return 6;
                case "cavalo":
                    return 15;
                case "macaco":
                    return 14;
                case "rinoceronte":
                    return 7;
                case "terrier":
                    return 2;
                case "aranha":
                    return 8;
                case "tartaruga":
                    return 9;
                case "vaca":
                    return 600;
                case "mario":
                    return 42;
                case "luigi":
                    return 44;
                case "jirachi":
                    return 52;
                case "victini":
                    return 56;
                case "tagepi":
                    return 55;
                case "entei":
                    return 53;
                case "ash":
                    return 47;
                case "ectoplasma":
                    return 51;
                case "beautifly":
                    return 48;
                case "celebi":
                    return 49;
                case "eevee":
                    return 45;
                case "pikachu":
                    return 43;
                case "pichu":
                    return 54;
                case "filhotegato":
                    return 28;
                case "filhotecachorro":
                    return 29;
                case "bebe":
                case "nenem":
                    return 36;

            }
        }
    }
}