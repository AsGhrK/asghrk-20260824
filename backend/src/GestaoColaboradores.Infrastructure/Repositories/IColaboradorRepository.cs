using GestaoColaboradores.Domain.Entities;

namespace GestaoColaboradores.Infrastructure.Repositories;

public interface IColaboradorRepository : IRepository<Colaborador>
{
    Task<Colaborador?> GetByIdWithRelationsAsync(int id);
    Task<List<Colaborador>> GetAllWithRelationsAsync();
}
