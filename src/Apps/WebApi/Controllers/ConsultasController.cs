using Application.DTOs;
using Application.Handlers.Consultas.Commands.AtualizarObservacoes;
using Application.Handlers.Consultas.Commands.Delete;
using Application.Handlers.Consultas.Commands.Update.FinalizarConsulta;
using Application.Handlers.Consultas.Commands.Update.FinalizarTriagem;
using Application.Handlers.Consultas.Commands.Update.IniciarConsulta;
using Application.Handlers.Consultas.Commands.Update.IniciarTriagem;
using Application.Handlers.Consultas.Commands.Update.UpdateConsulta;
using Application.Handlers.Consultas.Queries.GetConsultaById;
using Application.Handlers.Consultas.Queries.GetConsultaRelatorio;
using Application.Handlers.Consultas.Queries.GetConsultas;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/consultas")]
    [ApiController]
    public class ConsultasController : ApiControllerBase
    {
        [Authorize(Roles = "atendente")]
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ConsultaDTO>>> Get([FromQuery] GetConsultasQuery query) {
            return Ok(await Mediator.Send(query));
        }

        [Authorize(Roles = "atendente")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultaDTO>> GetById(Guid id) {
            var result = await Mediator.Send(new GetConsultaByIdQuery { Id = id });
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "atendente")]
        [HttpGet("{id}/relatorio")]
        public async Task<ActionResult<RelatorioConsultaDTO>> GetRelatorio(Guid id) {
            var result = await Mediator.Send(new GetConsultaRelatorioQuery { Id = id });
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "atendente")]
        [HttpPut("{id}/triagem/iniciar")]
        public async Task<ActionResult> IniciarTriagem(Guid id) {
            try {
                var result = await Mediator.Send(new UpdateIniciarTriagemCommand { ConsultaId = id });
                if (!result.Succeeded) {
                    return BadRequest(result);
                }
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "atendente")]
        [HttpPut("{id}/triagem/finalizar")]
        public async Task<ActionResult> FinalizarTriagem(Guid id) {
            try {
                var result = await Mediator.Send(new UpdateFinalizarTriagemCommand { ConsultaId = id });
                if (!result.Succeeded) {
                    return BadRequest(result);
                }
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "atendente")]
        [HttpPut("{id}/iniciar")]
        public async Task<ActionResult> IniciarConsulta(Guid id) 
        {
            try {
                var result = await Mediator.Send(new UpdateIniciarConsultaCommand { ConsultaId = id });
                if (!result.Succeeded) {
                    return BadRequest(result);
                }
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "atendente")]
        [HttpPut("{id}/finalizar")]
        public async Task<ActionResult> FinalizarConsulta(Guid id) {
            try {
                var result = await Mediator.Send(new UpdateFinalizarConsultaCommand { ConsultaId = id });
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
            var result = await Mediator.Send(new DeleteConsultaCommand { Id = id });
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = "atendente")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ConsultaDTO>> Update(Guid id, [FromBody] UpdateConsultaCommand command) {
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

        [Authorize(Roles ="atendente")]
        [HttpPut("{id}/observacoes")]
        public async Task<ActionResult<ServiceResult<ConsultaDTO>>> UpdateObservacoes(Guid id, [FromBody] OrquestradorConsultaCommand command) {
            try {
                command.ConsultaId = id;
                var result = await Mediator.Send(command);
                if (!result.Succeeded) {
                    return BadRequest(result);
                }
                return Ok(result);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

    }
}
