using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ProdutoDTO
{
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

    // Chave estrangeira que aponta para a Categoria associada ao produto
    public int CategoriaId { get; set; }
}
