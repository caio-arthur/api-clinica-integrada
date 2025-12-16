using Application.Handlers.Usuarios.Commands;
using Application.Handlers.Usuarios.Commands.CreateUsuario;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ApiControllerBase
    {
        public AuthenticateController()
        {
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await Mediator.Send(command);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "atendente")]
        [HttpPost("usuario/atendente")]
        public async Task<ActionResult<ServiceResult<bool>>> CreateUsuarioAtendente([FromBody] CreateUsuarioAtendenteCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
