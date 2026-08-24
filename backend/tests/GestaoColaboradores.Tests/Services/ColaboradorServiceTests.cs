using GestaoColaboradores.Application.Common.Exceptions;
using GestaoColaboradores.Application.DTOs.Colaboradores;
using GestaoColaboradores.Application.Services;
using GestaoColaboradores.Domain.Entities;
using GestaoColaboradores.Infrastructure.Repositories;
using GestaoColaboradores.Tests.TestHelpers;
using Xunit;

namespace GestaoColaboradores.Tests.Services;

public class ColaboradorServiceTests
{
    [Fact]
    public async Task CreateAsync_ComUnidadeInativa_LancaBusinessRuleException()
    {
        var context = InMemoryDbContextFactory.Create();
        var unidade = new Unidade { Codigo = "U1", Nome = "Filial Centro", Ativo = false };
        var usuario = new Usuario { Codigo = "US1", Login = "jdoe", SenhaHash = "hash", Ativo = true };
        context.Unidades.Add(unidade);
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();

        var service = new ColaboradorService(
            new ColaboradorRepository(context),
            new UnidadeRepository(context),
            new UsuarioRepository(context));

        var dto = new ColaboradorCreateDto("C1", "João Silva", unidade.Id, usuario.Id);

        await Assert.ThrowsAsync<BusinessRuleException>(() => service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ComUnidadeAtiva_CriaColaboradorComSucesso()
    {
        var context = InMemoryDbContextFactory.Create();
        var unidade = new Unidade { Codigo = "U2", Nome = "Filial Sul", Ativo = true };
        var usuario = new Usuario { Codigo = "US2", Login = "msilva", SenhaHash = "hash", Ativo = true };
        context.Unidades.Add(unidade);
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();

        var service = new ColaboradorService(
            new ColaboradorRepository(context),
            new UnidadeRepository(context),
            new UsuarioRepository(context));

        var dto = new ColaboradorCreateDto("C2", "Maria Silva", unidade.Id, usuario.Id);

        var resultado = await service.CreateAsync(dto);

        Assert.Equal("C2", resultado.Codigo);
        Assert.Equal(unidade.Id, resultado.UnidadeId);
        Assert.Equal(usuario.Id, resultado.UsuarioId);
    }
}
