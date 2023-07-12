using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations
{
    // Pode ser utiizado em qualquer modelo
    // É mais genérico e versátil
    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
    {
        // Valida se o valor passado como nome tem a primeira letra maiúscula
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var modelo = validationContext.ObjectInstance.GetType().Name;

            var primeiraLetra = value.ToString()[0].ToString(); // Pega a primeira letra
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                return new ValidationResult($"A primeira letra de {modelo} deve ser maiúscula!");
            }

            return ValidationResult.Success;
        }
    }
}
