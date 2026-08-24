using GestaoColaboradores.Application.DTOs.Usuarios;
using GestaoColaboradores.Application.Services;
using GestaoColaboradores.Infrastructure.Repositories;
using GestaoColaboradores.Tests.TestHelpers;
using Xunit;

namespace GestaoColaboradores.Tests.Services;

public class UsuarioServiceTests
{
    [Fact]
    public async Task UpdateAsync_AlteraApenasSenhaEStatus_MantendoLoginECodigo()
    {
        var context = InMemoryDbContextFactory.Create();
        var service = new UsuarioService(new UsuarioRepository(context));

        var criado = await service.CreateAsync(new UsuarioCreateDto("U1", "jdoe", "senha123"));

        var atualizado = await service.UpdateAsync(criado.Id, new UsuarioUpdateDto(Senha: null, Ativo: false));

        Assert.Equal("jdoe", atualizado.Login);
        Assert.Equal("U1", atualizado.Codigo);
        Assert.False(atualizado.Ativo);
    }

    [Fact]
    public async Task ListAsync_ComFiltroDeStatus_RetornaSomenteUsuariosCorrespondentes()
    {
        var context = InMemoryDbContextFactory.Create();
        var service = new UsuarioService(new UsuarioRepository(context));

        var ativo = await service.CreateAsync(new UsuarioCreateDto("U2", "ativo", "senha123"));
        var inativoDto = await service.CreateAsync(new UsuarioCreateDto("U3", "inativo", "senha123"));
        await service.UpdateAsync(inativoDto.Id, new UsuarioUpdateDto(Senha: null, Ativo: false));

        var ativos = await service.ListAsync(ativo: true);

        Assert.Contains(ativos, u => u.Id == ativo.Id);
        Assert.DoesNotContain(ativos, u => u.Id == inativoDto.Id);
    }
}
