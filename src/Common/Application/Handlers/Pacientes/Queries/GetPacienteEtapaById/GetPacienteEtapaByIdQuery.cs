using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Pacientes.Queries.GetPacienteEtapaById
{
    public class GetPacienteEtapaByIdQuery : IRequestWrapper<PacienteEtapa>
    {
        public Guid Id { get; set; }
    }

    public class GetPacienteEtapaByIdQueryHandler : IRequestHandlerWrapper<GetPacienteEtapaByIdQuery, PacienteEtapa>
    {
        private readonly IApplicationDbContext _context;

        public GetPacienteEtapaByIdQueryHandler(IApplicationDbContext context) {
            _context = context;
        }

        public async Task<ServiceResult<PacienteEtapa>> Handle(GetPacienteEtapaByIdQuery request, CancellationToken cancellationToken) {
            var paciente = await _context.Pacientes
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (paciente == null) {
                return ServiceResult.Failed<PacienteEtapa>(ServiceError.NotFound);
            }
           
            return ServiceResult.Success(paciente.Etapa);
        }
    }
}
