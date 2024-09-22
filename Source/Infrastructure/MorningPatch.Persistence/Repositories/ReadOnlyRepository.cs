namespace MorningPatch.Persistence.Repositories;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using MorningPatch.Application.Abstractions.Persistence.Repositories;

/**
 * <summary>
 * A read-only Entity Framework Core repository.
 * </summary>
 * <typeparam name="TEntity">The type of the entity stored in the <see cref="ReadOnlyRepository{TEntity}"/>.</typeparam>
 */
public class ReadOnlyRepository<TEntity> : RepositoryBase<TEntity>, IReadOnlyRepository<TEntity>
	where TEntity : class
{
	/**
	 * <summary>
	 * Instantiates a new <see cref="ReadOnlyRepository{TEntity}"/> instance.
	 * </summary>
	 * <param name="databaseContext">The application's Entity Framework Core <see cref="Microsoft.EntityFrameworkCore.DbContext"/>.</param>
	 */
	public ReadOnlyRepository(DatabaseContext databaseContext)
		: base(databaseContext)
	{

	}

	/**
	 * <summary>
	 * Instantiates a new <see cref="ReadOnlyRepository{TEntity}"/> instance.
	 * </summary>
	 * <param name="databaseContext">The application's Entity Framework Core <see cref="Microsoft.EntityFrameworkCore.DbContext"/>.</param>
	 * <param name="specificationEvaluator">The evaluator that applies specifications.</param>
	 */
	public ReadOnlyRepository(DatabaseContext databaseContext, ISpecificationEvaluator specificationEvaluator)
		: base(databaseContext, specificationEvaluator)
	{

	}
}