using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GestaoColaboradores.Infrastructure.Data;

/// <summary>
/// Permite `dotnet ef migrations add` sem precisar subir a Api (nem uma conexão real com o banco).
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("GESTAO_DB_CONNECTION")
            ?? "Host=localhost;Port=5432;Database=gestao_colaboradores;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
