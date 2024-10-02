namespace MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays;
using AutoMapper;
using MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays.DTOs;
using MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays;
using MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays.DTOs;

/**
 * <summary>
 * The AutoMapper <see cref="Profile"/> for the <see cref="ListPreviousDaysNewsEndpoint"/>.
 * </summary>
 */
public sealed class ListPreviousDaysNewsEndpointProfile : Profile
{
	/**
	 * <summary>
	 * Instantiates a new <see cref="ListPreviousDaysNewsEndpointProfile"/> instance.
	 * </summary>
	 */
	public ListPreviousDaysNewsEndpointProfile()
	{
		CreateMap<int, ListPreviousDaysNewsEndpointRequest>()
			.ForMember(destinationMember => destinationMember.DayOffset,
					   memberOptions => memberOptions.MapFrom(sourceMember => sourceMember));

		CreateMap<ListPreviousDaysNewsEndpointRequest, ListPreviousDaysNewsQuery>();
		CreateMap<ListPreviousDaysNewsQueryNewsResponse, ListPreviousDaysNewsEndpointNewsResponse>();
		CreateMap<ListPreviousDaysNewsQuerySteamGameResponse, ListPreviousDaysNewsEndpointSteamGameResponse>();
		CreateMap<ListPreviousDaysNewsQueryResponse, ListPreviousDaysNewsEndpointResponse>();
	}
}