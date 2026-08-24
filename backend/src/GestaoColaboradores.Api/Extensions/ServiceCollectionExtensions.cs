using GestaoColaboradores.Application.Auth;
using GestaoColaboradores.Application.Services;
using GestaoColaboradores.Infrastructure.Data;
using GestaoColaboradores.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IUnidadeRepository, UnidadeRepository>();
        services.AddScoped<IColaboradorRepository, ColaboradorRepository>();

        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IUnidadeService, UnidadeService>();
        services.AddScoped<IColaboradorService, ColaboradorService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}
