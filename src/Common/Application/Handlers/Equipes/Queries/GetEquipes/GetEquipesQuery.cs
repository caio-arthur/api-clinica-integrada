
using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Gridify;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Equipes.Queries.GetEquipes
{
    public class GetEquipesQuery : GridifyQuery, IRequestWrapper<PaginatedList<ListEquipeDTO>>
    {
    }

    public class GetEquipesQueryHandler : IRequestHandlerWrapper<GetEquipesQuery, PaginatedList<ListEquipeDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEquipesQueryHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PaginatedList<ListEquipeDTO>>> Handle(GetEquipesQuery request, CancellationToken cancellationToken) {
            var mapper = new GridifyMapper<Equipe>()
                .GenerateMappings();

            var gridifyQueryable = _context.Equipes
                .Where(p => !p.IsDeleted)
                .Include(e => e.Profissionais)
                    .ThenInclude(ep => ep.Profissional)
                .GridifyQueryable(request, mapper);

            var query = gridifyQueryable.Query;
            var result = await query.AsNoTracking().ToListAsync(cancellationToken);

            var resultDTO = _mapper.Map<List<ListEquipeDTO>>(result);

            PaginatedList<ListEquipeDTO> equipes = new PaginatedList<ListEquipeDTO>(resultDTO, gridifyQueryable.Count, request.Page, request.PageSize);
            return ServiceResult.Success(equipes);
        }

    }

}
