using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Gridify;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Profissionais.Queries.GetProfissionais
{
    public class GetProfissionaisQuery : GridifyQuery, IRequestWrapper<PaginatedList<ProfissionalDTO>>
    {


    }

    public class GetProfissionaisHandler : IRequestHandlerWrapper<GetProfissionaisQuery, PaginatedList<ProfissionalDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProfissionaisHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PaginatedList<ProfissionalDTO>>> Handle(GetProfissionaisQuery request, CancellationToken cancellationToken) {

            var mapper = new GridifyMapper<Profissional>()
                .GenerateMappings();

            var gridifyQueryable = _context.Profissionais
                .Where(p => !p.IsDeleted)
                .GridifyQueryable(request, mapper);

            var query = gridifyQueryable.Query;
            var result = query.AsNoTracking().ToList();

            var resultDTO = _mapper.Map<List<ProfissionalDTO>>(result);

            PaginatedList<ProfissionalDTO> profissionais = new PaginatedList<ProfissionalDTO>(resultDTO, gridifyQueryable.Count, request.Page, request.PageSize);
            return ServiceResult.Success(profissionais);
        }


    }



}
