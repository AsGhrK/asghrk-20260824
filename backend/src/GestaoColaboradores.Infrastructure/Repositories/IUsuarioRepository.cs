using GestaoColaboradores.Domain.Entities;

namespace GestaoColaboradores.Infrastructure.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByLoginAsync(string login);
    Task<List<Usuario>> GetByStatusAsync(bool ativo);
}
