using Application.DTOs;
using Application.Handlers.Agendamentos.Queries.GetAgendamentos;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Agendamentos.Queries.GetAgendamentosDoDiaByEspecialidade
{
    public class GetAgendamentosHojeByEspecialidadeQuery : GridifyQuery, IRequestWrapper<PaginatedList<AgendamentosHojeDTO>>
    {
        public Especialidade? Especialidade { get; set; }
        public AgendamentoTipo? Tipo { get; set; }
    }

    public class GetAgendamentosHojeByEspecialidadeHandler : IRequestHandlerWrapper<GetAgendamentosHojeByEspecialidadeQuery, PaginatedList<AgendamentosHojeDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAgendamentosHojeByEspecialidadeHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PaginatedList<AgendamentosHojeDTO>>> Handle(GetAgendamentosHojeByEspecialidadeQuery request, CancellationToken cancellationToken) {
            var mapper = new GridifyMapper<Agendamento>()
                .GenerateMappings()
                .AddMap("PacienteNome", agendamento => agendamento.Paciente != null ? agendamento.Paciente.Nome : (agendamento.NomeAluno ?? agendamento.NomeEquipe));

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var gridifyQueryable = _context.Agendamentos
                .Where(p => !p.IsDeleted &&
                            p.DataHoraInicio >= today &&
                            p.DataHoraInicio < tomorrow);

            if (request.Especialidade.HasValue && request.Especialidade != 0) {
                gridifyQueryable = gridifyQueryable.Where(p => p.Consulta.Especialidade == request.Especialidade.Value);
            }

            if (request.Tipo.HasValue && request.Tipo != 0) {
                gridifyQueryable = gridifyQueryable.Where(p => p.Tipo == request.Tipo.Value);
            }

            var query = gridifyQueryable.GridifyQueryable(request, mapper).Query;
            var result = await query.AsNoTracking().ToListAsync(cancellationToken);

            var resultDTO = _mapper.Map<List<AgendamentosHojeDTO>>(result);

            PaginatedList<AgendamentosHojeDTO> agendamentosHoje = new PaginatedList<AgendamentosHojeDTO>(resultDTO, gridifyQueryable.Count(), request.Page, request.PageSize);
            return ServiceResult.Success(agendamentosHoje);
        }
    }

}
