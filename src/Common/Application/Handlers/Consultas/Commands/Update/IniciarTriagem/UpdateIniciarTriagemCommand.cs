using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Consultas.Commands.Update.IniciarTriagem
{
    public class UpdateIniciarTriagemCommand : IRequest<ServiceResult>
    {
        public Guid ConsultaId { get; set; }
    }

    public class UpdateIniciarTriagemCommandHandler : IRequestHandler<UpdateIniciarTriagemCommand, ServiceResult>
    {
        private readonly ISender _mediator;
        private readonly IApplicationDbContext _context;
        public UpdateIniciarTriagemCommandHandler(IApplicationDbContext context, ISender mediator) {
            _mediator = mediator;
            _context = context;
        }
        public async Task<ServiceResult> Handle(UpdateIniciarTriagemCommand request, CancellationToken cancellationToken) {
            try {
                var consulta = await _context.Consultas.FindAsync(request.ConsultaId);
                if (consulta == null) {
                    throw new Exception(nameof(Consulta));
                }
                var agendamento = await _context.Agendamentos.FindAsync(consulta.AgendamentoId);
                agendamento.Status = AgendamentoStatus.Concluido;
                consulta.Status = ConsultaStatus.Triagem;

                //Bloquear Sala
                var salaConsulta = await _context.Salas.FirstOrDefaultAsync(x => x.Id == consulta.Agendamento.SalaId);
                if (salaConsulta != null) {
                    salaConsulta.IsDisponivel = false;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return ServiceResult.Success("Triagem Iniciada");
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
