namespace Pedidos.Domain.Entidade;

public class ItemPedido
{
    public Guid Id { get; set; }
    public Guid PedidoId { get; set; } // FK
    public Guid ProdutoId { get; set; } // ID do Produto (vindo de Catálogo)
    public string NomeProduto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }

    // Propriedade de navegação
    public Pedido Pedido { get; set; }
}