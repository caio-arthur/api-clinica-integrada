using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.ListaEsperaEntries.Queries.GetListaEsperaEntryById
{
    public class GetListaEsperaEntryByIdQuery : IRequestWrapper<ListaEsperaEntryDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetListaEsperaEntryByIdQueryHandler : IRequestHandlerWrapper<GetListaEsperaEntryByIdQuery, ListaEsperaEntryDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetListaEsperaEntryByIdQueryHandler(IApplicationDbContext context,
                                            IMapper mapper

                                            ) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ListaEsperaEntryDTO>> Handle(GetListaEsperaEntryByIdQuery request, CancellationToken cancellationToken) {

            try {
                var entity = await _context.ListaEspera
                                           .Where(p => !p.IsDeleted) // Adiciona a condição para IsDeleted
                                           .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("No se encontró la entrada de la lista de espera");
                }

                var mappedEntity = _mapper.Map<ListaEsperaEntryDTO>(entity);

                return ServiceResult.Success(mappedEntity);

            } catch (Exception e) {
                throw;
            }
        }
    }
}
