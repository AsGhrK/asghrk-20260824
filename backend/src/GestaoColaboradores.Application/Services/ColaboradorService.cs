using GestaoColaboradores.Application.Common.Exceptions;
using GestaoColaboradores.Application.DTOs.Colaboradores;
using GestaoColaboradores.Domain.Entities;
using GestaoColaboradores.Infrastructure.Repositories;

namespace GestaoColaboradores.Application.Services;

public class ColaboradorService : IColaboradorService
{
    private readonly IColaboradorRepository _colaboradorRepository;
    private readonly IUnidadeRepository _unidadeRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public ColaboradorService(
        IColaboradorRepository colaboradorRepository,
        IUnidadeRepository unidadeRepository,
        IUsuarioRepository usuarioRepository)
    {
        _colaboradorRepository = colaboradorRepository;
        _unidadeRepository = unidadeRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<ColaboradorResponseDto> CreateAsync(ColaboradorCreateDto dto)
    {
        if (await _colaboradorRepository.AnyAsync(c => c.Codigo == dto.Codigo))
        {
            throw new ConflictException($"Já existe um colaborador com o código '{dto.Codigo}'.");
        }

        var unidade = await _unidadeRepository.GetByIdAsync(dto.UnidadeId)
            ?? throw new NotFoundException($"Unidade {dto.UnidadeId} não encontrada.");

        if (!unidade.Ativo)
        {
            throw new BusinessRuleException("Unidade inativa não permite a inclusão de novos colaboradores.");
        }

        var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId)
            ?? throw new NotFoundException($"Usuário {dto.UsuarioId} não encontrado.");

        if (await _colaboradorRepository.AnyAsync(c => c.UsuarioId == dto.UsuarioId))
        {
            throw new ConflictException($"O usuário {dto.UsuarioId} já está relacionado a outro colaborador.");
        }

        var colaborador = new Colaborador
        {
            Codigo = dto.Codigo,
            Nome = dto.Nome,
            UnidadeId = unidade.Id,
            UsuarioId = usuario.Id,
        };

        await _colaboradorRepository.AddAsync(colaborador);
        await _colaboradorRepository.SaveChangesAsync();

        return ToDto(colaborador, unidade, usuario);
    }

    public async Task<ColaboradorResponseDto> UpdateAsync(int id, ColaboradorUpdateDto dto)
    {
        var colaborador = await _colaboradorRepository.GetByIdWithRelationsAsync(id)
            ?? throw new NotFoundException($"Colaborador {id} não encontrado.");

        var unidade = colaborador.UnidadeId == dto.UnidadeId
            ? colaborador.Unidade
            : await _unidadeRepository.GetByIdAsync(dto.UnidadeId)
                ?? throw new NotFoundException($"Unidade {dto.UnidadeId} não encontrada.");

        if (colaborador.UnidadeId != dto.UnidadeId && !unidade.Ativo)
        {
            throw new BusinessRuleException("Unidade inativa não permite a inclusão de novos colaboradores.");
        }

        colaborador.Nome = dto.Nome;
        colaborador.UnidadeId = unidade.Id;
        colaborador.Unidade = unidade;

        _colaboradorRepository.Update(colaborador);
        await _colaboradorRepository.SaveChangesAsync();

        return ToDto(colaborador, unidade, colaborador.Usuario);
    }

    public async Task DeleteAsync(int id)
    {
        var colaborador = await _colaboradorRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Colaborador {id} não encontrado.");

        _colaboradorRepository.Remove(colaborador);
        await _colaboradorRepository.SaveChangesAsync();
    }

    public async Task<ColaboradorResponseDto> GetByIdAsync(int id)
    {
        var colaborador = await _colaboradorRepository.GetByIdWithRelationsAsync(id)
            ?? throw new NotFoundException($"Colaborador {id} não encontrado.");

        return ToDto(colaborador, colaborador.Unidade, colaborador.Usuario);
    }

    public async Task<List<ColaboradorResponseDto>> ListAsync()
    {
        var colaboradores = await _colaboradorRepository.GetAllWithRelationsAsync();
        return colaboradores.Select(c => ToDto(c, c.Unidade, c.Usuario)).ToList();
    }

    private static ColaboradorResponseDto ToDto(Colaborador colaborador, Unidade unidade, Usuario usuario) => new(
        colaborador.Id,
        colaborador.Codigo,
        colaborador.Nome,
        unidade.Id,
        unidade.Nome,
        usuario.Id,
        usuario.Login);
}
