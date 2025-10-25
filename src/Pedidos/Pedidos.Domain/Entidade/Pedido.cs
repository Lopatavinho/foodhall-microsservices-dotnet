namespace Pedidos.Domain.Entidade;

public class Pedido
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; } // ID do Cliente (vindo de Identidade)
    public DateTime DataPedido { get; set; }
    public string Status { get; set; } // Ex: "Pendente", "Aprovado", "Entregue"
    public decimal ValorTotal { get; set; }
    
    // Propriedade de navegação
    public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}