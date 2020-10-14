using System;
using System.Data;
using System.Collections.Generic;
using Galaxy.Database.Interfaces;

namespace Galaxy.HabboHotel.Rooms.Chat.Pets.Commands
{
    public class PetCommandManager
    {
        private Dictionary<int, string> _commandRegister;
        private Dictionary<string, string> _commandDatabase;
        private Dictionary<string, PetCommand> _petCommands;

        public PetCommandManager()
        {
            this._petCommands = new Dictionary<string, PetCommand>();
            this._commandRegister = new Dictionary<int, string>();
            this._commandDatabase = new Dictionary<string, string>();

            this.Init();
        }

        public void Init()
        {
            this._petCommands.Clear();
            this._commandRegister.Clear();
            this._commandDatabase.Clear();

            DataTable Table = null;
            using (IQueryAdapter dbClient = GalaxyServer.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `bots_pet_commands`");
                Table = dbClient.getTable();

                if (Table != null)
                {
                    foreach (DataRow row in Table.Rows)
                    {
                        _commandRegister.Add(Convert.ToInt32(row[0]), row[1].ToString());
                        _commandDatabase.Add(row[1] + ".input", row[2].ToString());
                    }
                }
            }

            foreach (var pair in _commandRegister)
            {
                int commandID = pair.Key;
                string commandStringedID = pair.Value;
                string[] commandInput = this._commandDatabase[commandStringedID + ".input"].Split(',');

                foreach (string command in commandInput)
                {
                    this._petCommands.Add(command, new PetCommand(commandID, command));
                }
            }
        }

        public int TryInvoke(string Input)
        {
            PetCommand Command = null;
            if (this._petCommands.TryGetValue(Input.ToLower(), out Command))
                return Command.Id;
            return 0;
        }

        private static Int32 ConvertCommandToInt32(string Command)
        {
            switch (Command)
            {
                case "DESCANSA":
                    return 0;
                case "HABLA":
                    return 10;
                case "JUEGA":
                    return 11;
                case "CALLA":
                    return 12;
                case "A CASA":
                    return 13;
                case "BEBE":
                    return 14;
                case "IZQUIERDA":
                    return 15;
                case "DERECHA":
                    return 16;
                case "FÚTBOL":
                    return 17;
                case "ARRODÍLLATE":
                    return 18;
                case "BOTA":
                    return 19;
                case "SIÉNTATE":
                    return 1;
                case "ESTATUA":
                    return 20;
                case "BAILA":
                    return 21;
                case "GIRA":
                    return 22;
                case "ENCIENDE TV":
                    return 23;
                case "ADELANTE":
                    return 24;
                //case "IZQUIERDA":
                //    return 25;
                //case "DERECHA":
                //    return 26;
                case "RELAX":
                    return 27;
                case "CROA":
                    return 28;
                case "INMERSIÓN":
                    return 29;
                case "TÚMBATE":
                    return 2;
                case "SALUDA":
                    return 30;
                case "MARCHA":
                    return 31;
                case "GRAN SALTO":
                    return 32;
                case "BAILE POLLO":
                    return 33;
                case "TRIPLE SALTO":
                    return 34;
                case "MUESTRA ALAS":
                    return 35;
                case "ECHA FUEGO":
                    return 36;
                case "PLANEA":
                    return 37;
                case "ANTORCHA":
                    return 38;
                case "VEN AQUí":
                    return 3;
                case "CAMBIA VUELO":
                    return 40;
                case "VOLTERETA":
                    return 41;
                case "ANILLO FUEGO":
                    return 42;
                case "COMER":
                    return 43;
                case "MOVER COLA":
                    return 44;
                case "Cuenta":
                    return 45;
                case "Cruzar":
                    return 46;
                case "PIDE":
                    return 4;
                case "HAZ EL MUERTO":
                    return 5;
                case "QUIETO":
                    return 6;
                case "SÍGUEME":
                    return 7;
                case "LEVANTA":
                    return 8;
                case "SALTA":
                    return 9;
                case "AQUI":
                    return 2;
            }
            return 0;
        }
    }
}