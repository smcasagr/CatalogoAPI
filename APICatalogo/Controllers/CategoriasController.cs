using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            // Busca todas as categorias e seus produtos
            return _context.Categorias.Include(p => p.Produtos).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            return _context.Categorias.ToList();
        }

        [HttpGet("{id:int}", Name = "BuscarCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(x => x.Id == id);
            if (categoria == null)
            {
                return NotFound($"Categoria não encontrada - ID: {id}");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias.Add(categoria); // persiste na memória
            _context.SaveChanges(); // salva do BD

            // informa o produto salvo no header
            // Aciona a rota informada, com o ID informado
            return new CreatedAtRouteResult("BuscarCategoria",
                new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified; // informa que a entidade foi modificada e deve ser persistida
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.Id == id);
            if (categoria is null)
            {
                return NotFound($"Categoria não localizada! - ID: {id}");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }

    }
}
