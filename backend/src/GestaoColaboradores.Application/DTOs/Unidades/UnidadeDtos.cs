using GestaoColaboradores.Application.DTOs.Colaboradores;

namespace GestaoColaboradores.Application.DTOs.Unidades;

public record UnidadeCreateDto(string Codigo, string Nome);

public record UnidadeUpdateDto(bool Ativo);

public record UnidadeResponseDto(
    int Id,
    string Codigo,
    string Nome,
    bool Ativo,
    List<ColaboradorSummaryDto> Colaboradores);
