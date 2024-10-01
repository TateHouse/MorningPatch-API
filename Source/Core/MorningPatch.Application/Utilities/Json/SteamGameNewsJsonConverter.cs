namespace MorningPatch.Application.Utilities.Json;
using MorningPatch.Domain.Entities;
using MorningPatch.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

/**
 * <summary>
 * A <see cref="JsonConverter"/> for <see cref="SteamGameNews"/> models.
 * </summary>
 */
public sealed class SteamGameNewsJsonConverter : JsonConverter<SteamGameNews>
{
	/**
	 * <summary>
	 * A data transfer object for internal use within the <see cref="SteamGameNewsJsonConverter"/>.
	 * </summary>
	 */
	private sealed class ParsedNews
	{
		public string Title { get; set; } = string.Empty;
		public string Author { get; set; } = string.Empty;
		public long UnixTimestamp { get; set; }
		public Uri Uri { get; set; } = null!;
	}

	private readonly SteamGame steamGame;

	public SteamGameNewsJsonConverter(SteamGame steamGame)
	{
		this.steamGame = steamGame;
	}

	public override SteamGameNews? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new JsonException();
		}

		var parsedNews = new ParsedNews();

		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				return new SteamGameNews
				{
					AppId = steamGame.AppId,
					Name = steamGame.Name,
					ImageIconHash = steamGame.ImageIconHash,
					Title = parsedNews.Title,
					Author = parsedNews.Author,
					UnixTimestamp = parsedNews.UnixTimestamp,
					Uri = parsedNews.Uri
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
				case "title":
					var title = reader.GetString();
					SteamGameNewsJsonConverter.ValidateStringProperty(title, "The new's title must be provided.");
					parsedNews.Title = title!;

					break;

				case "author":
					var author = reader.GetString();
					parsedNews.Author = author!;

					break;

				case "date":
					var unixTimestamp = reader.GetInt64();
					parsedNews.UnixTimestamp = unixTimestamp;

					break;

				case "url":
					var uri = reader.GetString();
					SteamGameNewsJsonConverter.ValidateStringProperty(uri, "The new's URI must be provided.");
					parsedNews.Uri = new Uri(uri!);

					break;

				default:
					reader.Skip();

					break;
			}
		}

		throw new JsonException();
	}

	private static void ValidateStringProperty(string? property, string errorMessage)
	{
		if (string.IsNullOrWhiteSpace(property) || property.Trim() == ",")
		{
			throw new JsonException(errorMessage);
		}
	}

	public override void Write(Utf8JsonWriter writer, SteamGameNews value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteNumber("AppId", value.AppId);
		writer.WriteString("Name", value.Name);
		writer.WriteString("ImageIconHash", value.ImageIconHash);
		writer.WriteString("Title", value.Title);
		writer.WriteString("Author", value.Author);
		writer.WriteNumber("UnixTimestamp", value.UnixTimestamp);
		writer.WriteString("Uri", value.Uri.ToString());
		writer.WriteEndObject();
	}
}