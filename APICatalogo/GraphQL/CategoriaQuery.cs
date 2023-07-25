using APICatalogo.Repository;
using GraphQL;
using GraphQL.Types;

namespace APICatalogo.GraphQL
{
    // Mapeamos os campos para uma dada consulta
    // para uma chamada no repositório CategoriasRepository
    public class CategoriaQuery : ObjectGraphType
    {
        // recebe a instância do nosso UnitOfWork que
        // contém as instâncias dos repositórios
        public CategoriaQuery(IUnitOfWork _context)
        {
            // este método vai retornar um objeto categoria
            Field<CategoriaType>("categoria",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType>() { Name = "id" }),
                    resolve: context =>
                    {
                        var id = context.GetArgument<int>("id");
                        return _context.CategoriaRepository
                                       .GetById(c => c.Id == id);
                    });

            // método que retorna uma lista de objetos categoria
            // aqui o resolve irá mapear a requisição do cliente
            // com os dados da consulta GET definida em CategoriaRepository
            Field<ListGraphType<CategoriaType>>("categorias",
                resolve: context =>
                {
                    return _context.CategoriaRepository.Get();
                });
        }
    }
}
