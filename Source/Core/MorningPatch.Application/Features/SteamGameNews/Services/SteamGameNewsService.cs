namespace MorningPatch.Application.Features.SteamGameNews.Services;
using MorningPatch.Application.Abstractions.Application.SteamGameNews;
using MorningPatch.Application.Utilities.Json;
using MorningPatch.Domain.Entities;
using MorningPatch.Domain.Models;
using System.Text.Json;
using System.Web;

/**
 * <summary>
 * A service that uses the Steam Web API to retrieve the news for a given <see cref="SteamGame"/> entity.
 * </summary>
 */
public sealed class SteamGameNewsService : ISteamGameNewsService
{
	private readonly IHttpClientFactory httpClientFactory;

	/**
	 * <summary>
	 * Instantiates a new <see cref="SteamGameNewsService"/> instance.
	 * </summary>
	 * <param name="httpClientFactory">A factory to create <see cref="HttpClient"/> instances.</param>
	 */
	public SteamGameNewsService(IHttpClientFactory httpClientFactory)
	{
		this.httpClientFactory = httpClientFactory;
	}

	public async Task<IEnumerable<SteamGameNews>> GetNewsForGameAsync(SteamGame steamGame, long startUnixTimestamp)
	{
		var url = SteamGameNewsService.BuildGetNewsForGameUrl(steamGame.AppId);
		using var httpClient = httpClientFactory.CreateClient();
		var response = await httpClient.GetAsync(url);

		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException($"The request to {url} failed with status code {response.StatusCode}.");
		}

		var content = await response.Content.ReadAsStreamAsync();
		using var jsonDocument = await JsonDocument.ParseAsync(content);
		var newsJson = jsonDocument.RootElement.GetProperty("appnews").GetProperty("newsitems");
		var jsonOptions = new JsonSerializerOptions();
		jsonOptions.Converters.Add(new SteamGameNewsJsonConverter(steamGame));

		var gameNews = (JsonSerializer.Deserialize<List<SteamGameNews>>(newsJson.GetRawText(), jsonOptions) ?? new List<SteamGameNews>())
					   .AsEnumerable()
					   .Where(news => news.UnixTimestamp >= startUnixTimestamp);

		return gameNews;
	}

	/**
	 * <summary>
	 * Builds the url for retrieving the news for the <see cref="SteamGame"/> entity.
	 * </summary>
	 * <returns>A string representation of the url for retrieving the <see cref="SteamGame"/> entity's news.</returns>
	 */
	private static string BuildGetNewsForGameUrl(int appId)
	{
		const string baseUrl = "https://api.steampowered.com/ISteamNews/GetNewsForApp/v2/";

		var uriBuilder = new UriBuilder(baseUrl);
		var query = HttpUtility.ParseQueryString(uriBuilder.Query);
		query["appid"] = appId.ToString();

		uriBuilder.Query = query.ToString();

		return uriBuilder.ToString();
	}
}