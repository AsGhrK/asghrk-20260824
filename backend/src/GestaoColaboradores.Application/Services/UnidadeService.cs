using GestaoColaboradores.Application.Common.Exceptions;
using GestaoColaboradores.Application.DTOs.Colaboradores;
using GestaoColaboradores.Application.DTOs.Unidades;
using GestaoColaboradores.Domain.Entities;
using GestaoColaboradores.Infrastructure.Repositories;

namespace GestaoColaboradores.Application.Services;

public class UnidadeService : IUnidadeService
{
    private readonly IUnidadeRepository _unidadeRepository;

    public UnidadeService(IUnidadeRepository unidadeRepository)
    {
        _unidadeRepository = unidadeRepository;
    }

    public async Task<UnidadeResponseDto> CreateAsync(UnidadeCreateDto dto)
    {
        if (await _unidadeRepository.AnyAsync(u => u.Codigo == dto.Codigo))
        {
            throw new ConflictException($"Já existe uma unidade com o código '{dto.Codigo}'.");
        }

        var unidade = new Unidade
        {
            Codigo = dto.Codigo,
            Nome = dto.Nome,
            Ativo = true,
        };

        await _unidadeRepository.AddAsync(unidade);
        await _unidadeRepository.SaveChangesAsync();

        return ToDto(unidade);
    }

    public async Task<UnidadeResponseDto> UpdateStatusAsync(int id, UnidadeUpdateDto dto)
    {
        var unidade = await _unidadeRepository.GetByIdWithColaboradoresAsync(id)
            ?? throw new NotFoundException($"Unidade {id} não encontrada.");

        unidade.Ativo = dto.Ativo;

        _unidadeRepository.Update(unidade);
        await _unidadeRepository.SaveChangesAsync();

        return ToDto(unidade);
    }

    public async Task<UnidadeResponseDto> GetByIdAsync(int id)
    {
        var unidade = await _unidadeRepository.GetByIdWithColaboradoresAsync(id)
            ?? throw new NotFoundException($"Unidade {id} não encontrada.");

        return ToDto(unidade);
    }

    public async Task<List<UnidadeResponseDto>> ListAsync()
    {
        var unidades = await _unidadeRepository.GetAllWithColaboradoresAsync();
        return unidades.Select(ToDto).ToList();
    }

    private static UnidadeResponseDto ToDto(Unidade unidade) => new(
        unidade.Id,
        unidade.Codigo,
        unidade.Nome,
        unidade.Ativo,
        unidade.Colaboradores.Select(c => new ColaboradorSummaryDto(c.Id, c.Codigo, c.Nome)).ToList());
}
