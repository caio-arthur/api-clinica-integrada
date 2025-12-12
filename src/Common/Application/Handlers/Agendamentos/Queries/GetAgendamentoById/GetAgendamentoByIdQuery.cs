using Application.DTOs;
using Application.Handlers.Equipes.Queries.GetEquipeById;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Agendamentos.Queries.GetAgendamentoById
{
    public class GetAgendamentoByIdQuery : IRequestWrapper<AgendamentoDTO>
    {
        public Guid Id { get; set; }
    }

    public class GetAgendamentoByIdQueryHandler : IRequestHandlerWrapper<GetAgendamentoByIdQuery, AgendamentoDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAgendamentoByIdQueryHandler(IApplicationDbContext context,
                                            IMapper mapper

                                            ) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<AgendamentoDTO>> Handle(GetAgendamentoByIdQuery request, CancellationToken cancellationToken) {

            try {
                var entity = await _context.Agendamentos
                                            .Include(p => p.Paciente)
                                            .Include(p => p.Sala)
                                            .Include(p => p.Consulta)
                                           .Where(p => !p.IsDeleted) // Adiciona a condição para IsDeleted
                                           .FirstOrDefaultAsync(p => p.Id == request.Id);

                if (entity == null) {
                    throw new Exception("Agendamento não encontrado");
                }

                var mappedEntity = _mapper.Map<AgendamentoDTO>(entity);

                return ServiceResult.Success(mappedEntity);

            } catch (Exception e) {
                throw;
            }
        }

    }

}
