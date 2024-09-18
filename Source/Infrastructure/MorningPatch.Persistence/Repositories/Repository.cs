namespace MorningPatch.Persistence.Repositories;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MorningPatch.Application.Abstractions.Persistence.Repositories;

/**
 * <summary>
 * A read-write Entity Framework Core repository.
 * </summary>
 * <typeparam name="TEntity">The type of the entity stored in the <see cref="Repository{TEntity}"/>.</typeparam>
 */
public class Repository<TEntity> : RepositoryBase<TEntity>, IRepository<TEntity>
	where TEntity : class
{
	/**
	 * <summary>
	 * Instantiates a new <see cref="Repository{TEntity}"/> instance.
	 * </summary>
	 * <param name="databaseContext">The application's Entity Framework Core <see cref="DbContext"/>.</param>
	 */
	public Repository(DatabaseContext databaseContext)
		: base(databaseContext)
	{

	}

	/**
	 * <summary>
	 * Instantiates a new <see cref="Repository{TEntity}"/> instance.
	 * </summary>
	 * <param name="databaseContext">The application's Entity Framework Core <see cref="DbContext"/>.</param>
	 * <param name="specificationEvaluator">The evaluator that applies specifications.</param>
	 */
	public Repository(DatabaseContext databaseContext, ISpecificationEvaluator specificationEvaluator)
		: base(databaseContext, specificationEvaluator)
	{

	}
}