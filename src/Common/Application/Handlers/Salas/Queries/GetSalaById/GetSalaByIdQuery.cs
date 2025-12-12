using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Salas.Queries.GetSalaById
{
    public class GetSalaByIdQuery : IRequestWrapper<SalaDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetSalaByIdQueryHandler : IRequestHandlerWrapper<GetSalaByIdQuery, SalaDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetSalaByIdQueryHandler(IApplicationDbContext context,
                                        IMapper mapper
                                        ) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<SalaDTO>> Handle(GetSalaByIdQuery request, CancellationToken cancellationToken) {

            try {
                var entity = await _context.Salas
                                           .Where(p => !p.IsDeleted) // Adiciona a condição para IsDeleted
                                           .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("Sala não encontrada");
                }

                var mappedEntity = _mapper.Map<SalaDTO>(entity);

                return ServiceResult.Success(mappedEntity);

            } catch (Exception e) {
                throw;
            }
        }

    }

}
