using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository.Produtos
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(C => C.Preco).ToListAsync();
        }

        // NEcessário para se ter uma melhor performance na aplicação
        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
/*            return Get()
                .OrderBy(on => on.Nome)
                .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize) // separando os registros pelo tamanho definido
                .Take(produtosParameters.PageSize) // seleciona o n de registros correspondentes ao n da página
                .ToList();*/

            return PagedList<Produto>
                .ToPagedList(Get().OrderBy(on => on.Nome),
                             produtosParameters.PageNumber,
                             produtosParameters.PageSize);
        }
    }
}
