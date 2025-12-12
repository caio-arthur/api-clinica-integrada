using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Handlers.Profissionais.Commands.Create
{
    public class CreateProfissionalCommand : ProfissionalCommand, IRequest<ServiceResult>
    {

    }

    public class CreateProfissionalCommandHandler : IRequestHandler<CreateProfissionalCommand, ServiceResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProfissionalCommandHandler(
            IApplicationDbContext context,
            IMapper mapper
            ) {
            _context = context;
            _mapper = mapper;

        }

        public async Task<ServiceResult> Handle(CreateProfissionalCommand request, CancellationToken cancellationToken) {
            try {
                var entity = new Profissional {
                    Nome = request.Nome,
                    RA = request.RA,
                    Telefone = request.Telefone,
                    Email = request.Email,
                    Tipo = request.Tipo,
                    Especialidade = request.Especialidade
                };

                await _context.Profissionais.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return ServiceResult.Success(entity.Id);
            } catch (Exception ex) {
                await _context.RollBack();
                throw;
            }
        }
    }
}
