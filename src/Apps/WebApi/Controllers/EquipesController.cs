using Application.DTOs;
using Application.Handlers.Equipes.Commands.Create;
using Application.Handlers.Equipes.Commands.Delete;
using Application.Handlers.Equipes.Commands.Update;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Handlers.Equipes.Queries.GetEquipes;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/equipes")]
    [ApiController]
    public class EquipesController : ApiControllerBase
    {
        [Authorize(Roles = "atendente")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ListEquipeDTO>>> Get([FromQuery] GetEquipesQuery query) {
            return Ok(await Mediator.Send(query));
        }

        [Authorize(Roles = "atendente")]
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipeDTO>> GetById(Guid id) {
            var result = await Mediator.Send(new GetEquipeByIdQuery { Id = id });
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "atendente")]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateEquipe(CreateEquipeCommand command) {
            try {
                var result = await Mediator.Send(command);
                if (!result.Succeeded) {
                    return BadRequest(result);
                }
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "atendente")]
        [HttpPut("{id}/inserir-profissional/{profissionalId}")]
        public async Task<ActionResult> InserirProfissional(Guid id, Guid profissionalId) {
            try {
                var result = await Mediator.Send(new InserirProfissionalEquipeCommand { EquipeId = id, ProfissionalId = profissionalId });
                if (!result.Succeeded) {
                    return BadRequest(result);
                }
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "atendente")]
        [HttpPut("{id}/remover-profissional/{profissionalId}")]
        public async Task<ActionResult> RemoverProfissional(Guid id, Guid profissionalId) {
            try {
                var result = await Mediator.Send(new RemoverProfissionalEquipeCommand { EquipeId = id, ProfissionalId = profissionalId });
                if (!result.Succeeded) {
                    return BadRequest(result);
                }
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "atendente")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) {
            var result = await Mediator.Send(new DeleteEquipeCommand { Id = id });
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
