namespace MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MorningPatch.API.Endpoints.SteamGameNews.Queries.ListPreviousDays.DTOs;
using MorningPatch.Application.Features.SteamGameNews.Queries.ListPreviousDays;

/**
 * <summary>
 * An endpoint for retrieving all news for stored <see cref="MorningPatch.Domain.Entities.SteamGame"/> entities for the
 * given number of days.
 * </summary>
 */
public static class ListPreviousDaysNewsEndpoint
{
	/**
	 * <summary>
	 * Maps the endpoint.
	 * </summary>
	 * <param name="groupBuilder">The builder for the route group.</param>
	 * <param name="tags">An array of tags to associate with this endpoint.</param>
	 */
	public static void MapEndpoint(RouteGroupBuilder groupBuilder, string[] tags)
	{
		groupBuilder.MapGet("/", ListPreviousDaysNewsEndpoint.HandleAsync)
					.WithName("SteamGameNews-ListPreviousDaysNews")
					.WithTags(tags)
					.WithSummary("Retrieves the news for all stored Steam games in the database for the given number of days.")
					.WithDescription("Retrieves the news for all stored Steam games in the database for the given number of days.")
					.Produces<ListPreviousDaysNewsEndpointResponse>(StatusCodes.Status200OK)
					.ProducesProblem(StatusCodes.Status500InternalServerError);
	}

	/**
	 * <summary>
	 * Asynchronously handles the endpoint.
	 * </summary>
	 * <param name="mediator">The mediator to use.</param>
	 * <param name="mapper">The mapper to use.</param>
	 * <param name="dayOffset">The number of days to retrieve the news for.</param>
	 * <returns>A task that represents the asynchronous operation, and it contains the endpoint's <see cref="IResult"/>.</returns>
	 */
	private async static Task<IResult> HandleAsync(IMediator mediator,
												   IMapper mapper,
												   [FromQuery] int dayOffset)
	{
		try
		{
			var negativeDayOffset = Math.Abs(dayOffset) * -1;
			var request = mapper.Map<ListPreviousDaysNewsEndpointRequest>(negativeDayOffset);
			var query = mapper.Map<ListPreviousDaysNewsQuery>(request);
			var response = mapper.Map<ListPreviousDaysNewsEndpointResponse>(await mediator.Send(query));

			return Results.Ok(response);
		}
		catch (HttpRequestException)
		{
			return Results.Problem(detail: "An unknown error occured with the Steam Web API.",
								   statusCode: StatusCodes.Status500InternalServerError);
		}
	}
}