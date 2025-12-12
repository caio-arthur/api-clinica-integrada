using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Handlers.Equipes.Commands.Update
{
    public class RemoverProfissionalEquipeCommand : IRequest<ServiceResult>
    {
        public Guid EquipeId { get; set; }
        public Guid ProfissionalId { get; set; }
    }

    public class RemoverProfissionalEquipeCommandHandler : IRequestHandler<RemoverProfissionalEquipeCommand, ServiceResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public RemoverProfissionalEquipeCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IDateTime dateTime
            ) {
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;

        }

        public async Task<ServiceResult> Handle(RemoverProfissionalEquipeCommand request, CancellationToken cancellationToken) {

            var entity = _context.EquipeProfissional.FirstOrDefault(ep => ep.EquipeId == request.EquipeId && ep.ProfissionalId == request.ProfissionalId);
            if (entity == null) {
                return ServiceResult.Failed(ServiceError.CustomMessage("Profissional não encontrado na equipe"));
            }

            _context.EquipeProfissional.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success("Ok");
        }
    }
}
