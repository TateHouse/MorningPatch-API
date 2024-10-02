namespace MorningPatch.Application.UnitTest.Utilities.Json;
using MorningPatch.Application.Utilities.Json;
using MorningPatch.Domain.Entities;
using MorningPatch.Domain.Models;
using System.Text.Json;

[TestFixture]
public sealed class SteamGameNewsJsonConverterTest
{
	private JsonSerializerOptions jsonSerializerOptions;

	[SetUp]
	public void SetUp()
	{
		var steamGame = new SteamGame
		{
			AppId = 10,
			Name = "Counter-Strike",
			ImageIconHash = "6b0312cda02f5f777efa2f3318c307ff9acafbb5",
		};

		jsonSerializerOptions = new JsonSerializerOptions
		{
			Converters = { new SteamGameNewsJsonConverter(steamGame) }
		};
	}

	[Test]
	public void GivenInvalidJson_WhenRead_ThenThrowsJsonException()
	{
		const string steamGameNewsJson = """{"appid":10,""";

		Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<SteamGameNews>(steamGameNewsJson, jsonSerializerOptions));
	}

	[Test]
	public void GivenValidJson_WhenRead_ThenReturnsSteamGameNews()
	{
		const string steamGameNewsJson = """{"appid":10, "title": "Counter-Strike creator regrets not balancing the AWP", "author": "editor@pcgamesn.com", "date": 1722529128, "url": "https://steamstore-a.akamaihd.net/news/externalpost/PCGamesN/5963413728335624909"}""";
		var steamGameNews = JsonSerializer.Deserialize<SteamGameNews>(steamGameNewsJson, jsonSerializerOptions);

		Assert.That(steamGameNews, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(steamGameNews.AppId, Is.EqualTo(10));
			Assert.That(steamGameNews.Author, Is.EqualTo("editor@pcgamesn.com"));
			Assert.That(steamGameNews.Title, Is.EqualTo("Counter-Strike creator regrets not balancing the AWP"));
			Assert.That(steamGameNews.Uri, Is.EqualTo(new Uri("https://steamstore-a.akamaihd.net/news/externalpost/PCGamesN/5963413728335624909")));
			Assert.That(steamGameNews.UnixTimestamp, Is.EqualTo(1722529128));
		});
	}

	[Test]
	public void GivenValidSteamGameNews_WhenWrite_ThenWritesSteamGameNews()
	{
		var steamGameNews = new SteamGameNews
		{
			AppId = 10,
			Name = "Counter-Strike",
			ImageIconHash = "6b0312cda02f5f777efa2f3318c307ff9acafbb5",
			Title = "Counter-Strike creator regrets not balancing the AWP",
			Author = "editor@pcgamesn.com",
			UnixTimestamp = 1722529128,
			Uri = new Uri("https://steamstore-a.akamaihd.net/news/externalpost/PCGamesN/5963413728335624909")
		};

		var steamGameNewsJson = JsonSerializer.Serialize<SteamGameNews>(steamGameNews, jsonSerializerOptions);

		Assert.That(steamGameNewsJson, Is.Not.Null);
		Assert.That(steamGameNewsJson, Contains.Substring(@"""AppId"":10,"));
		Assert.That(steamGameNewsJson, Contains.Substring(@"""Name"":""Counter-Strike"","));
		Assert.That(steamGameNewsJson, Contains.Substring(@"""ImageIconHash"":""6b0312cda02f5f777efa2f3318c307ff9acafbb5"","));
		Assert.That(steamGameNewsJson, Contains.Substring(@"""Title"":""Counter-Strike creator regrets not balancing the AWP"","));
		Assert.That(steamGameNewsJson, Contains.Substring(@"""Author"":""editor@pcgamesn.com"","));
		Assert.That(steamGameNewsJson, Contains.Substring(@"""UnixTimestamp"":1722529128,"));
		Assert.That(steamGameNewsJson, Contains.Substring(@"""Uri"":""https://steamstore-a.akamaihd.net/news/externalpost/PCGamesN/5963413728335624909"""));
	}
}