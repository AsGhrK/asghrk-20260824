namespace GestaoColaboradores.Application.DTOs.Usuarios;

public record UsuarioCreateDto(string Codigo, string Login, string Senha);

public record UsuarioUpdateDto(string? Senha, bool? Ativo);

public record UsuarioResponseDto(int Id, string Codigo, string Login, bool Ativo);
