using System.Linq.Expressions;
using GestaoColaboradores.Domain.Common;

namespace GestaoColaboradores.Infrastructure.Repositories;

public interface IRepository<T> where T : EntidadeBase
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task SaveChangesAsync();
}
