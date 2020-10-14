namespace Galaxy.Communication.RCON.Commands.Hotel
{
	class UpdateFrontPage : IRCONCommand
	{
		public string Description => "Atualizar frontpage";
		public string Parameters => "";

		public bool TryExecute(string[] parameters)
		{
			Core.CatalogSettings.RunCatalogSettings();
			return true;
		}
	}
}