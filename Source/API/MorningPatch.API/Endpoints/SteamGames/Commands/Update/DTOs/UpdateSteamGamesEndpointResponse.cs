namespace MorningPatch.API.Endpoints.SteamGames.Commands.Update.DTOs;
/**
 * <summary>
 * A data transfer object containing the data returned by the <see cref="UpdateSteamGamesEndpoint"/>.
 * </summary>
 */
public sealed record UpdateSteamGamesEndpointResponse
{
	public required int Count { get; init; }
}