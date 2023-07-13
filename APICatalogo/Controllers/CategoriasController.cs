using APICatalogo.Context;
using APICatalogo.Models;
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
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, IConfiguration configuration,
            ILogger<CategoriasController> logger)
        {
            _context = context;
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

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
        {
            _logger.LogInformation(" ========== GET categorias/produtos ========== ");
            // Busca todas as categorias e seus produtos
            //return _context.Categorias.Include(p => p.Produtos).ToList();
            // como não há alterações a serem feitas, não é necessário rastrear (guardar as infos em cache)
            return await _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToListAsync();

            // O mais correto é nunca retornar uma busca sem algum filtro, como acima
            // Numa aplicação real, melhor fazer como abaixo
            // return _context.Categorias.Include(p => p.Produtos).Where(c => c.Id <= 5).AsNoTracking().ToList();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAll()
        {
            //return _context.Categorias.ToList();            
            return await _context.Categorias.AsNoTracking().ToListAsync();

            // No ambiente real, é sempre melhor retornar um número específico de registros
            // Nunca usar todos como acima.
            // return _context.Categorias.Take(10).AsNoTracking().ToList();
        }

        // [HttpGet("{id:alpha}", Name = "BuscarCategoria")] - alpha: caso queira restringir os parâmetros a caracteres alfanuméricos
        // [HttpGet("{id:alpha:length(5)}", Name = "BuscarCategoria")] - aceita somente - e estritamente - o número estipulado de caracteres
        [HttpGet("{id:int}", Name = "BuscarCategoria")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            _logger.LogInformation($" ========== GET categorias/id = {id} ========== ");
            // Teste de exceção personalizada
            //throw new Exception("Erro ao busca a categoria pelo ID");
            try
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
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
