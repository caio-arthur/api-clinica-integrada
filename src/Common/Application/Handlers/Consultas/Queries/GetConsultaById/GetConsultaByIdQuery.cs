using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Consultas.Queries.GetConsultaById
{
    public class GetConsultaByIdQuery : IRequestWrapper<ConsultaDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetConsultaByIdQueryHandler : IRequestHandlerWrapper<GetConsultaByIdQuery, ConsultaDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetConsultaByIdQueryHandler(IApplicationDbContext context,
            IMapper mapper
            ) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ConsultaDTO>> Handle(GetConsultaByIdQuery request, CancellationToken cancellationToken) {
            try {
                var entity = await _context.Consultas
                    .Where(p => !p.IsDeleted)
                    .Include(p => p.Agendamento)
                        .ThenInclude(p => p.Paciente)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("Consulta não encontrada");
                }

                var result = _mapper.Map<ConsultaDTO>(entity);

                return ServiceResult.Success(result);
            } catch (Exception e) {
                throw;
            }
        }
    }
}
