// Importa o namespace com classes para validação de dados em modelos
using System.ComponentModel.DataAnnotations;

// Define o namespace onde esta classe está localizada, organizando o projeto
namespace APICatalogo.Validation;

// Declaração da classe PrimeiraLetraMaiusculaAttribute que herda de ValidationAttribute
// Isso permite usar a classe como um atributo de validação personalizado
public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
{
    // Sobrescreve o método IsValid para implementar a lógica de validação personalizada
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Verifica se o valor é nulo ou uma string vazia
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            // Se for vazio ou nulo, considera a validação como sucesso
            return ValidationResult.Success;
        }

        // Pega o primeiro caractere do valor e converte para string
        var primeiraLetra = value.ToString()[0].ToString();

        // Verifica se a primeira letra NÃO é maiúscula comparando com sua versão maiúscula
        if (primeiraLetra != primeiraLetra.ToUpper())
        {
            // Se não for maiúscula, retorna uma mensagem de erro personalizada
            return new ValidationResult("A primeira Letra do nome do produto deve ser maiuscula");
        }

        // Se tudo estiver correto, retorna sucesso na validação
        return ValidationResult.Success;
    }
}