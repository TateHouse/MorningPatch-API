namespace MorningPatch.Application.Utilities.Json;
using MorningPatch.Domain.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

/**
 * <summary>
 * A <see cref="JsonConverter"/> for <see cref="SteamGame"/> entities.
 * </summary>
 */
public sealed class SteamGameJsonConverter : JsonConverter<SteamGame>
{
	/**
	 * <summary>
	 * A data transfer object for internal use within the <see cref="SteamGameJsonConverter"/>.
	 * </summary>
	 */
	private sealed class ParsedSteamGame
	{
		public int AppId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string ImageIconHash { get; set; } = string.Empty;
	}

	public override SteamGame? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new JsonException();
		}

		var parsedSteamGame = new ParsedSteamGame();

		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				return new SteamGame
				{
					AppId = parsedSteamGame.AppId,
					Name = parsedSteamGame.Name,
					ImageIconHash = parsedSteamGame.ImageIconHash
				};
			}

			if (reader.TokenType != JsonTokenType.PropertyName)
			{
				throw new JsonException();
			}

			var propertyName = reader.GetString();
			reader.Read();

			switch (propertyName)
			{
				case "appid":
					parsedSteamGame.AppId = reader.GetInt32();

					break;

				case "name":
					var name = reader.GetString();

					if (string.IsNullOrWhiteSpace(name) || name.Trim() == ",")
					{
						throw new JsonException("The game's name must be provided.");
					}

					parsedSteamGame.Name = name;

					break;

				case "img_icon_url":
					parsedSteamGame.ImageIconHash = reader.GetString()!;

					break;

				default:
					reader.Skip();

					break;
			}
		}

		throw new JsonException();
	}

	public override void Write(Utf8JsonWriter writer, SteamGame value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteNumber("AppId", value.AppId);
		writer.WriteString("Name", value.Name);
		writer.WriteString("ImageIconHash", value.ImageIconHash);
		writer.WriteEndObject();
	}
}