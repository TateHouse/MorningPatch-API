namespace MorningPatch.Application.Features.SteamGames.Commands.Update.DTOs;
using MorningPatch.Domain.Entities;

/**
 * <summary>
 * A data transfer object containing the number of <see cref="SteamGame"/> entities updated in the
 * database.
 * </summary>
 */
public sealed record UpdateSteamGamesCommandResponse
{
	public int Count { get; init; }
}