using APICatalogo.Context;
using APICatalogo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogoxUnitTests
{
    public class DBUnitTestsMockInitializer
    {
        public DBUnitTestsMockInitializer() { }

        public void Seed(AppDbContext context) 
        {
            context.Categorias.Add
            (new Categoria { Id = 999, Nome = "Bebidas999", ImagemUrl = "bebidas999.jpg" });

            context.Categorias.Add
            (new Categoria { Id = 2, Nome = "Sucos", ImagemUrl = "sucos1.jpg" });

            context.Categorias.Add
            (new Categoria { Id = 3, Nome = "Doces", ImagemUrl = "doces1.jpg" });

            context.Categorias.Add
            (new Categoria { Id = 4, Nome = "Salgados", ImagemUrl = "salgados1.jpg" });

            context.Categorias.Add
            (new Categoria { Id = 5, Nome = "Tortas", ImagemUrl = "tortas1.jpg" });

            context.Categorias.Add
            (new Categoria { Id = 6, Nome = "Bolos", ImagemUrl = "bolos1.jpg" });

            context.Categorias.Add
            (new Categoria { Id = 7, Nome = "Lanches", ImagemUrl = "lanches1.jpg" });

            context.SaveChanges();
        }
    }
}
