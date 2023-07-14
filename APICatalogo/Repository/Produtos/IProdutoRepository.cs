using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.Produtos
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        // Além dos métodos constantes na interface genérica
        // Quero este método em específico, por exemplo
        Task<IEnumerable<Produto>> GetProdutosPorPreco();
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
    }
}
