using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    [Table("Categorias")]
    public class Categoria
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

        public ICollection<Produto>? Produtos { get; set; } // Necessário para definir a relação 1:n - Uma categoria pode ter n produtos.
    }
}