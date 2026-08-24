using GestaoColaboradores.Application.DTOs.Colaboradores;

namespace GestaoColaboradores.Application.Services;

public interface IColaboradorService
{
    Task<ColaboradorResponseDto> CreateAsync(ColaboradorCreateDto dto);
    Task<ColaboradorResponseDto> UpdateAsync(int id, ColaboradorUpdateDto dto);
    Task DeleteAsync(int id);
    Task<ColaboradorResponseDto> GetByIdAsync(int id);
    Task<List<ColaboradorResponseDto>> ListAsync();
}
