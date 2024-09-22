namespace MorningPatch.Application.UnitTest.Utilities.Json;
using MorningPatch.Application.Utilities.Json;
using MorningPatch.Domain.Entities;
using System.Text.Json;

[TestFixture]
public sealed class SteamGameJsonConverterTest
{
	private JsonSerializerOptions jsonSerializerOptions;

	[SetUp]
	public void SetUp()
	{
		jsonSerializerOptions = new JsonSerializerOptions
		{
			Converters = { new SteamGameJsonConverter() },
		};
	}

	[Test]
	public void GivenInvalidJson_WhenRead_ThenThrowsJsonException()
	{
		const string steamGameJson = """{"appid":10, "name": "Counter-Strike""";

		Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<SteamGame>(steamGameJson, jsonSerializerOptions));
	}

	[Test]
	public void GivenValidJson_WhenRead_ThenReturnsSteamGame()
	{
		const string steamGameJson = """{"appid":10, "name": "Counter-Strike", "img_icon_url": "6b0312cda02f5f777efa2f3318c307ff9acafbb5"}""";
		var steamGame = JsonSerializer.Deserialize<SteamGame>(steamGameJson, jsonSerializerOptions);

		Assert.That(steamGame, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(steamGame.AppId, Is.EqualTo(10));
			Assert.That(steamGame.Name, Is.EqualTo("Counter-Strike"));
			Assert.That(steamGame.ImageIconHash, Is.EqualTo("6b0312cda02f5f777efa2f3318c307ff9acafbb5"));
		});
	}

	[Test]
	public void GivenValidSteamGame_WhenWrite_ThenWritesSteamGame()
	{
		var steamGame = new SteamGame
		{
			AppId = 10,
			Name = "Counter-Strike",
			ImageIconHash = "6b0312cda02f5f777efa2f3318c307ff9acafbb5"
		};

		var steamGameJson = JsonSerializer.Serialize<SteamGame>(steamGame, jsonSerializerOptions);

		Assert.That(steamGameJson, Is.Not.Null);
		Assert.That(steamGameJson, Contains.Substring(@"""AppId"":10,"));
		Assert.That(steamGameJson, Contains.Substring(@"""Name"":""Counter-Strike"","));
		Assert.That(steamGameJson, Contains.Substring(@"""ImageIconHash"":""6b0312cda02f5f777efa2f3318c307ff9acafbb5"""));
	}
}