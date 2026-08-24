using GestaoColaboradores.Application.Common.Exceptions;
using GestaoColaboradores.Application.DTOs.Usuarios;
using GestaoColaboradores.Domain.Entities;
using GestaoColaboradores.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace GestaoColaboradores.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioResponseDto> CreateAsync(UsuarioCreateDto dto)
    {
        if (await _usuarioRepository.AnyAsync(u => u.Codigo == dto.Codigo))
        {
            throw new ConflictException($"Já existe um usuário com o código '{dto.Codigo}'.");
        }

        if (await _usuarioRepository.GetByLoginAsync(dto.Login) is not null)
        {
            throw new ConflictException($"Já existe um usuário com o login '{dto.Login}'.");
        }

        var usuario = new Usuario
        {
            Codigo = dto.Codigo,
            Login = dto.Login,
            Ativo = true,
        };
        usuario.SenhaHash = _passwordHasher.HashPassword(usuario, dto.Senha);

        await _usuarioRepository.AddAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();

        return ToDto(usuario);
    }

    public async Task<UsuarioResponseDto> UpdateAsync(int id, UsuarioUpdateDto dto)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Usuário {id} não encontrado.");

        // Regra de negócio: apenas senha e status podem ser alterados.
        if (!string.IsNullOrWhiteSpace(dto.Senha))
        {
            usuario.SenhaHash = _passwordHasher.HashPassword(usuario, dto.Senha);
        }

        if (dto.Ativo.HasValue)
        {
            usuario.Ativo = dto.Ativo.Value;
        }

        _usuarioRepository.Update(usuario);
        await _usuarioRepository.SaveChangesAsync();

        return ToDto(usuario);
    }

    public async Task<UsuarioResponseDto> GetByIdAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Usuário {id} não encontrado.");

        return ToDto(usuario);
    }

    public async Task<List<UsuarioResponseDto>> ListAsync(bool? ativo)
    {
        var usuarios = ativo.HasValue
            ? await _usuarioRepository.GetByStatusAsync(ativo.Value)
            : await _usuarioRepository.GetAllAsync();

        return usuarios.Select(ToDto).ToList();
    }

    private static UsuarioResponseDto ToDto(Usuario usuario) =>
        new(usuario.Id, usuario.Codigo, usuario.Login, usuario.Ativo);
}
