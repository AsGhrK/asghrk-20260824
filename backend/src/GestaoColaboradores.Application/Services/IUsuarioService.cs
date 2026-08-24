using GestaoColaboradores.Application.DTOs.Usuarios;

namespace GestaoColaboradores.Application.Services;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto);
    Task<UsuarioResponseDto> UpdateAsync(int id, UsuarioUpdateDto dto);
    Task<UsuarioResponseDto> GetByIdAsync(int id);
    Task<List<UsuarioResponseDto>> ListAsync(bool? ativo);
}
