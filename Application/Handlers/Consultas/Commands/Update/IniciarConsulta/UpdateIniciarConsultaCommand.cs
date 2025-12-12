using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Consultas.Commands.Update.IniciarConsulta
{
    public class UpdateIniciarConsultaCommand : IRequest<ServiceResult>
    {
        public Guid ConsultaId { get; set; }
    }

    public class UpdateIniciarConsultaCommandHandler : IRequestHandler<UpdateIniciarConsultaCommand, ServiceResult>
    {
        private readonly ISender _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateIniciarConsultaCommandHandler(IApplicationDbContext context,
            IMapper mapper,
            ISender sender) 
            {
            _context = context;
            _mapper = mapper;
            _mediator = sender;
        }

        public async Task<ServiceResult> Handle(UpdateIniciarConsultaCommand request, CancellationToken cancellationToken) {
            try {
                var consulta = await _context.Consultas
                    .Include(c => c.Agendamento) // Inclui o Agendamento relacionado
                    .FirstOrDefaultAsync(c => c.Id == request.ConsultaId);

                if (consulta == null) {
                    throw new Exception(nameof(Consulta));
                }
                var agendamento = await _context.Agendamentos.FindAsync(consulta.AgendamentoId);
                agendamento.Status = AgendamentoStatus.Concluido;
                consulta.Status = ConsultaStatus.EmAndamento;
                consulta.DataHoraInicio = DateTime.Now; // O Horário é registrado

                //Bloquear Sala
                var salaConsulta = await _context.Salas.FirstOrDefaultAsync(x => x.Id == consulta.Agendamento.SalaId);
                if (salaConsulta != null) {
                    salaConsulta.IsDisponivel = false;
                }

                await _context.SaveChangesAsync(cancellationToken);

                var result = salaConsulta != null ? "Sala Bloqueada" : "Ok";

                return ServiceResult.Success(result);
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
