using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Consultas.Queries.GetConsultaById
{
    public class GetConsultaByIdQuery : IRequestWrapper<ConsultaDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetConsultaByIdQueryHandler : IRequestHandlerWrapper<GetConsultaByIdQuery, ConsultaDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetConsultaByIdQueryHandler(IApplicationDbContext context,
            IMapper mapper
            ) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ConsultaDTO>> Handle(GetConsultaByIdQuery request, CancellationToken cancellationToken) {
            var result = await _context.Consultas
                .Where(p => !p.IsDeleted && p.Id == request.Id)
                .AsNoTracking()
                .ProjectTo<ConsultaDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null) {
                throw new Exception("Consulta não encontrada");
            }

            return ServiceResult.Success(result);
        }
    }
}
