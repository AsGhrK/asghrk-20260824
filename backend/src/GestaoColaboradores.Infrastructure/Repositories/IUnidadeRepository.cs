using GestaoColaboradores.Domain.Entities;

namespace GestaoColaboradores.Infrastructure.Repositories;

public interface IUnidadeRepository : IRepository<Unidade>
{
    Task<Unidade?> GetByIdWithColaboradoresAsync(int id);
    Task<List<Unidade>> GetAllWithColaboradoresAsync();
}
