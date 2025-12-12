using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Gridify;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.ListaEsperaEntries.Queries.GetListaEsperaEntries
{
    public class GetListaEsperaEntriesQuery : GridifyQuery, IRequest<PaginatedList<ListaEsperaEntryDTO>>
    {
        public class GetListaEsperaQueryHandler : IRequestHandler<GetListaEsperaEntriesQuery, PaginatedList<ListaEsperaEntryDTO>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetListaEsperaQueryHandler(IApplicationDbContext context, IMapper mapper) {
                _context = context;
                _mapper = mapper;
            }


            public Task<PaginatedList<ListaEsperaEntryDTO>> Handle(GetListaEsperaEntriesQuery request, CancellationToken cancellationToken) {
                var mapper = new GridifyMapper<ListaEspera>()
                    .AddMap("PacienteNome", listaEspera => listaEspera.Paciente.Nome)
                    .GenerateMappings();

                var gridifyQueryable = _context.ListaEspera
                    .Where(p => !p.IsDeleted)
                    .Include(p => p.Paciente)
                    .GridifyQueryable(request, mapper);

                var query = gridifyQueryable.Query;
                var result = query.AsNoTracking().ToList();

                var resultDTO = _mapper.Map<List<ListaEsperaEntryDTO>>(result);

                PaginatedList<ListaEsperaEntryDTO> listaEsperaEntries = new PaginatedList<ListaEsperaEntryDTO>(resultDTO, gridifyQueryable.Count, request.Page, request.PageSize);
                return Task.FromResult(listaEsperaEntries);
            }
        }

    }
}
