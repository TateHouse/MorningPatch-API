namespace MorningPatch.Persistence;
/**
 * <summary>
 * A class that stores the settings for the database.
 * </summary>
 */
public sealed class DatabaseSettings
{
	/**
	 * <summary>
	 * The name of the database.
	 * </summary>
	 */
	public string Name { get; init; }

	/**
	 * <summary>
	 * The name of the database provider.
	 * </summary>
	 */
	public string Provider { get; init; }

	/**
	 * <summary>
	 * Instantiates a new <see cref="DatabaseSettings"/> instance.
	 * </summary>
	 * <exception cref="ArgumentException">Thrown if the <paramref name="name"/> or <paramref name="provider"/> is null or whitespace.</exception>
	 */
	public DatabaseSettings(string name, string provider)
	{
		if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(provider))
		{
			throw new ArgumentException("The Database:Name and Database:Provider must be provided.");
		}

		Name = name;
		Provider = provider;
	}
}