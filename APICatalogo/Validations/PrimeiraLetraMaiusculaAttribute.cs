using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations
{
    public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
    {
        // Valida se o valor passado como nome tem a primeira letra maiúscula
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var primeiraLetra = value.ToString()[0].ToString(); // Pega a primeira letra
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula!");
            }

            return ValidationResult.Success;
        }
    }
}
