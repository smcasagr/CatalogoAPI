using APICatalogo.Models;

namespace APICatalogo.Repository.Categorias
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
