namespace MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays.DTOs;
/**
 * <summary>
 * A data transfer object containing the data provided in the request for the <see cref="ListPreviousDaysNewsEndpoint"/>.
 * </summary>
 */
public sealed record ListPreviousDaysNewsEndpointRequest
{
	public required int DayOffset { get; init; }
}