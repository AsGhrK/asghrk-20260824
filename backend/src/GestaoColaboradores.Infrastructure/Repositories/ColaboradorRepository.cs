using GestaoColaboradores.Domain.Entities;
using GestaoColaboradores.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.Infrastructure.Repositories;

public class ColaboradorRepository : Repository<Colaborador>, IColaboradorRepository
{
    public ColaboradorRepository(AppDbContext context) : base(context)
    {
    }

    public Task<Colaborador?> GetByIdWithRelationsAsync(int id) =>
        DbSet.Include(c => c.Unidade).Include(c => c.Usuario).FirstOrDefaultAsync(c => c.Id == id);

    public Task<List<Colaborador>> GetAllWithRelationsAsync() =>
        DbSet.Include(c => c.Unidade).Include(c => c.Usuario).ToListAsync();
}
