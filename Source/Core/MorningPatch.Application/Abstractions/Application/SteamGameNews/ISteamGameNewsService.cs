namespace MorningPatch.Application.Abstractions.Application.SteamGameNews;
using MorningPatch.Domain.Entities;
using MorningPatch.Domain.Models;

/**
 * <summary>
 * An interface for retrieving news for a specific Steam game.
 * </summary>
 */
public interface ISteamGameNewsService
{
	/**
	 * <summary>
	 * Retrieves the news for the <paramref name="steamGame"/>, starting at the<paramref name="startUnixTimestamp"/>
	 * up until the current time the request is made.
	 * </summary>
	 * <param name="steamGame">The game the retrieve the news for.</param>
	 * <param name="startUnixTimestamp">The Unix timestamp for the time to start from.</param>
	 * <returns>A task that represents the asynchronous operation, and it contains an enumerable of
	 * <see cref="SteamGameNews"/> instances for the given <paramref name="steamGame"/>.</returns>
	 * <exception cref="HttpRequestException">Thrown if the Steam Web API request fails.</exception>
	 */
	public Task<IEnumerable<SteamGameNews>> GetNewsForGameAsync(SteamGame steamGame, long startUnixTimestamp);
}