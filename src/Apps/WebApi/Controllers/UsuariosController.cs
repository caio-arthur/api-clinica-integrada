using Application.DTOs;
using Application.Handlers.Usuarios.Queries.GetUsuarios;
using Application.Models;
using Infrastructure.Identity.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioLogado _usuarioLogado;
        private readonly IMediator _mediator;

        public UsuariosController(IUsuarioService usuarioService,
            IUsuarioLogado usuarioLogado,
            IMediator mediator) {
            _usuarioService = usuarioService;
            _usuarioLogado = usuarioLogado;
            _mediator = mediator;

        }

        [Authorize(Roles = "atendente")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ListUsuarioDTO>>> Get([FromQuery] GetUsuariosQuery query) {
            return Ok(await _mediator.Send(query));
        }


        [Authorize]
        [HttpGet("informacao")]
        public async Task<ActionResult<UsuarioInfoDTO>> GetUsuario() {
            var usuario = await _usuarioService.GetById(_usuarioLogado.Id);

            if (usuario == null) {
                return NotFound("Usuário não encontrado.");
            }

            var usuarioViewModel = new UsuarioInfoDTO() {
                Id = usuario.Id,
                Name = usuario.Name,
                Email = usuario.Email,
                PhoneNumber = usuario.PhoneNumber,
                Roles = await _usuarioService.GetPerfisPorUsuario(usuario)
            };

            return Ok(usuarioViewModel);
        }

    }
}
