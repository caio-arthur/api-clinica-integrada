using Application.DTOs;
using Application.Handlers.Consultas.Commands.Update.FinalizarConsulta;
using Application.Handlers.Consultas.Commands.Update.IniciarConsulta;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Application.Handlers.Consultas.Commands.AtualizarObservacoes
{
    public class OrquestradorConsultaCommand : IRequestWrapper<ConsultaDTO>
    {
        [JsonIgnore]
        public Guid ConsultaId { get; set; }
        public string Observacoes { get; set; }
    }

    public class AtualizarObservacoesCommandHandler : IRequestHandlerWrapper<OrquestradorConsultaCommand, ConsultaDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISender _mediator;
        public AtualizarObservacoesCommandHandler(IApplicationDbContext context, IMapper mapper, ISender mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<ServiceResult<ConsultaDTO>> Handle(OrquestradorConsultaCommand request, CancellationToken cancellationToken)
        {
            var consulta = await _context.Consultas
                .Include(c => c.Agendamento)
                    .ThenInclude(a => a.Paciente)
                .FirstOrDefaultAsync(c => c.Id == request.ConsultaId, cancellationToken);

            if (consulta == null)
                return ServiceResult.Failed<ConsultaDTO>(ServiceError.NotFound);

            consulta.Observacao = request.Observacoes;

            if (consulta.Status == ConsultaStatus.Agendada)
            {
                await _mediator.Send(new UpdateIniciarConsultaCommand
                {
                    ConsultaId = request.ConsultaId
                }, cancellationToken);
            }
            else if (consulta.Status == ConsultaStatus.EmAndamento) 
            {
                await _mediator.Send(new UpdateFinalizarConsultaCommand
                {
                    ConsultaId = request.ConsultaId,
                }, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<ConsultaDTO>(consulta);
            return ServiceResult.Success(result);
        }

    }
}
