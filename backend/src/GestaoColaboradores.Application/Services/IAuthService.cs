using GestaoColaboradores.Application.DTOs.Auth;

namespace GestaoColaboradores.Application.Services;

public interface IAuthService
{
    Task<TokenResponseDto> LoginAsync(LoginDto dto);
}
