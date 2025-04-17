namespace APICatalogo.Models;

public class Produto
{
    //informa chave primaria tendo o Id na propriedade
    public int ProdutoId { get; set; }
    public string Nome { get; set; }
    public string NomeDescricao { get; set; }
    public string Descricao {  get; set; }
    public decimal Preco {  get; set; }
    public string ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    //mapeia a coluna de categoria
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
}
