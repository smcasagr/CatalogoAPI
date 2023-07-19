using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using APICatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")] // habilita a proteção por bearer
    [Route("[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    //[EnableCors("PermitirApiRequest")] // Habilita o CORS via service
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork uof, IMapper mapper,
            IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _mapper = mapper;
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
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            var categoria = await _uof.CategoriaRepository.GetCategoriasProdutos();
            var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categoria);

            return categoriaDTO;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAll([FromQuery] CategoriasParameters categoriasParameters)
        {
            //var categoria = _uof.CategoriaRepository.Get().Take(10).ToList();
            var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));

            var categoriaDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

            return categoriaDTO;
        }

        /// <summary>
        /// Obtém uma categoria pelo seu Id
        /// </summary>
        /// <param name="id">Código da categoria</param>
        /// <returns>Objetos Categoria</returns>
        // [HttpGet("{id:alpha:length(5)}", Name = "BuscarCategoria")] - aceita somente - e estritamente - o número estipulado de caracteres
        [HttpGet("{id:int}", Name = "BuscarCategoria")]
        //[EnableCors("PermitirApiRequest")] // Habilita o CORS via service somente nesta Action
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            //throw new Exception("Erro ao busca a categoria pelo ID");
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(c => c.Id == id);
                if (categoria == null)
                {
                    return NotFound($"Categoria não encontrada - ID: {id}");
                }

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação no servidor!");
            }
        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST categorias
        ///     {
        ///         "id": 1,
        ///         "nome": "categoria1",
        ///         "imagemUrl: "categoria1.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoria">objeto Categoria</param>
        /// <returns>O objeto Categoria incluída</returns>
        /// <remarks>Retorna o objeto Categoria incluído</remarks>
        [HttpPost]
        public async Task<ActionResult> Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _uof.CategoriaRepository.Add(categoria); // persiste na memória
            await _uof.Commit(); // salva do BD

            // informa o produto salvo no header
            // Aciona a rota informada, com o ID informado
            return new CreatedAtRouteResult("BuscarCategoria",
                new { id = categoria.Id }, categoria);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                BadRequest();
            }

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            _uof.CategoriaRepository.Update(categoria);
            await _uof.Commit();

            return Ok(categoriaDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetById(c => c.Id == id);
            if (categoria is null)
            {
                return NotFound($"Categoria não localizada! - ID: {id}");
            }

            _uof.CategoriaRepository.Delete(categoria);
            await _uof.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);
        }

    }
}
