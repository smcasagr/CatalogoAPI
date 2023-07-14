using System.Linq.Expressions;

namespace APICatalogo.Repository
{
    // Interface genérica
    public interface IRepository<T>
    {
        // utilizado o tipo IQueryable pois desta forma será possível customizar as consultas
        /*
         * Ao usar IQueryable, uma consulta é criada para a fonte de dados, sendo que a consulta 
         * é executada apenas quando o IQueryable for enumerado, ou seja, quando ToList ou foreach
         * for chamado. Isso permite que você crie consultas usando Where, OrderBy ou Select sem nunca
         * ter que acessar o banco de dados. Se tivéssemos que acessar o banco de dados várias vezes, 
         * você pode imaginar como isso ia ficar lento. - Explicação Macoratti
         */
        IQueryable<T> Get();
        Task<T> GetById(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
