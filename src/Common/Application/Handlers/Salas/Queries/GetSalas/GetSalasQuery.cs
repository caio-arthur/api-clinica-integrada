using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Salas.Queries.GetSalas
{
    public class GetSalasQuery : GridifyQuery, IRequestWrapper<PaginatedList<SalaDTO>>
    {
    }

    public class GetSalasHandler : IRequestHandlerWrapper<GetSalasQuery, PaginatedList<SalaDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSalasHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PaginatedList<SalaDTO>>> Handle(GetSalasQuery request, CancellationToken cancellationToken) {

            var mapper = new GridifyMapper<Sala>()
                .GenerateMappings();

            var gridifyQueryable = _context.Salas
                .Where(p => !p.IsDeleted)
                .GridifyQueryable(request, mapper);

            var query = gridifyQueryable.Query;
            var result = query.AsNoTracking().ToList();

            var resultDTO = _mapper.Map<List<SalaDTO>>(result);

            PaginatedList<SalaDTO> salas = new PaginatedList<SalaDTO>(resultDTO, gridifyQueryable.Count, request.Page, request.PageSize);
            return ServiceResult.Success(salas);
        }
    }

}
