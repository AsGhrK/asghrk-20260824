using GestaoColaboradores.Application.DTOs.Unidades;

namespace GestaoColaboradores.Application.Services;

public interface IUnidadeService
{
    Task<UnidadeResponseDto> CreateAsync(UnidadeCreateDto dto);
    Task<UnidadeResponseDto> UpdateStatusAsync(int id, UnidadeUpdateDto dto);
    Task<UnidadeResponseDto> GetByIdAsync(int id);
    Task<List<UnidadeResponseDto>> ListAsync();
}
