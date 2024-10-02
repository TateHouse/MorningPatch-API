namespace MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays.DTOs;
/**
 * <summary>
 * A data transfer object containing the <see cref="MorningPatch.Domain.Models.SteamGameNews"/> data provided in the
 * response for the <see cref="ListPreviousDaysNewsEndpoint"/>.
 * </summary>
 */
public sealed record ListPreviousDaysNewsEndpointNewsResponse
{
	public required ListPreviousDaysNewsEndpointSteamGameResponse SteamGame { get; init; }
	public required string Title { get; init; }
	public required string Author { get; init; }
	public required long UnixTimestamp { get; init; }
	public required Uri Uri { get; init; }
}