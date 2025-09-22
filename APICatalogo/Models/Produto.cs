// Importa o namespace onde está definido o atributo personalizado [PrimeiraLetraMaiuscula]
using APICatalogo.Validation;

// Fornece atributos e classes para validação de modelos (ex: [Required], [StringLength])
using System.ComponentModel.DataAnnotations;

// Fornece atributos para mapeamento objeto-relacional (ex: [Key], [Table])
using System.ComponentModel.DataAnnotations.Schema;

// Permite controlar a serialização JSON, como ignorar propriedades no retorno da API
using System.Text.Json.Serialization;

// Define o namespace lógico do projeto para organizar as classes
namespace APICatalogo.Models;

// Mapeia esta classe para uma tabela chamada "Produtos" no banco de dados
[Table("Produtos")]
// Implementa IValidatableObject para adicionar validações customizadas além dos atributos
public class Produto : IValidatableObject
{
    // Indica que ProdutoId é a chave primária da entidade
    [Key]
    public int ProdutoId { get; set; }

    // Valida que o campo Nome é obrigatório
    [Required(ErrorMessage = "O nome é obrigatório")]

    // Limita o tamanho máximo do nome a 80 caracteres
    [StringLength(80)]

    // Garante que o nome comece com letra maiúscula (atributo personalizado)
    //[PrimeiraLetraMaiuscula]
    public string Nome { get; set; }

    // Valida que a descrição é obrigatória
    [Required]

    // Limita a descrição entre 5 e 10 caracteres e define mensagem personalizada
    [StringLength(10, ErrorMessage = "A descricao deve conter no maximo {1} caracteres", MinimumLength = 5)]
    public string? Descricao { get; set; } // Propriedade opcional

    // Valida que o preço é obrigatório
    [Required]

    // Garante que o preço esteja entre 1 e 1000, com mensagem personalizada
    [Range(1, 1000, ErrorMessage = "O preco deve estar entre {1} e {2}")]
    public decimal Preco { get; set; }

    // Valida que a imagemUrl é obrigatória
    [Required]

    // Garante que a URL tenha entre 10 e 300 caracteres
    [StringLength(300, MinimumLength = 10)]
    public string? ImagemUrl { get; set; } // Propriedade opcional

    // Representa a quantidade disponível em estoque
    public float Estoque { get; set; }

    // Armazena a data/hora em que o produto foi cadastrado
    public DateTime DataCadastro { get; set; }

    // Chave estrangeira que aponta para a Categoria associada ao produto
    public int CategoriaId { get; set; }

    // Impede que a propriedade seja incluída na resposta JSON (evita loops de serialização)
    [JsonIgnore]
    public CategoriaDTO? Categoria { get; set; } // Propriedade de navegação

    // Método da interface IValidatableObject para validações personalizadas
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Só valida se o nome não for vazio ou nulo
        if (!string.IsNullOrEmpty(this.Nome))
        {
            // Pega a primeira letra do nome
            var primeiraLetra = this.Nome[0].ToString();

            // Verifica se a primeira letra NÃO é maiúscula
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                // Retorna um erro de validação indicando a propriedade inválida
                yield return new ValidationResult(
                    "A primeira Letra do produto deve ser maiúscula",
                    new[] { nameof(this.Nome) });
            }

            if (this.Estoque <= 0)
            {
                yield return new ValidationResult(
                    "O estoque deve ser maior que zero",
                    new[] { nameof(this.Nome) });
            }
        }
    }
}