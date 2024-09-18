namespace MorningPatch.Persistence;
using Microsoft.EntityFrameworkCore;

/**
 * <summary>
 * The application's Entity Framework Core <see cref="DbContext"/>.
 * </summary>
 */
public sealed class DatabaseContext : DbContext
{
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