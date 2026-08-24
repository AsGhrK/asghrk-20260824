using GestaoColaboradores.Application.Auth;
using GestaoColaboradores.Application.Common.Exceptions;
using GestaoColaboradores.Application.DTOs.Auth;
using GestaoColaboradores.Domain.Entities;
using GestaoColaboradores.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace GestaoColaboradores.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public AuthService(IUsuarioRepository usuarioRepository, IJwtTokenService jwtTokenService)
    {
        _usuarioRepository = usuarioRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarioRepository.GetByLoginAsync(dto.Login)
            ?? throw new UnauthorizedException("Login ou senha inválidos.");

        if (!usuario.Ativo)
        {
            throw new UnauthorizedException("Usuário inativo.");
        }

        var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, dto.Senha);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Login ou senha inválidos.");
        }

        return _jwtTokenService.GenerateToken(usuario);
    }
}
