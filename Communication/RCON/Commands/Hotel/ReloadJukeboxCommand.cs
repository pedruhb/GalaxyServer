using Galaxy.HabboHotel.Rooms.TraxMachine;

namespace Galaxy.Communication.RCON.Commands.Hotel
{
    class ReloadJukeboxCommand : IRCONCommand
    {
        public string Description => "Atualizar a Jukebox";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            TraxSoundManager.Init();
            return true;
        }
    }
}