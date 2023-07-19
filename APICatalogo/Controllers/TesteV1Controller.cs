using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    //[ApiVersion("1.0", Deprecated = true)] // mostra no header q está obsoleta
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")] // atende duas versões, a depender do método action
    [Route("teste")]
    //[Route("v{v:apiVersion}/teste")] // localhost:port/version/teste - adicionando no cabeçalho (ApiVersionReader), não é necessário essa instrução
    [ApiController]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>TesteV1Controller - V1.0 </h2></body></html>", "text/html");
        }

/*        [HttpGet, MapToApiVersion("2.0")] // atende somente a versão 2, mas ese método não é muito recomendado
        public IActionResult Get2()
        {
            return Content("<html><body><h2>TesteV1Controller - GET V2.0 </h2></body></html>", "text/html");
        }*/
    }
}
