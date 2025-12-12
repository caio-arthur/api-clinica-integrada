using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Gridify;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Consultas.Queries.GetConsultas
{
    public class GetConsultasQuery : GridifyQuery, IRequestWrapper<PaginatedList<ConsultaDTO>>
    {

    }


    public class GetConsultasHandler : IRequestHandlerWrapper<GetConsultasQuery, PaginatedList<ConsultaDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetConsultasHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PaginatedList<ConsultaDTO>>> Handle(GetConsultasQuery request, CancellationToken cancellationToken) {

            var mapper = new GridifyMapper<Consulta>()
                .AddMap("PacienteNome", c => c.Agendamento.Paciente.Nome)
                .AddMap("PacienteId", c => c.Agendamento.PacienteId)
                .AddMap("Tipo", c => c.Agendamento.Tipo)
                .GenerateMappings();

            var gridifyQueryable = _context.Consultas
                .Where(p => !p.IsDeleted)
                .Include(p => p.Agendamento)
                    .ThenInclude(p => p.Paciente)
                .GridifyQueryable(request, mapper);

            var query = gridifyQueryable.Query;
            var result = await query.AsNoTracking().ToListAsync(cancellationToken);

            var resultDTO = _mapper.Map<List<ConsultaDTO>>(result);

            PaginatedList<ConsultaDTO> consultas = new PaginatedList<ConsultaDTO>(resultDTO, gridifyQueryable.Count, request.Page, request.PageSize);
            return ServiceResult.Success(consultas);
        }
    }
}
