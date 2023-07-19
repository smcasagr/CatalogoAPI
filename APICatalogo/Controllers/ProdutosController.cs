using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
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
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof; // injetando a instância de db context no controlador
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos() 
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAll([FromQuery] ProdutosParameters produtosParameters)
        {
            //var produtos = _uof.ProdutoRepository.Get().ToList();
            //var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters).ToList();
            var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);

            // Passando as informações de paginação para o header
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));


            if (produtos is null)
            {
                return NotFound("Produtos não encontrados!");
            }

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTO;
        }

        [HttpGet("{id:int}", Name="BuscarProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            // throw new Exception("Erro ao busca os produtos pelo ID");
            try
            {
                var produto = await _uof.ProdutoRepository.GetById(p => p.Id == id);
                if (produto == null)
                {
                    return NotFound($"Produto não encontrado - ID: {id}");
                }

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
                
                return produtoDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação no servidor!");
            }            
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO); // faz o inverso - mapeia de DTO para a entity

            if (produto is null)
                return BadRequest();

            _uof.ProdutoRepository.Add(produto); // persiste na memória
            await _uof.Commit(); // salva do BD

            var _produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            // informa o produto salvo no header
            // Aciona a rota informada, com o ID informado
            return new CreatedAtRouteResult("BuscarProduto",
                new { id = produto.Id }, _produtoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.Id)
            {
                BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();

            return Ok(produtoDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        { 
            var produto = await _uof.ProdutoRepository.GetById(p => p.Id == id);
            if (produto is null )
            {
                return NotFound($"Produto não localizado! - ID: {id}");
            }

            _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }
    }
}
