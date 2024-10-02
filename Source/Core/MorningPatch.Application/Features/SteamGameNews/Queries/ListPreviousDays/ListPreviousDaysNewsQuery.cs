namespace MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays;
using MediatR;
using MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays.DTOs;

/**
 * <summary>
 * A query for retrieving the news for all <see cref="MorningPatch.Domain.Entities.SteamGame"/> entities in the
 * database for a specific number of days ago.
 * </summary>
 */
public sealed class ListPreviousDaysNewsQuery : IRequest<ListPreviousDaysNewsQueryResponse>
{
	public required int DayOffset { get; init; }
}