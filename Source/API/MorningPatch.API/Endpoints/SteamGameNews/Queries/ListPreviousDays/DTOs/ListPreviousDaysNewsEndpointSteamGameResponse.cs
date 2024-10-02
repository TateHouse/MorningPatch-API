namespace MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays.DTOs;
/**
 * <summary>
 * A data transfer object containing the <see cref="MorningPatch.Domain.Entities.SteamGame"/> data provided in the
 * response for the <see cref="ListPreviousDaysNewsEndpoint"/>.
 * </summary>
 */
public sealed record ListPreviousDaysNewsEndpointSteamGameResponse
{
	public required int AppId { get; init; }
	public required string Name { get; init; }
	public required string ImageIconHash { get; init; }
}