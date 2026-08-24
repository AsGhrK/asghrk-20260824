using GestaoColaboradores.Domain.Entities;
using GestaoColaboradores.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context)
    {
    }

    public Task<Usuario?> GetByLoginAsync(string login) =>
        DbSet.FirstOrDefaultAsync(u => u.Login == login);

    public Task<List<Usuario>> GetByStatusAsync(bool ativo) =>
        DbSet.Where(u => u.Ativo == ativo).ToListAsync();
}
