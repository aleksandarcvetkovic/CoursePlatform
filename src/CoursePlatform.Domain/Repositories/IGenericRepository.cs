using CoursePlatform.Domain.Common;
using System.Linq.Expressions;

namespace CoursePlatform.Domain.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    // Basic CRUD operations
    Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string id, CancellationToken cancellationToken = default);

}
