using GestaoColaboradores.Application.DTOs.Auth;
using GestaoColaboradores.Domain.Entities;

namespace GestaoColaboradores.Application.Auth;

public interface IJwtTokenService
{
    TokenResponseDto GenerateToken(Usuario usuario);
}
