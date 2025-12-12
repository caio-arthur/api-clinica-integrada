using Application.DTOs;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Equipes.Queries.GetEquipeById
{
    public class GetEquipeByIdQuery : IRequestWrapper<EquipeDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetEquipeByIdQueryHandler : IRequestHandlerWrapper<GetEquipeByIdQuery, EquipeDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEquipeByIdQueryHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<EquipeDTO>> Handle(GetEquipeByIdQuery request, CancellationToken cancellationToken) {

            try {
                var entity = await _context.Equipes
                                           .Where(p => !p.IsDeleted) // Adiciona a condição para IsDeleted
                                           .Include(p => p.Profissionais)
                                                .ThenInclude(p => p.Profissional)
                                           .FirstOrDefaultAsync(p => p.Id == request.Id
                                           ,cancellationToken: cancellationToken);

                if (entity == null) {
                    return ServiceResult.Failed<EquipeDTO>(ServiceError.CustomMessage("Entidade não encontrada."));
                }

                var mappedEntity = _mapper.Map<EquipeDTO>(entity);

                return ServiceResult.Success(mappedEntity);

            } catch (Exception) {
                throw;
            }
        }


    }

}
