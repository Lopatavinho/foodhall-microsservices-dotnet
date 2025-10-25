namespace Catalogo.Domain.Entidade;

public class Lojista
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Categoria { get; set; }
    public List<Produto> Produtos { get; set; } = new List<Produto>();
}