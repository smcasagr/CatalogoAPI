using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.Categorias
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
    }
}
