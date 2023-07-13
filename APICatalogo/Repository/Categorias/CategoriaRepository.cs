using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository.Categorias
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(c => c.Produtos).AsNoTracking();
        }
    }
}
