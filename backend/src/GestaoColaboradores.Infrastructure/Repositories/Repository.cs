using System.Linq.Expressions;
using GestaoColaboradores.Domain.Common;
using GestaoColaboradores.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : EntidadeBase
{
    protected readonly AppDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(AppDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public virtual Task<T?> GetByIdAsync(int id) => DbSet.FirstOrDefaultAsync(e => e.Id == id);

    public virtual Task<List<T>> GetAllAsync() => DbSet.ToListAsync();

    public virtual async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

    public virtual void Update(T entity) => DbSet.Update(entity);

    public virtual void Remove(T entity) => DbSet.Remove(entity);

    public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => DbSet.AnyAsync(predicate);

    public Task SaveChangesAsync() => Context.SaveChangesAsync();
}
