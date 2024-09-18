namespace MorningPatch.Application.Features.SteamGames.Commands.Update.DTOs;
/**
 * <summary>
 * A data transfer object containing the number of <see cref="MorningPatch.Domain.SteamGame"/> entities updated in the
 * database.
 * </summary>
 */
public sealed record UpdateSteamGamesCommandResponse
{
	public int Count { get; init; }
}