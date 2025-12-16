using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Salas.Queries.GetConsultaSalaAtivas
{
    public class GetConsultaSalasAtivasQuery
        : IRequestWrapper<List<ConsultaSalaAtivaDTO>>
    {
        public Guid Id { get; set; }
    }

    public class GetConsultaSalasAtivasQueryHandler
        : IRequestHandlerWrapper<GetConsultaSalasAtivasQuery, List<ConsultaSalaAtivaDTO>>
    {
        private readonly IApplicationDbContext _context;

        public GetConsultaSalasAtivasQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<ConsultaSalaAtivaDTO>>> Handle(
            GetConsultaSalasAtivasQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var salasAtivas = await _context.Consultas
                    .Where(c => c.Status == ConsultaStatus.EmAndamento)
                    .Select(c => new ConsultaSalaAtivaDTO
                    {
                        ConsultaId = c.Id,
                        SalaId = c.Agendamento.SalaId
                    })
                    .ToListAsync(cancellationToken);

                return ServiceResult.Success(salasAtivas);
            }
            catch (Exception)
            {
                return ServiceResult.Failed<List<ConsultaSalaAtivaDTO>>(ServiceError.DefaultError);
            }
        }
    }
}
