using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Pacientes.Queries.GetPacienteById
{
    public class GetPacienteByIdQuery : IRequestWrapper<PacienteDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetPacienteByIdQueryHandler : IRequestHandlerWrapper<GetPacienteByIdQuery, PacienteDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPacienteByIdQueryHandler(IApplicationDbContext context,
                                            IMapper mapper

                                            ) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<PacienteDTO>> Handle(GetPacienteByIdQuery request, CancellationToken cancellationToken) {

            try {
                var entity = await _context.Pacientes
                                           .Where(p => !p.IsDeleted) // Adiciona a condição para IsDeleted
                                           .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("No se encontró el paciente");
                }

                var mappedEntity = _mapper.Map<PacienteDTO>(entity);

                return ServiceResult.Success(mappedEntity);

            } catch (Exception e) {
                throw;
            }
        }

    }
}

