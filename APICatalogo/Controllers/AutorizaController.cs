using APICatalogo.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AutorizaController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em : "
                + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO model)
        {
            // ApiController faz essa validação de forma automática
            /*if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));*/

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, false);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email,
                userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido!");
                return BadRequest(ModelState);
            }
        }
    }
}
