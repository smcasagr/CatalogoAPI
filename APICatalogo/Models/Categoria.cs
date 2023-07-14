using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    [Table("Categorias")]
    public class Categoria : IValidatableObject
    {
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string? Nome { get; set; }

        [Required]
        [MaxLength(300)]
        public string? ImagemUrl { get; set; }

        [JsonIgnore]
        public ICollection<Produto>? Produtos { get; set; } // Necessário para definir a relação 1:n - Uma categoria pode ter n produtos.

        // Outra maneira de se criar uma validação personalizada, diretamente no modelo - Necesário implementar IValidatableObject
        // Desta maneira aqui, só é possível fazer a validação no modelo que a implementa - no caso específico, Categoria
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nome))
            {
                var primeiraLetra = this.Nome[0].ToString();
                if (primeiraLetra != primeiraLetra.ToUpper())
                {
                    // itera em todos os itens que apresentarem este problema
                    yield return new
                        ValidationResult("A primeira letra da categoria deve ser maiúscula!",
                            new[] { nameof(this.Nome) });
                }
            }
        }
    }
}