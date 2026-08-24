namespace GestaoColaboradores.Application.DTOs.Colaboradores;

public record ColaboradorCreateDto(string Codigo, string Nome, int UnidadeId, int UsuarioId);

public record ColaboradorUpdateDto(string Nome, int UnidadeId);

public record ColaboradorResponseDto(
    int Id,
    string Codigo,
    string Nome,
    int UnidadeId,
    string UnidadeNome,
    int UsuarioId,
    string UsuarioLogin);

public record ColaboradorSummaryDto(int Id, string Codigo, string Nome);
