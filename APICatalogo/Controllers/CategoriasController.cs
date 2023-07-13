using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repository;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork uof, IConfiguration configuration,
            ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _configuration = configuration;
            _logger = logger;
        }

        // Lendo dados do arquivo appsettings.json
        [HttpGet("autor")]
        public string GetAutor()
        {
            var autor = _configuration["Autor"];
            var conn = _configuration["ConnectionStrings:DefaultConnection"];

            return $"Autor : {autor}\nConexão DB: {conn}";
        }

        // Exemplo de utilização de Service criado
        [HttpGet("saudacao/{nome=Samuel}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("catprodutos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            return _uof.CategoriaRepository.Get().Take(10).ToList();
        }

        // [HttpGet("{id:alpha:length(5)}", Name = "BuscarCategoria")] - aceita somente - e estritamente - o número estipulado de caracteres
        [HttpGet("{id:int}", Name = "BuscarCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            //throw new Exception("Erro ao busca a categoria pelo ID");
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(c => c.Id == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria não encontrada - ID: {id}");
                }

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação no servidor!");
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _uof.CategoriaRepository.Add(categoria); // persiste na memória
            _uof.Commit(); // salva do BD

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

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.Id == id);
            if (categoria is null)
            {
                return NotFound($"Categoria não localizada! - ID: {id}");
            }

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            return Ok(categoria);
        }

    }
}
