using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Consultas.Commands.Create
{
    public class CreateConsultaCommand : ConsultaCommand, IRequest<ServiceResult>
    {

    }

    public class CreateConsultaCommandHandler : IRequestHandler<CreateConsultaCommand, ServiceResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateConsultaCommandHandler(
            IApplicationDbContext context,
            IMapper mapper
            ) {
            _context = context;
            _mapper = mapper;

        }

        public async Task<ServiceResult> Handle(CreateConsultaCommand request, CancellationToken cancellationToken) {
            try {
                var entity = new Consulta {
                    Observacao = request.Observacao,
                    DataHoraInicio = request.DataHoraInicio,
                    DataHoraFim = request.DataHoraFim,
                    Especialidade = request.Especialidade,
                    Status = request.Status,
                    AgendamentoId = request.AgendamentoId,
                    EquipeId = request.EquipeId,
                };

                await _context.Consultas.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success("Ok");
            } catch (Exception ex) {
                await _context.RollBack();
                throw;
            }
        }
    }
}
