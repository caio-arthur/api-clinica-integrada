using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Gridify;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Pacientes.Queries.GetPacientes
{
    public class GetPacientesQuery : GridifyQuery, IRequestWrapper<PaginatedList<PacienteDTO>>
    {

    }

    public class GetPacientesQueryHandler : IRequestHandlerWrapper<GetPacientesQuery, PaginatedList<PacienteDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPacientesQueryHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PaginatedList<PacienteDTO>>> Handle(GetPacientesQuery request, CancellationToken cancellationToken) {

            var mapper = new GridifyMapper<Paciente>()
                .GenerateMappings();

            var gridifyQueryable = _context.Pacientes
                .Where(p => !p.IsDeleted)
                .GridifyQueryable(request, mapper);

            var query = gridifyQueryable.Query;
            var result = query.AsNoTracking().ToList();

            var resultDTO = _mapper.Map<List<PacienteDTO>>(result);

            PaginatedList<PacienteDTO> pacientes = new PaginatedList<PacienteDTO>(resultDTO, gridifyQueryable.Count, request.Page, request.PageSize);
            return ServiceResult.Success(pacientes);
        }
    }

}
