using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Gridify;
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
                .AddMap("DataAgendamento", c => c.Agendamento.DataHoraInicio.Date)
                .AddMap("Tipo", c => c.Agendamento.Tipo)
                .AddMap("Status", c => c.Status)
                .AddMap("Especialidade", c => c.Especialidade)
                .GenerateMappings();

            // [FIX] Apply base filters (like IsDeleted) BEFORE Gridify handles pagination
            var baseQuery = _context.Consultas
                .AsNoTracking()
                .Where(p => !p.IsDeleted);

            var gridifyQueryable = baseQuery.GridifyQueryable(request, mapper);

            var query = gridifyQueryable.Query;

            var resultDTO = await query
                .ProjectTo<ConsultaDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            PaginatedList<ConsultaDTO> consultas = new PaginatedList<ConsultaDTO>(resultDTO, gridifyQueryable.Count, request.Page, request.PageSize);
            return ServiceResult.Success(consultas);
        }
    }
}
