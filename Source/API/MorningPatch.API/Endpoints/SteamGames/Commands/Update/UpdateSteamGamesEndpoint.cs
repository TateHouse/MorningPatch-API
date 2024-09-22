namespace MorningPatch.API.Endpoints.SteamGames.Commands.Update;
using AutoMapper;
using MediatR;
using MorningPatch.API.Endpoints.SteamGames.Commands.Update.DTOs;
using MorningPatch.Application.Features.SteamGames.Commands.Update;

/**
 * <summary>
 * An endpoint for updating the <see cref="MorningPatch.Domain.Entities.SteamGame"/> stored in the database.
 * </summary>
 */
public static class UpdateSteamGamesEndpoint
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
		groupBuilder.MapPost("/", UpdateSteamGamesEndpoint.HandleAsync)
					.WithName("SteamGames-Update")
					.WithTags(tags)
					.WithSummary("Refreshes the collection of stored Steam games the user owns.")
					.WithDescription("All of the Steam games the user owns are retrieved using the Steam Web API. Then, any new Steam games not already stored are added to the database.")
					.Produces<UpdateSteamGamesEndpointResponse>(StatusCodes.Status200OK)
					.ProducesProblem(StatusCodes.Status500InternalServerError);
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
		try
		{
			var request = new UpdateSteamGamesCommand();
			var response = mapper.Map<UpdateSteamGamesEndpointResponse>(await mediator.Send(request));

			return Results.Ok(response);
		}
		catch (HttpRequestException)
		{
			return Results.Problem("An unknown error occurred with the Steam Web API.", statusCode: StatusCodes.Status500InternalServerError);
		}
	}
}