using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Produtos")]
public class Produto
{
    //informa chave primaria tendo o Id na propriedade
    [Key]
    public int ProdutoId { get; set; }
    [Required]
    [StringLength(80)]
    public string Nome { get; set; }
    [Required]
    [StringLength(80)]
    public string? Descricao {  get; set; }
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco {  get; set; }
    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }
    //mapeia a coluna de categoria
    public int CategoriaId { get; set; }
    [JsonIgnore] //ignora a serializacao
    public Categoria? Categoria { get; set; }

}
