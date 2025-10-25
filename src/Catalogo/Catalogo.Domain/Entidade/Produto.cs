namespace Catalogo.Domain.Entidade;

public class Produto
{
    public Guid Id { get; set; }
    public Guid LojistaId { get; set; } // FK
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    
    // Propriedade de navegação
    public Lojista Lojista { get; set; }
}