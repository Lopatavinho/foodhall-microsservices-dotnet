using Catalogo.Domain.Entidade;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infrastructure.Data;

public class CatalogoDbContext : DbContext
{
    public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Lojista> Lojistas { get; set; }
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lojista>().HasKey(l => l.Id);
        modelBuilder.Entity<Lojista>().Property(l => l.Nome).IsRequired().HasMaxLength(150);
        
        modelBuilder.Entity<Produto>().HasKey(p => p.Id);
        modelBuilder.Entity<Produto>().Property(p => p.Nome).IsRequired().HasMaxLength(150);
        modelBuilder.Entity<Produto>().Property(p => p.Preco).HasColumnType("decimal(18, 2)");
        
        // Relacionamento 1 (Lojista) para N (Produtos)
        modelBuilder.Entity<Produto>()
            .HasOne(p => p.Lojista)
            .WithMany(l => l.Produtos)
            .HasForeignKey(p => p.LojistaId);
    }
}