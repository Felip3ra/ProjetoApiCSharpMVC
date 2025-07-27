using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;
[Table("Categorias")]
public class Categoria
{
    public Categoria()
    {
        //inicializa a colecao da classe
        Produtos = new Collection<Produto>();
    }
    //informa chave primaria tendo o Id na propriedade
    [Key]
    public int CategoriaId { get; set; }
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }
    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}
