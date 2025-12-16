using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Agendamentos.Queries.GetAgendamentos
{
    public class GetAgendamentosQuery : GridifyQuery, IRequestWrapper<PaginatedList<AgendamentoDTO>>
    {
    }

    public class GetAgendamentosHandler : IRequestHandlerWrapper<GetAgendamentosQuery, PaginatedList<AgendamentoDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAgendamentosHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PaginatedList<AgendamentoDTO>>> Handle(GetAgendamentosQuery request, CancellationToken cancellationToken) {



            var mapper = new GridifyMapper<Agendamento>()
                .AddMap("PacienteNome", agendamento => agendamento.Paciente != null ? agendamento.Paciente.Nome : (agendamento.NomePaciente ?? agendamento.NomeEquipe))
                .AddMap("Especialidade", agendamento => agendamento.Consulta.Especialidade)
                .AddMap("DataAgendamento", ag => ag.DataHoraInicio)
                .GenerateMappings()
                ;

            var gridifyQueryable = _context.Agendamentos
                .Include(p => p.Paciente)
                .Include(p => p.Sala)
                .Include(p => p.Consulta)
                    .ThenInclude(c => c.Equipe)
                .Where(p => !p.IsDeleted)
                .GridifyQueryable(request, mapper);

            var query = gridifyQueryable.Query;
            var result = query.AsNoTracking().ToList();

            var resultDTO = _mapper.Map<List<AgendamentoDTO>>(result);

            PaginatedList<AgendamentoDTO> agendamentos = new PaginatedList<AgendamentoDTO>(resultDTO, gridifyQueryable.Count, request.Page, request.PageSize);
            return ServiceResult.Success(agendamentos);
        }
    }



}

