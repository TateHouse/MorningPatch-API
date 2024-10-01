namespace MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays;
using AutoMapper;
using MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays.DTOs;
using MorningPatch.Domain.Entities;
using MorningPatch.Domain.Models;

/**
 * <summary>
 * The AutoMapper <see cref="Profile"/> for the <see cref="ListPreviousDaysNewsQuery"/>.
 * </summary>
 */
public sealed class ListPreviousDaysNewsQueryProfile : Profile
{
	/**
	 * <summary>
	 * Instantaites a new <see cref="ListPreviousDaysNewsQueryProfile"/> instance.
	 * </summary>
	 */
	public ListPreviousDaysNewsQueryProfile()
	{
		CreateMap<SteamGame, ListPreviousDaysNewsQuerySteamGameResponse>();
		CreateMap<SteamGameNews, ListPreviousDaysNewsQueryNewsResponse>();
		CreateMap<IEnumerable<SteamGameNews>, ListPreviousDaysNewsQueryResponse>()
			.ForMember(destinationMember => destinationMember.News,
					   memberOptions => memberOptions.MapFrom(sourceMember => sourceMember));
	}
}