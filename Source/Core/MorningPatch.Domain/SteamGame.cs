namespace MorningPatch.Domain;

/**
 * <summary>
 * An entity that represents a Steam game in the database.
 * </summary>
 */
public class SteamGame
{
	public required int AppId { get; set; }
	public required string Name { get; set; }
	public required string ImageIconHash { get; set; }
}