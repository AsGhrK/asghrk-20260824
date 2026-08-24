using GestaoColaboradores.Domain.Entities;
using GestaoColaboradores.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.Infrastructure.Repositories;

public class UnidadeRepository : Repository<Unidade>, IUnidadeRepository
{
    public UnidadeRepository(AppDbContext context) : base(context)
    {
    }

    public Task<Unidade?> GetByIdWithColaboradoresAsync(int id) =>
        DbSet.Include(u => u.Colaboradores).FirstOrDefaultAsync(u => u.Id == id);

    public Task<List<Unidade>> GetAllWithColaboradoresAsync() =>
        DbSet.Include(u => u.Colaboradores).ToListAsync();
}
