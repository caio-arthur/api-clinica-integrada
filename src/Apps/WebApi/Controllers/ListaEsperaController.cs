using Application.DTOs;
using Application.Handlers.ListaEsperaEntries.Commands.Create;
using Application.Handlers.ListaEsperaEntries.Commands.Delete;
using Application.Handlers.ListaEsperaEntries.Commands.Update;
using Application.Handlers.ListaEsperaEntries.Queries.GetListaEsperaEntries;
using Application.Handlers.ListaEsperaEntries.Queries.GetListaEsperaEntryById;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/lista-espera")]
    [ApiController]
    public class ListaEsperaController : ApiControllerBase
    {
        //[Authorize(Roles = "atendente")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ListaEsperaEntryDTO>>> Get([FromQuery] GetListaEsperaEntriesQuery query) {
            return Ok(await Mediator.Send(query));
        }

        [Authorize(Roles = "atendente")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ListaEsperaEntryDTO>> GetById(Guid id) {
            var result = await Mediator.Send(new GetListaEsperaEntryByIdQuery { Id = id });
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "atendente")]
        [HttpPost("{pacienteId}")]
        public async Task<ActionResult<ListaEsperaEntryDTO>> Create(Guid pacienteId, CreateListaEsperaEntryCommand command) {
            command.PacienteId = pacienteId;
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
        [HttpPut("{id}")]
        public async Task<ActionResult<ListaEsperaEntryDTO>> Update(Guid id, UpdateListaEsperaEntryCommand command) {
            command.Id = id;
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
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(Guid id) {
            var result = await Mediator.Send(new DeleteListaEsperaEntryCommand { Id = id });
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
