using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Consultas.Commands.Update.FinalizarTriagem
{
    public class UpdateFinalizarTriagemCommand : IRequest<ServiceResult>
    {
        public Guid ConsultaId { get; set; }
    }

    public class UpdateFinalizarTriagemCommandHandler : IRequestHandler<UpdateFinalizarTriagemCommand, ServiceResult>
    {
        private readonly ISender _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UpdateFinalizarTriagemCommandHandler(IApplicationDbContext context, IMapper mapper, ISender mediator) {
            _mediator = mediator;
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(UpdateFinalizarTriagemCommand request, CancellationToken cancellationToken) {
            try {
                var consulta = await _context.Consultas
                    .Include(c => c.Agendamento)
                    .FirstOrDefaultAsync(c => c.Id == request.ConsultaId);
                if (consulta == null) {
                    throw new Exception(nameof(Consulta));
                }
                consulta.Status = ConsultaStatus.AguardandoConsulta;

                //Liberar Sala
                var salaConsulta = await _context.Salas.FirstOrDefaultAsync(x => x.Id == consulta.Agendamento.SalaId);
                if (salaConsulta != null) {
                    salaConsulta.IsDisponivel = true;
                }

                await _context.SaveChangesAsync(cancellationToken);
                return ServiceResult.Success("Triagem Finalizada");
            } catch (Exception ex) {
                throw;
            }
        }
    }
}
