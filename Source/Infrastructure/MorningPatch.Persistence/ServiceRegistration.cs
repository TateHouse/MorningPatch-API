namespace MorningPatch.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/**
 * <summary>
 * A class for registering services.
 * </summary>
 */
public static class ServiceRegistration
{

	/**
	 * <summary>
	 * An extension method for <see cref="IServiceCollection"/> to register the <see cref="Persistence"/> services.
	 * </summary>
	 * <exception cref="ArgumentException">Thrown if the database name or provider is null or whitespace.</exception>
	 * <exception cref="InvalidOperationException">Thrown if the database settings failed to parse.</exception>
	 * <exception cref="NotSupportedException">Thrown if the database provider is not supported.</exception>
	 */
	public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services,
																 IConfiguration configuration)
	{
		var databaseSettings = configuration.GetSection("Database").Get<DatabaseSettings>();

		if (databaseSettings == null)
		{
			throw new InvalidOperationException("At least one of the provided database settings was invalid.");
		}

		services.AddDbContext<DatabaseContext>(optionsAction =>
		{
			ServiceRegistration.ConfigureDatabaseContext(optionsAction, databaseSettings);
		});

		return services;
	}

	/**
	 * <summary>
	 * Configures the Entity Framework Core <see cref="DatabaseContext"/>.
	 * </summary>
	 * <param name="optionsBuilder">The options builder to use for the <see cref="DatabaseContext"/>.</param>
	 * <param name="databaseSettings">The database settings to use for the <see cref="DatabaseContext"/>.</param>
	 * <exception cref="NotSupportedException">Thrown if the database provider is not supported.</exception>
	 */
	private static void ConfigureDatabaseContext(DbContextOptionsBuilder optionsBuilder,
												 DatabaseSettings databaseSettings)
	{
		switch (databaseSettings.Provider)
		{
			case "InMemory":
				optionsBuilder.UseInMemoryDatabase(databaseSettings.Name);
				optionsBuilder.ConfigureWarnings(warningsConfigurationBuilderAction =>
				{
					warningsConfigurationBuilderAction.Ignore(InMemoryEventId.TransactionIgnoredWarning);
				});

				break;

			default:
				throw new NotSupportedException($"Unsupported database provider: {databaseSettings.Provider}");
		}
	}
}