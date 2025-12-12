using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Consultas.Queries.GetConsultaRelatorio
{
    public class GetConsultaRelatorioQuery : IRequestWrapper<RelatorioConsultaDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetConsultaRelatorioQueryHandler : IRequestHandlerWrapper<GetConsultaRelatorioQuery, RelatorioConsultaDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetConsultaRelatorioQueryHandler(IApplicationDbContext context,
            IMapper mapper
            ) {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResult<RelatorioConsultaDTO>> Handle(GetConsultaRelatorioQuery request, CancellationToken cancellationToken) {
            try {
                var entity = await _context.Consultas
                    .Include(p => p.Agendamento)
                    .Include(p => p.Equipe)
                    .Include(p => p.Agendamento.Paciente)
                    .Include(p => p.Agendamento.Sala)
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);
                if (entity == null) {
                    throw new Exception("Consulta não encontrada");
                }
                var result = _mapper.Map<RelatorioConsultaDTO>(entity);
                return ServiceResult.Success(result);
            } catch (Exception e) {
                throw;
            }
        }
    }
}
