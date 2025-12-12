using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Profissionais.Queries.GetProfissionalById
{
    public class GetProfissionalByIdQuery : IRequestWrapper<ProfissionalDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetProfissionalByIdQueryHandler : IRequestHandlerWrapper<GetProfissionalByIdQuery, ProfissionalDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProfissionalByIdQueryHandler(IApplicationDbContext context,
                                            IMapper mapper

                                            ) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ProfissionalDTO>> Handle(GetProfissionalByIdQuery request, CancellationToken cancellationToken) {

            try {
                var entity = await _context.Profissionais
                                           .Where(p => !p.IsDeleted) // Adiciona a condição para IsDeleted
                                           .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("No se encontró el profissional");
                }

                var mappedEntity = _mapper.Map<ProfissionalDTO>(entity);

                return ServiceResult.Success(mappedEntity);

            } catch (Exception e) {
                throw;
            }
        }

    }
}
