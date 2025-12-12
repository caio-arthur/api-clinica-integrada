using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Salas.Commands.Create
{
    public class CreateSalaCommand : SalaCommand, IRequest<ServiceResult>
    {

    }

    public class CreateSalaCommandHandler : IRequestHandler<CreateSalaCommand, ServiceResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateSalaCommandHandler(
            IApplicationDbContext context,
            IMapper mapper
            ) {
            _context = context;
            _mapper = mapper;

        }

        public async Task<ServiceResult> Handle(CreateSalaCommand request, CancellationToken cancellationToken) {
            try {
                var entity = new Sala {
                    Nome = request.Nome,
                    Especialidade = request.Especialidade,
                    Capacidade = request.Capacidade,
                    IsDisponivel = true
                };

                await _context.Salas.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success(entity.Id);
            } catch (Exception ex) {
                await _context.RollBack();
                throw;
            }
        }
    }

}
