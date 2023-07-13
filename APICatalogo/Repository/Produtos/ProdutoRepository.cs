using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repository.Produtos
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(C => C.Preco).ToList();
        }
    }
}
