using APICatalogo.Repository.Categorias;
using APICatalogo.Repository.Produtos;

namespace APICatalogo.Repository
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }

        Task Commit(); // método que irá persistir os dados
    }
}
