using Application.DTOs;
using Application.Handlers.Relatorio.Queries.GetRelatorio;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/relatorios")]
    [ApiController]
    public class RelatorioController : ApiControllerBase
    {
        [Authorize(Roles = "atendente")]
        [HttpGet]
        public async Task<ActionResult<ServiceResult<RelatorioDTO>>> GetRelatorio()
        {
            var result = await Mediator.Send(new GetRelatorioQuery());
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }   
    }
}
