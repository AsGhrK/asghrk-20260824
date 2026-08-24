namespace GestaoColaboradores.Application.DTOs.Auth;

public record LoginDto(string Login, string Senha);

public record TokenResponseDto(string Token, DateTime ExpiresAt);
