using APICatalogo.Models;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class ProdutoDTOUpdateResponse
    {
        
        public int ProdutoId { get; set; }

     

        // Garante que o nome comece com letra maiúscula (atributo personalizado)
        //[PrimeiraLetraMaiuscula]
        public string Nome { get; set; }

        // Valida que a descrição é obrigatória
        
        public string? Descricao { get; set; } // Propriedade opcional

        // Valida que o preço é obrigatório
        
        public decimal Preco { get; set; }

        // Valida que a imagemUrl é obrigatória
        
        public string? ImagemUrl { get; set; } // Propriedade opcional

        // Representa a quantidade disponível em estoque
        public float Estoque { get; set; }

        // Armazena a data/hora em que o produto foi cadastrado
        public DateTime DataCadastro { get; set; }

        // Chave estrangeira que aponta para a Categoria associada ao produto
        public int CategoriaId { get; set; }

        
        public Categoria? Categoria { get; set; } // Propriedade de navegação

        
    }
}
