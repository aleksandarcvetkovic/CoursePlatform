using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

