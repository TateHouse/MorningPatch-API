namespace MorningPatch.API.Endpoints.SteamGames.Queries.List;
using AutoMapper;
using MediatR;
using MorningPatch.API.Endpoints.SteamGames.Queries.List.DTO;
using MorningPatch.Application.Features.SteamGames.Queries.List;

/**
 * <summary>
 * An endpoint for retrieving all <see cref="MorningPatch.Domain.Entities.SteamGame"/> entities stored in the database.
 * </summary>
 */
public static class ListSteamGamesEndpoint
{
	/**
	 * <summary>
	 * Maps the endpoint.
	 * </summary>
	 * <param name="groupBuilder">The builder for the route group.</param>
	 * <param name="tags">An array of tags to assocaite with this endpoint.</param>
	 */
	public static void MapEndpoint(RouteGroupBuilder groupBuilder, string[] tags)
	{
		groupBuilder.MapGet("/", ListSteamGamesEndpoint.HandleAsync)
					.WithName("SteamGames-List")
					.WithTags(tags)
					.WithSummary("Retrieves all stored Steam games.")
					.WithDescription("Retrieves all stored Steam games from the database.")
					.Produces<ListSteamGamesEndpointResponse>(StatusCodes.Status200OK);
	}

	/**
	 * <summary>
	 * Asynchronously handles the endpoint.
	 * </summary>
	 * <param name="mediator">The mediator to use.</param>
	 * <param name="mapper">The mapper to use.</param>
	 * <returns>A task that represents the asynchronous operation, and it contains the endpoint's <see cref="IResult"/>.</returns>
	 */
	private async static Task<IResult> HandleAsync(IMediator mediator,
												   IMapper mapper)
	{
		var request = new ListSteamGamesQuery();
		var response = mapper.Map<ListSteamGamesEndpointResponse>(await mediator.Send(request));

		return Results.Ok(response);
	}
}