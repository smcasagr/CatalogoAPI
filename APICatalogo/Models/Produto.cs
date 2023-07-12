using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models

{    
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80)]
        [PrimeiraLetraMaiuscula] // Validação personalizada
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? Descricao { get; set; }

        [Required]
        [Range(0.01,10000, ErrorMessage = "O preço deve estar entre {1} e {2}!")]
        [Column(TypeName = "decimal(12,3)")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        //Necessário para definir a relação Categoria:Produto
        public int CategoriaId { get; set; }

        // Ignora esta propriedade na hora de serializar o objeto
        [JsonIgnore]
        public Categoria? Categoria { get; set; }
    }
}