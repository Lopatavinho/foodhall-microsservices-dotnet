using Identidade.Domain.Entidade;
using Microsoft.EntityFrameworkCore;

namespace Identidade.Infrastructure.Data;

public class IdentidadeDbContext : DbContext
{
    public IdentidadeDbContext(DbContextOptions<IdentidadeDbContext> options)
        : base(options)
    {
    }

    // Mapeamento da Entidade para a tabela no DB
    public DbSet<Usuario> Usuarios { get; set; }

    // Configurações de Mapeamento (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasKey(u => u.Id); // Define PK
        
        modelBuilder.Entity<Usuario>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255); // Opcional, mas boa prática

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email) // Define índice único (UQ)
            .IsUnique();
    }
}