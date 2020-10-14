namespace Galaxy.Communication.Packets.Outgoing.Catalog
{
	class CatalogItemDiscountComposer : ServerPacket
    {
        public CatalogItemDiscountComposer()
            : base(ServerPacketHeader.CatalogItemDiscountMessageComposer)
        {
			WriteInteger(100);//Most you can get.
            WriteInteger(0); ///WriteInteger(6);
			WriteInteger(1);
			WriteInteger(1);
            WriteInteger(0);//Count // WriteInteger(2);//Count
            {
				WriteInteger(40);
				WriteInteger(99);
            }
        }
    }
}