using GestaoColaboradores.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoColaboradores.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Unidade> Unidades => Set<Unidade>();
    public DbSet<Colaborador> Colaboradores => Set<Colaborador>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.Codigo).IsUnique();
            entity.HasIndex(u => u.Login).IsUnique();
            entity.Property(u => u.Login).IsRequired().HasMaxLength(100);
            entity.Property(u => u.SenhaHash).IsRequired();
        });

        modelBuilder.Entity<Unidade>(entity =>
        {
            entity.HasIndex(u => u.Codigo).IsUnique();
            entity.Property(u => u.Nome).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Colaborador>(entity =>
        {
            entity.HasIndex(c => c.Codigo).IsUnique();
            entity.Property(c => c.Nome).IsRequired().HasMaxLength(150);

            entity.HasOne(c => c.Unidade)
                .WithMany(u => u.Colaboradores)
                .HasForeignKey(c => c.UnidadeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Usuario)
                .WithOne(u => u.Colaborador)
                .HasForeignKey<Colaborador>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(c => c.UsuarioId).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        AtualizarTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AtualizarTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AtualizarTimestamps()
    {
        foreach (var entry in ChangeTracker.Entries<Domain.Common.EntidadeBase>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CriadoEm = DateTime.UtcNow;
                entry.Entity.AtualizadoEm = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.AtualizadoEm = DateTime.UtcNow;
            }
        }
    }
}
