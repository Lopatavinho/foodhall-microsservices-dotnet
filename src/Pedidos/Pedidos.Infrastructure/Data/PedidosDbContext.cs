using Pedidos.Domain.Entidade;
using Microsoft.EntityFrameworkCore;

namespace Pedidos.Infrastructure.Data;

public class PedidosDbContext : DbContext
{
    public PedidosDbContext(DbContextOptions<PedidosDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>().HasKey(p => p.Id);
        modelBuilder.Entity<Pedido>().Property(p => p.ValorTotal).HasColumnType("decimal(18, 2)");
        
        modelBuilder.Entity<ItemPedido>().HasKey(ip => ip.Id);
        modelBuilder.Entity<ItemPedido>().Property(ip => ip.PrecoUnitario).HasColumnType("decimal(18, 2)");

        // Relacionamento 1 (Pedido) para N (ItensPedido)
        modelBuilder.Entity<Pedido>()
            .HasMany(p => p.Itens)
            .WithOne(ip => ip.Pedido)
            .HasForeignKey(ip => ip.PedidoId);
    }
}