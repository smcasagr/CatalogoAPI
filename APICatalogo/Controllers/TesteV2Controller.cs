using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiVersion("2.0")]
    //[Route("v{v:apiVersion}/teste")]
    [Route("teste2")] // remover o 2 para testar - se deixar, o Swagger não roda direito
    [ApiController]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>TesteV2Controller - V2.0 </h2></body></html>", "text/html");
        }
    }
}
