using GestaoColaboradores.Application.Auth;
using GestaoColaboradores.Application.Common.Exceptions;
using GestaoColaboradores.Application.DTOs.Auth;
using GestaoColaboradores.Application.DTOs.Usuarios;
using GestaoColaboradores.Application.Services;
using GestaoColaboradores.Infrastructure.Repositories;
using GestaoColaboradores.Tests.TestHelpers;
using Microsoft.Extensions.Options;
using Xunit;

namespace GestaoColaboradores.Tests.Services;

public class AuthServiceTests
{
    private static JwtTokenService CreateJwtTokenService() =>
        new(Options.Create(new JwtOptions
        {
            Key = "chave-de-teste-com-pelo-menos-32-caracteres!",
            Issuer = "test-issuer",
            Audience = "test-audience",
            ExpiryMinutes = 60,
        }));

    [Fact]
    public async Task LoginAsync_ComUsuarioInativo_LancaUnauthorizedException()
    {
        var context = InMemoryDbContextFactory.Create();
        var usuarioRepository = new UsuarioRepository(context);
        var usuarioService = new UsuarioService(usuarioRepository);
        var authService = new AuthService(usuarioRepository, CreateJwtTokenService());

        var usuario = await usuarioService.CreateAsync(new UsuarioCreateDto("U1", "jdoe", "senha123"));
        await usuarioService.UpdateAsync(usuario.Id, new UsuarioUpdateDto(Senha: null, Ativo: false));

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => authService.LoginAsync(new LoginDto("jdoe", "senha123")));
    }

    [Fact]
    public async Task LoginAsync_ComCredenciaisValidas_RetornaToken()
    {
        var context = InMemoryDbContextFactory.Create();
        var usuarioRepository = new UsuarioRepository(context);
        var usuarioService = new UsuarioService(usuarioRepository);
        var authService = new AuthService(usuarioRepository, CreateJwtTokenService());

        await usuarioService.CreateAsync(new UsuarioCreateDto("U2", "msilva", "senha123"));

        var token = await authService.LoginAsync(new LoginDto("msilva", "senha123"));

        Assert.False(string.IsNullOrWhiteSpace(token.Token));
    }

    [Fact]
    public async Task LoginAsync_ComSenhaInvalida_LancaUnauthorizedException()
    {
        var context = InMemoryDbContextFactory.Create();
        var usuarioRepository = new UsuarioRepository(context);
        var usuarioService = new UsuarioService(usuarioRepository);
        var authService = new AuthService(usuarioRepository, CreateJwtTokenService());

        await usuarioService.CreateAsync(new UsuarioCreateDto("U3", "csantos", "senha123"));

        await Assert.ThrowsAsync<UnauthorizedException>(
            () => authService.LoginAsync(new LoginDto("csantos", "senha-errada")));
    }
}
