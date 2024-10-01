namespace MorningPatch.Domain.Models;
/**
 * <summary>
 * A model that represents a single news post for a <see cref="MorningPatch.Domain.Entities.SteamGame"/> entity.
 * </summary>
 */
public sealed record SteamGameNews
{
	public required int AppId { get; init; }
	public required string Name { get; set; }
	public required string ImageIconHash { get; set; }
	public required string Title { get; init; }
	public required string Author { get; init; }
	public required long UnixTimestamp { get; init; }
	public required Uri Uri { get; init; }
}