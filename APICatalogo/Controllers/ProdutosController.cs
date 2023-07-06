using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context; // injetando a instância de db context no controlador

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetAll()
        {
            var produtos = _context.Produtos.ToList();
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados!");
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name="BuscarProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);
            if (produto == null) 
            {
                return NotFound($"Produto não encontrado - ID: {id}");
            }

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            _context.Produtos.Add(produto); // persiste na memória
            _context.SaveChanges(); // salva do BD

            // informa o produto salvo no header
            // Aciona a rota informada, com o ID informado
            return new CreatedAtRouteResult("BuscarProduto",
                new { id = produto.Id }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.Id)
            {
                BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified; // informa que a entidade foi modificada e deve ser persistida
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        { 
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto is null )
            {
                return NotFound($"Produto não localizado! - ID: {id}");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
