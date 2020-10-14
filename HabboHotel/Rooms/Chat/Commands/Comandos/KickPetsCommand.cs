using Galaxy.Communication.Packets.Outgoing.Inventory.Pets;
using Galaxy.Database.Interfaces;
using Galaxy.HabboHotel.GameClients;
using Galaxy.HabboHotel.Rooms.AI;
using System.Drawing;
using System.Linq;

namespace Galaxy.HabboHotel.Rooms.Chat.Commands.Comandos
{
    class KickPetsCommand : IChatCommand
    {
        public string PermissionRequired => "command_kickpets";
        public string Parameters => "";
        public string Description => "Expulse todos os pets da sala!";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {


            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Bem, somente o proprietário da sala pode executar esse comando!");
                return;
            }

            if (Room.GetRoomUserManager().GetPets().Count > 0)
            {
                foreach (RoomUser Pet in Room.GetRoomUserManager().GetUserList().ToList())
                {
                    if (Pet == null)
                        continue;

                    if (Pet.RidingHorse)
                    {
                        RoomUser UserRiding = Room.GetRoomUserManager().GetRoomUserByVirtualId(Pet.HorseID);
                        if (UserRiding != null)
                        {
                            UserRiding.RidingHorse = false;
                            UserRiding.ApplyEffect(-1);
                            UserRiding.MoveTo(new Point(UserRiding.X + 1, UserRiding.Y + 1));
                        }
                        else
                            Pet.RidingHorse = false;
                    }

                    Pet.PetData.RoomId = 0;
                    Pet.PetData.PlacedInRoom = false;

                    Pet pet = Pet.PetData;
                    if (pet != null)
                    {
                        using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.runFastQuery("UPDATE `bots` SET `room_id` = '0', `x` = '0', `Y` = '0', `Z` = '0' WHERE `id` = '" + pet.PetId + "' LIMIT 1");
                            dbClient.runFastQuery("UPDATE `bots_petdata` SET `experience` = '" + pet.experience + "', `energy` = '" + pet.Energy + "', `nutrition` = '" + pet.Nutrition + "', `respect` = '" + pet.Respect + "' WHERE `id` = '" + pet.PetId + "' LIMIT 1");
                        }
                    }

                    if (pet.OwnerId != Session.GetHabbo().Id)
                    {
                        GameClient Target = GalaxyServer.GetGame().GetClientManager().GetClientByUserID(pet.OwnerId);
                        if (Target != null)
                        {
                            Target.GetHabbo().GetInventoryComponent().TryAddPet(Pet.PetData);
                            Room.GetRoomUserManager().RemoveBot(Pet.VirtualId, false);

                            Target.SendMessage(new PetInventoryComposer(Target.GetHabbo().GetInventoryComponent().GetPets()));
                            return;
                        }
                    }

                    Session.GetHabbo().GetInventoryComponent().TryAddPet(Pet.PetData);
                    Room.GetRoomUserManager().RemoveBot(Pet.VirtualId, false);
                    Session.SendMessage(new PetInventoryComposer(Session.GetHabbo().GetInventoryComponent().GetPets()));
                }
                Session.SendWhisper("Sucesso, você retirou todos os animais de estimação.");
            }
            else
            {
                Session.SendWhisper("Bem, não há nenhum animal de estimação aqui!?");
            }
        }
    }
}
