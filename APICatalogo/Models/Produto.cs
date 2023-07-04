using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models

{    
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(12,3)")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        //Necessário para definir a relação Categoria:Produto
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}