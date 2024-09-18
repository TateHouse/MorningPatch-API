namespace MorningPatch.Persistence;
using Microsoft.EntityFrameworkCore;
using MorningPatch.Domain;

/**
 * <summary>
 * The application's Entity Framework Core <see cref="DbContext"/>.
 * </summary>
 */
public sealed class DatabaseContext : DbContext
{
	/**
	 * <summary>
	 * The collection of <see cref="SteamGame"/> that the user owns on Steam.
	 * </summary>
	 */
	public DbSet<SteamGame> OwnedSteamGames { get; set; }

	/**
	 * <summary>
	 * Instantiates a new <see cref="DatabaseContext"/> instance.
	 * </summary>
	 * <param name="options">The options to be used by the <see cref="DatabaseContext"/>.</param>
	 */
	public DatabaseContext(DbContextOptions<DatabaseContext> options)
		: base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
	}
}