using APICatalogo.Models;
using GraphQL.Types;

namespace APICatalogo.GraphQL
{
    // Definindo qual entidade será mapeada para o nosso Type
    public class CategoriaType : ObjectGraphType<Categoria>
    {
        public CategoriaType()
        {
            Field(x => x.Id);
            Field(x => x.Nome);
            Field(x => x.ImagemUrl);

            Field<ListGraphType<CategoriaType>>("categorias");
        }
    }
}
