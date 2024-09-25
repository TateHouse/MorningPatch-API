namespace MorningPatch.Application.Features.SteamGames.Queries.List;
using MediatR;
using MorningPatch.Application.Features.SteamGames.Queries.List.DTOs;

/**
 * <summary>
 * A query for retrieving all <see cref="MorningPatch.Domain.Entities.SteamGame"/> entities from the database.
 * </summary>
 */
public sealed class ListSteamGamesQuery : IRequest<ListSteamGamesQueryResponse>
{

}