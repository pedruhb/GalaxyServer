namespace Galaxy.Communication.Packets.Outgoing.Pets
{
	class PetBreedingComposer : ServerPacket
    {
        public PetBreedingComposer()
            : base(ServerPacketHeader.PetBreedingMessageComposer)
        {
			WriteInteger(219005779);//An Id?
            {

				WriteInteger(4);//Count
                {
					WriteInteger(1);
					WriteInteger(3);
					WriteInteger(18);
					WriteInteger(19);
					WriteInteger(20);
					WriteInteger(3);
					WriteInteger(6);
					WriteInteger(12);
					WriteInteger(13);
					WriteInteger(14);
					WriteInteger(15);
					WriteInteger(16);
					WriteInteger(17);
					WriteInteger(4);
					WriteInteger(5);
					WriteInteger(7);
					WriteInteger(8);
					WriteInteger(9);
					WriteInteger(10);
					WriteInteger(11);
					WriteInteger(92);
					WriteInteger(6);
					WriteInteger(1);
					WriteInteger(2);
					WriteInteger(3);
					WriteInteger(4);
					WriteInteger(5);
					WriteInteger(6);
                }
                WriteInteger(28);
                WriteInteger(15);
            }
        }
    }
}