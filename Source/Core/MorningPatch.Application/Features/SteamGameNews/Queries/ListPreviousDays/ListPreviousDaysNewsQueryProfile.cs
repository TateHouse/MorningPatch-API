namespace MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays;
using AutoMapper;
using MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays.DTOs;
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
	 * Instantiates a new <see cref="ListPreviousDaysNewsQueryProfile"/> instance.
	 * </summary>
	 */
	public ListPreviousDaysNewsQueryProfile()
	{
		CreateMap<SteamGameNews, ListPreviousDaysNewsQueryNewsResponse>()
			.ForMember(destinationMember => destinationMember.SteamGame,
					   memberOptions => memberOptions.MapFrom(sourceMember => new ListPreviousDaysNewsQuerySteamGameResponse
					   {
						   AppId = sourceMember.AppId,
						   Name = sourceMember.Name,
						   ImageIconHash = sourceMember.ImageIconHash
					   }));

		CreateMap<IEnumerable<SteamGameNews>, ListPreviousDaysNewsQueryResponse>()
			.ForMember(destinationMember => destinationMember.News,
					   memberOptions => memberOptions.MapFrom(sourceMember => sourceMember));
	}
}