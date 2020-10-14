using Galaxy.Database.Interfaces;
using System;
using System.Data;

namespace Galaxy.Communication.Packets.Outgoing.Rooms.Nux
{
    class NuxItemListComposer : ServerPacket
    {
        public NuxItemListComposer() : base(ServerPacketHeader.NuxItemListComposer)
        {
            base.WriteInteger(1); // Número de páginas.

            base.WriteInteger(1); // ELEMENTO 1
            base.WriteInteger(3); // ELEMENTO 2
            base.WriteInteger(3); // Número total de premios:

            using (IQueryAdapter dbQuery = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbQuery.SetQuery("SELECT * FROM `nux_gifts` LIMIT 3");
                DataTable gUsersTable = dbQuery.getTable();

                foreach (DataRow Row in gUsersTable.Rows)
                {
                    base.WriteString(Convert.ToString(Row["image"]));
                    base.WriteInteger(1); 
                    base.WriteString(Convert.ToString(Row["title"]));
                    base.WriteString("");
                }
            }
        }
    }
}