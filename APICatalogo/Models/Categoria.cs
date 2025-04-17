using System.Collections.ObjectModel;

namespace APICatalogo.Models;

public class Categoria
{
    public Categoria()
    {
        //inicializa a colecao da classe
        Produtos = new Collection<Produto>();
    }
    //informa chave primaria tendo o Id na propriedade
    public int CategoriaId { get; set; }
    public string? Nome { get; set; }
    public string? ImagemUrl { get; set; }
    public ICollection<Produto>? Produtos { get; set; }
}
