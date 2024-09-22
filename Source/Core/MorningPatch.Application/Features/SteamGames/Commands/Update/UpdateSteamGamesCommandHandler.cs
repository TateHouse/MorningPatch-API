namespace MorningPatch.Application.Features.SteamGames.Commands.Update;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using MorningPatch.Application.Abstractions.Persistence.Repositories;
using MorningPatch.Application.Features.SteamGames.Commands.Update.DTOs;
using MorningPatch.Application.Utilities.Json;
using MorningPatch.Domain.Entities;
using System.Text.Json;
using System.Web;

/**
 * <summary>
 * The command handler for the <see cref="UpdateSteamGamesCommand"/>.
 * </summary>
 */
public sealed class UpdateSteamGamesCommandHandler : IRequestHandler<UpdateSteamGamesCommand, UpdateSteamGamesCommandResponse>
{
	private readonly IMapper mapper;
	private readonly IRepository<SteamGame> steamGameRepository;
	private readonly IHttpClientFactory httpClientFactory;
	private readonly IConfiguration configuration;

	/**
	 * <summary>
	 * Instantiates a new <see cref="UpdateSteamGamesCommandHandler"/> instance.
	 * </summary>
	 * <param name="mapper">The mapper to use.</param>
	 * <param name="steamGameRepository">An entity repository of <see cref="SteamGame"/> entities.</param>
	 * <param name="httpClientFactory">A factory to create <see cref="HttpClient"/> instances.</param>
	 * <param name="configuration">The application's set of key/value pair configurations.</param>
	 */
	public UpdateSteamGamesCommandHandler(IMapper mapper,
										  IRepository<SteamGame> steamGameRepository,
										  IHttpClientFactory httpClientFactory,
										  IConfiguration configuration)
	{
		this.mapper = mapper;
		this.steamGameRepository = steamGameRepository;
		this.httpClientFactory = httpClientFactory;
		this.configuration = configuration;
	}

	public async Task<UpdateSteamGamesCommandResponse> Handle(UpdateSteamGamesCommand request,
															  CancellationToken cancellationToken)
	{
		var currentSteamGames = await steamGameRepository.ListAsync(cancellationToken);
		var currentAppIds = new HashSet<int>(currentSteamGames.Select(steamGame => steamGame.AppId));
		var newSteamGames = (await ListOwnedGamesAsync()).Where(steamGame => !currentAppIds.Contains(steamGame.AppId))
														 .ToList();

		if (newSteamGames.Count == 0)
		{
			return mapper.Map<UpdateSteamGamesCommandResponse>(0);
		}

		await steamGameRepository.AddRangeAsync(newSteamGames, cancellationToken);
		await steamGameRepository.SaveChangesAsync(cancellationToken);

		return mapper.Map<UpdateSteamGamesCommandResponse>(newSteamGames.Count);
	}

	/**
	 * <summary>
	 * Asynchronously retrieves the Steam games owned.
	 * </summary>
	 * <returns>A task that represents the asynchronous operation, and it contains a collection of <see cref="SteamGame"/>
	 * entities retrieved from the Steam Web API that the user owns.</returns>
	 * <exception cref="HttpRequestException">Thrown if the HTTP request to the Steam Web API fails.</exception>
	 */
	private async Task<IEnumerable<SteamGame>> ListOwnedGamesAsync()
	{
		using var httpClient = httpClientFactory.CreateClient();
		var url = BuildOwnedGamesUrl();
		var response = await httpClient.GetAsync(url);

		if (response.IsSuccessStatusCode == false)
		{
			throw new HttpRequestException($"The request to {url} failed with status code {response.StatusCode}.");
		}

		var content = await response.Content.ReadAsStreamAsync();
		using var jsonDocument = await JsonDocument.ParseAsync(content);
		var gamesJson = jsonDocument.RootElement.GetProperty("response").GetProperty("games");
		var jsonOptions = new JsonSerializerOptions();
		jsonOptions.Converters.Add(new SteamGameJsonConverter());

		var games = JsonSerializer.Deserialize<List<SteamGame>>(gamesJson.GetRawText(), jsonOptions);

		return games ?? new List<SteamGame>();
	}

	/**
	 * <summary>
	 * Builds the url for retrieving the Steam games owned.
	 * </summary>
	 * <returns>A string representation of the url for retrieving the Steam games owned.</returns>
	 * <exception cref="InvalidOperationException">Thrown if either of the necessary environment variables are not set.</exception>
	 */
	private string BuildOwnedGamesUrl()
	{
		const string baseUrl = "https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/";
		const string steamWebApiKeyEnvironmentVariableName = "STEAM_WEB_API_KEY";
		const string steamIdEnvironmentVariableName = "STEAM_ID";

		var apiKey = configuration[steamWebApiKeyEnvironmentVariableName] ?? throw new InvalidOperationException($"Environment variable not set: {steamWebApiKeyEnvironmentVariableName}");
		var steamId = configuration[steamIdEnvironmentVariableName] ?? throw new InvalidOperationException($"Environment variable not set: {steamIdEnvironmentVariableName}");
		var uriBuilder = new UriBuilder(baseUrl);
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["key"] = apiKey;
		query["steamid"] = steamId;
		query["include_appinfo"] = "1";
		query["include_played_free_games"] = "1";

		uriBuilder.Query = query.ToString();

		return uriBuilder.ToString();
	}
}